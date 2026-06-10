using AutoMapper;
using ComfyJamSummer.AutoMapper;
using ComfyJamSummer.Data;
using ComfyJamSummer.Data.Jsons;
using ComfyJamSummer.Entities;
using ComfyJamSummer.Entities.Base;
using ComfyJamSummer.Entities.Configs;
using ComfyJamSummer.Entities.Debuffs;
using ComfyJamSummer.Entities.TextureData;
using ComfyJamSummer.Enums;
using ComfyJamSummer.Helpers;
using ComfyJamSummer.JsonsData.Xnb;
using ComfyJamSummer.PoolObjects;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Nez;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ComfyJamSummer.Prefab
{
    public class Prefabs : SceneComponent
    {
        readonly IMapper _mapper;

        public Prefabs()
        {
            _mapper = AutoMapperConfig.RegisterMappings();
        }

        #region data
        public LogoData LogoData { get { return _dataLogo; } }

        public PlayerData PlayerData { get { return _dataPlayer; } }

        public PlayerConfig PlayerConfig => _playerConfig;

        public EnemyConfig EnemyConfig => _enemyConfig;

        public CreatureConfig CreatureConfig => _creatureConfig;

        LogoData _dataLogo;
        PlayerData _dataPlayer;
        EnemyData _dataEnemy;
        NpcObjectData _dataNpc;
        MapData _dataMap;
        UIData _dataUI;

        CreatureConfig _creatureConfig;
        PlayerConfig _playerConfig;
        EnemyConfig _enemyConfig;
        #endregion

        #region objects
        public List<GameTextJsonXnbData<GameTextJsonData>> GameTextJsonData;

        GameTextJsonXnbData<GameTextJsonData> _uiGameTextJsonData;

        public StarFish StarFish { get; private set; }

        public Rocket Rocket { get; private set; }

        public Crab Crab { get; private set; }

        Player Player;

        Gun Gun;

        public Bullet Bullet { get; private set; }

        public Island Island { get; private set; }

        Enemy Enemy;
        Bullet BulletEnemy;
        public Shadow Shadow;
        List<DebuffIcon> DebuffsIcons;
        List<CustomTextureData> _uiTextures;
        List<CustomTextureData> UITextures;
        #endregion

        #region music and sound fxs
        public List<SoundPrefab> SoundFXs;

        private List<SoundPrefab> Musics;
        #endregion

        public void FastLoad()
        {
            CreateDatas();
        }

        public void Load()
        {
            CreateMap();

            CreatePlayer();

            CreateEnemies();

            CreateUIContent();

            CreateGeneral();
        }

        void CreateDatas()
        {
            _dataLogo = new LogoData(this, _mapper);
            _dataNpc = new NpcObjectData(this, _mapper);
            _dataPlayer = new PlayerData(this, _mapper);
            _dataEnemy = new EnemyData(this, _mapper);
            _dataMap = new MapData(this, _mapper);

            _playerConfig = new PlayerConfig();
            _enemyConfig = new EnemyConfig();
        }

        void CreateGameTexts()
        {
            //GameTextJsonData = _textData.CreateAllTexts();

            //PlayerGameTextJsonData = GameTextJsonData.FirstOrDefault(x => x.ArchiveName == GameTextsArchivesNamesEnum.player);
        }

        void CreatePlayer()
        {
            Player = _dataPlayer.Create();
            Gun = _dataPlayer.CreateGun();
            Bullet = _dataPlayer.CreateBullet();
            Shadow = new Shadow();
        }

        void CreateEnemies()
        {
            Enemy = _dataEnemy.Create();
            BulletEnemy = _dataEnemy.CreateBullet();
        }

        void CreateMap()
        {
            Island = _dataMap.Create();

            Crab = _dataNpc.CreateCrab();
            Rocket = _dataNpc.CreateRocket();
            StarFish = _dataNpc.CreateStarFish();
        }

        void CreateUIContent()
        {
            _uiTextures = _dataUI?.CreateAll();
        }

        void CreateGeneral()
        {
        }

        public Player GetPlayer(Vector2 pos)
        {
            if (Gun == null)
            {
                return null;
            }

            var player = Player?.ClonePlayer(_dataPlayer.InitializePlayer(pos));

            if (player != null)
            {
                var gun = Gun.CloneGun(player, player.Position);

                player.Gun = gun;
            }

            return player;
        }

        public Enemy GetEnemy(EnemyType type, Vector2 pos)
        {
            var player = Enemy?.CloneEnemy(_dataEnemy.InitializeEnemy(type, pos));

            return player;
        }

        public DebuffIcon GetDebuffIcon(Debuff debuff, Vector2 offset)
        {
            try
            {
                var selected = DebuffsIcons.FirstOrDefault(x => x.Type == debuff.Type);

                if (selected != null)
                {
                    return selected.Clonar(debuff.Duration, offset);
                }
            }
            catch (Exception ex)
            {
            }

            return null;
        }

        #region sound and music stuff
        public SoundPrefab GetSoundFxPrefab(SoundFxName name, float overritedVolume = 0f, float overridedPitch = 0f)
        {
            SoundPrefab soundPrefab = null;

            var volume = 1f;

            var pitch = overridedPitch;

            //switch (name)
            //{
            //}

            volume = overritedVolume > 0 ? overritedVolume : volume;

            var prefabFx = SoundFXs.FirstOrDefault(x => x.Name.Equals(name.ToString(), StringComparison.InvariantCultureIgnoreCase));

            if (prefabFx != null)
            {
                var config = new SoundPrefabPoolConfig(name.ToString(), prefabFx.SoundEffect, volume, pitch: pitch);

                soundPrefab = Game1.SoundManager.Obtain(config);

                if (soundPrefab != null)
                {
                    soundPrefab.OriginalVolume = volume;
                    soundPrefab.ActualVolume = volume;
                }
            }

            return soundPrefab;
        }

        private SoundPrefab GetMusicPrefab(MusicName name, float overritedVolume = 0f)
        {
            var prefabMusic = Musics.FirstOrDefault(x => x.Name.Contains(name.ToString(), StringComparison.InvariantCultureIgnoreCase));

            return prefabMusic;
        }

        public void PlaySound(SoundFxName name, float volume = 0f, float delay = 0f, float pitch = 0f)
        {
            if (delay > 0)
            {
                Core.Schedule(delay, false, t =>
                {
                    var soundPrefab = GetSoundFxPrefab(name, volume, pitch);

                    if (soundPrefab != null)
                    {

                        PlayFx(soundPrefab);
                    }

                    t.Stop();
                });
            }
            else
            {
                var soundPrefab = GetSoundFxPrefab(name, volume, pitch);

                if (soundPrefab != null)
                {
                    PlayFx(soundPrefab);
                }
            }
        }

        private void PlayFx(SoundPrefab sound)
        {
            if (sound != null)
            {
                var fx = sound.SoundEffectInstance;

                fx.Pitch = sound.Pitch;

                fx.Volume = sound.OriginalVolume * Game1.SaveData.SoundFxActualVolumePercentage;

                fx.Play();

                sound.TimesPlayed++;

                sound.LastTimePlayed = DateTime.Now;
            }
        }

        public void PlaySoundRandomPitch(SoundFxName name, float maxValue, float volume = 0f)
        {
            var soundPrefab = GetSoundFxPrefab(name, volume);

            if (soundPrefab != null)
            {
                var fx = soundPrefab.SoundEffectInstance;

                fx.Pitch = Nez.Random.Range(-maxValue, maxValue);

                PlayFx(soundPrefab);
            }
        }

        private float CalculateVolume(float distance, float maxDistance, float originalVolume)
        {
            float volume = originalVolume * Math.Max(0, 1 - (distance / maxDistance) * (distance / maxDistance));

            return volume;
        }

        public void StopSound(SoundFxName name)
        {
            if (Game1.GameManager == null)
            {
                return;
            }

            Game1.SoundManager.StopSoundFx(name);
        }

        public void StopAllSounds()
        {
            if (Game1.GameManager == null)
            {
                return;
            }

            Game1.SoundManager.StopAllSoundFxs();
        }

        public void PauseAllSounds(List<SoundFxName> ignoredSounds = null)
        {
            if (Game1.GameManager == null)
            {
                return;
            }

            Game1.SoundManager.PauseAllSoundFxs(ignoredSounds);
        }

        public void ResumeAllSounds()
        {
            if (Game1.GameManager == null)
            {
                return;
            }

            Game1.SoundManager.ResumeAllSoundFxs();
        }

        public void FadeOutSound(SoundFxName name, float amount)
        {
            var soundPrefab = GetSoundFxPrefab(name);

            if (soundPrefab != null)
            {
                var fx = soundPrefab.SoundEffectInstance;

                var volume = fx.Volume * Game1.SaveData.SoundFxActualVolumePercentage;

                volume -= amount;

                volume = Mathf.Clamp(volume, 0, 1);

                fx.Volume = volume;
            }
        }
        public void PlayMusic(MusicName name)
        {
#if DEBUG
            return;
#endif
            StopMusic();

            var soundPrefab = GetMusicPrefab(name);

            if (soundPrefab != null)
            {
                var fx = soundPrefab.SoundEffectInstance;

                fx.Volume = soundPrefab.OriginalVolume * Game1.SaveData.MusicActualVolumePercentage;
                fx.Play();
            }
        }


        public void ChangeVolumeOfMusic()
        {
            foreach (var item in Musics)
            {
                if (item.SoundEffectInstance != null)
                {
                    item.SoundEffectInstance.Volume = item.OriginalVolume * Game1.SaveData.MusicActualVolumePercentage;
                }
            }
        }
        public void StopMusic(MusicName name = MusicName.None)
        {
            if (name == MusicName.None)
            {
                var playingMusics = Musics.Where(x => x.SoundEffectInstance != null && x.SoundEffectInstance.State == SoundState.Playing);

                foreach (var item in playingMusics)
                {
                    item.SoundEffectInstance.Stop();
                }
            }
            else
            {
                var music = Musics.FirstOrDefault(x => x.Name.Contains(name.ToString(), StringComparison.InvariantCultureIgnoreCase) &&
                x.SoundEffectInstance != null && x.SoundEffectInstance.State == SoundState.Playing);

                if (music != null)
                {
                    music.SoundEffectInstance.Stop();
                }
            }
        }

        public void PauseMusic(MusicName name = MusicName.None)
        {
            if (name == MusicName.None)
            {
                var playingMusics = Musics.Where(x => x.SoundEffectInstance != null && x.SoundEffectInstance.State == SoundState.Playing);

                foreach (var item in playingMusics)
                {
                    var fx = item.SoundEffectInstance;

                    item.SoundEffectInstance.Pause();
                }
            }
            else
            {
                var music = Musics.FirstOrDefault(x => x.Name.Contains(name.ToString(), StringComparison.InvariantCultureIgnoreCase) &&
                x.SoundEffectInstance != null && x.SoundEffectInstance.State == SoundState.Playing);

                if (music != null)
                {
                    var fx = music.SoundEffectInstance;

                    music.SoundEffectInstance.Pause();
                }
            }
        }

        public void ResumeMusic(MusicName name = MusicName.None)
        {
            if (name == MusicName.None)
            {
                var playingMusics = Musics.Where(x => x.SoundEffectInstance != null && x.SoundEffectInstance.State == SoundState.Paused);

                foreach (var item in playingMusics)
                {
                    var fx = item.SoundEffectInstance;

                    fx.Resume();
                }
            }
            else
            {
                var music = Musics.FirstOrDefault(x => x.Name.Contains(name.ToString(), StringComparison.InvariantCultureIgnoreCase) &&
                x.SoundEffectInstance != null && x.SoundEffectInstance.State == SoundState.Paused);

                if (music != null)
                {
                    var fx = music.SoundEffectInstance;

                    music.SoundEffectInstance.Resume();
                }
            }
        }
        #endregion


        #region texts and ui stuff
        public Texture2D GetUITexture(Enum spriteName)
        {
            var originalTexture = _uiTextures.FirstOrDefault(x => x.Name.ToLower() == spriteName.ToString().ToLower());

            if (originalTexture != null)
            {
                return UtilHelper.CloneTexture(originalTexture.Texture);
            }

            return null;
        }

        public CustomTextureData GetUITextureData(Enum name)
        {
            var data = _uiTextures.FirstOrDefault(x => x.Name.ToLower() == name.ToString().ToLower());

            if (data != null)
            {
                return data;
            }

            return null;
        }

        public Texture2D GetUITextureByName(string spriteName)
        {
            var originalTexture = _uiTextures.FirstOrDefault(x => x.Name.ToLower() == spriteName.ToLower());

            if (originalTexture != null)
            {
                return UtilHelper.CloneTexture(originalTexture.Texture);
            }

            return null;
        }

        public List<GameText> GetUITextsOfScreen(UIGameTextType screenUIName)
        {
            var result = new List<GameText>();

            if (Game1.TextManager != null)
            {
                result.AddRange(Game1.TextManager.GetAllByType(_uiGameTextJsonData, screenUIName.ToString()));
            }

            return result;
        }

        public List<GameText> GetTextsByType(GameTextJsonXnbData<GameTextJsonData> xnbData, Enum type)
        {
            var result = new List<GameText>();

            if (Game1.TextManager != null)
            {
                result.AddRange(Game1.TextManager.GetAllByType(xnbData, type.ToString()));
            }

            return result;
        }

        public List<GameText> GetMultipleTextsByType(GameTextJsonXnbData<GameTextJsonData> xnbData, Enum name, Enum type)
        {
            var result = new List<GameText>();

            if (Game1.TextManager != null)
            {
                result.AddRange(Game1.TextManager.GetListByNameType(xnbData, name.ToString(), type.ToString()));
            }

            return result;
        }

        public List<GameText> GetTextsByName(GameTextJsonXnbData<GameTextJsonData> xnbData, Enum name)
        {
            var result = new List<GameText>();

            if (Game1.TextManager != null)
            {
                result.AddRange(Game1.TextManager.GetAllByName(xnbData, name.ToString()));
            }

            return result;
        }

        public List<GameText> GetTextsByLanguage(GameTextJsonXnbData<GameTextJsonData> xnbData)
        {
            var result = new List<GameText>();

            if (Game1.TextManager != null)
            {
                result.AddRange(Game1.TextManager.GetAll(xnbData));
            }

            return result;
        }
        #endregion
    }
}
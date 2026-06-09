using System;
using System.Collections.Generic;
using System.Linq;
using ComfyJamSummer.Enums;
using ComfyJamSummer.PoolObjects;
using ComfyJamSummer.Prefab;
using Microsoft.Xna.Framework.Audio;
using Nez;

namespace ComfyJamSummer.Components.Extensions
{
    public class SoundManager : SceneComponent
    {
        private const double SoundRecycleDelaySeconds = 0.5;

        private Prefabs _prefabs;

        #region variables and constructors
        private List<SoundPrefab> _soundPrefabs;

        public SoundManager()
        {
            _soundPrefabs = new List<SoundPrefab>();

            Pool<SoundPrefab>.WarmCache(500000);
        }
        #endregion

        #region public methods
        public SoundPrefab Obtain(SoundPrefabPoolConfig config)
        {
            var now = DateTime.Now;

            var stoppedSounds = _soundPrefabs
                .Where(x => x.SoundEffectInstance.State == SoundState.Stopped && (now - x.LastTimePlayed).TotalSeconds > 0.5)
                .ToList();

            for (int i = 0; i < stoppedSounds.Count; i++)
            {
                var soundPrefab = stoppedSounds[i];

                Pool<SoundPrefab>.Free(soundPrefab);

                _soundPrefabs.Remove(soundPrefab);
            }

            var activeCount = GetPlayingSoundsQuantity(config.Name);

            var maxInstancesPerSound = 8;

            if (activeCount >= maxInstancesPerSound)
            {
                return null;
            }

            var reusableSoundPrefab = _soundPrefabs.FirstOrDefault(x => x.Name.Equals(config.Name, StringComparison.InvariantCultureIgnoreCase) &&
            x.TimesPlayed < 2 && x.SoundEffectInstance.State != SoundState.Playing);

            if (reusableSoundPrefab != null)
            {
                return reusableSoundPrefab;
            }

            var prefab = Pool<SoundPrefab>.Obtain();

            prefab.Initialize(config);

            _soundPrefabs.Add(prefab);

            return prefab;
        }

        public int GetPlayingSoundsQuantity(string name)
        {
            return _soundPrefabs.Count(x =>
                    x.Name.Equals(name, StringComparison.InvariantCultureIgnoreCase) &&
                    x.SoundEffectInstance.State == SoundState.Playing);
        }

        public void StopAllMusic()
        {
            StopAll(true);
        }

        public void ResumeMusic(MusicName name)
        {
            Resume(name);
        }

        public void PauseMusic(MusicName name)
        {
            Pause(name);
        }

        public void StopMusic(MusicName name)
        {
            Stop(name);
        }

        public void StopAllSoundFxs()
        {
            StopAll(false);
        }

        public void PauseAllSoundFxs(List<SoundFxName> ignoredSounds = null)
        {
            PauseAll(false, ignoredSounds);
        }

        public void PauseAllMusic()
        {
            PauseAll(true);
        }

        public void ResumeAllSoundFxs()
        {
            ResumeAll(false);
        }

        public void ResumeAllMusic()
        {
            ResumeAll(true);
        }

        public void ResumeSoundFx(SoundFxName name)
        {
            Resume(name);
        }

        public void PauseSoundFx(SoundFxName name)
        {
            Pause(name);
        }

        public void StopSoundFx(SoundFxName name)
        {
            Stop(name);
        }

        public void Free(SoundPrefab prefab)
        {
            Pool<SoundPrefab>.Free(prefab);

            _soundPrefabs.Remove(prefab);
        }

        public void Clear()
        {
            Pool<SoundPrefab>.ClearCache();

            _soundPrefabs.Clear();
        }

        public override void OnRemovedFromScene()
        {
            base.OnRemovedFromScene();

            Clear();
        }
        #endregion

        #region private methods
        private void StopAll(bool isMusic)
        {
            var sounds = _soundPrefabs.Where(x => x.SoundEffectInstance.State == SoundState.Playing && x.IsMusic == isMusic);

            foreach (var prefab in sounds)
            {
                prefab.SoundEffectInstance.Stop();
            }
        }

        private void PauseAll(bool isMusic, List<SoundFxName> ignoredSounds = null)
        {
            var ignored = new List<SoundFxName>();

            if (ignoredSounds != null)
            {
                ignored.AddRange(ignoredSounds);
            }

            var sounds = _soundPrefabs.Where(x => x.SoundEffectInstance.State == SoundState.Playing &&
            x.IsMusic == isMusic &&
            !ignored.Any(i => x.Name.Equals(i.ToString(), StringComparison.InvariantCultureIgnoreCase)));

            foreach (var prefab in sounds)
            {
                prefab.SoundEffectInstance.Pause();
            }
        }

        private void ResumeAll(bool isMusic)
        {
            var sounds = _soundPrefabs.Where(x => x.SoundEffectInstance.State == SoundState.Paused && x.IsMusic == isMusic);

            foreach (var prefab in sounds)
            {
                prefab.SoundEffectInstance.Resume();
            }
        }

        private void Resume(Enum name)
        {
            var sound = _soundPrefabs
                .FirstOrDefault(x => x.Name.Equals(name.ToString(), StringComparison.InvariantCultureIgnoreCase) &&
                x.SoundEffectInstance.State == SoundState.Paused);

            if (sound != null)
            {
                sound.SoundEffectInstance.Resume();
            }
        }

        private void Pause(Enum name)
        {
            var sound = _soundPrefabs
                .FirstOrDefault(x => x.Name.Equals(name.ToString(), StringComparison.InvariantCultureIgnoreCase) &&
                x.SoundEffectInstance.State == SoundState.Playing);

            if (sound != null)
            {
                sound.SoundEffectInstance.Pause();
            }
        }

        private void Stop(Enum name)
        {
            var sound = _soundPrefabs
                .FirstOrDefault(x => x.Name.Equals(name.ToString(), StringComparison.InvariantCultureIgnoreCase) &&
                x.SoundEffectInstance.State == SoundState.Playing);

            if (sound != null)
            {
                sound.SoundEffectInstance.Stop();
            }
        }
        #endregion
    }
}
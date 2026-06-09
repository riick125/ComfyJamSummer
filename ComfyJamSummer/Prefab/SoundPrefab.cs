using System;
using ComfyJamSummer.PoolObjects;
using Microsoft.Xna.Framework.Audio;

namespace ComfyJamSummer.Prefab
{
    public class SoundPrefab
    {
        public string Name { get; set; }

        public SoundEffect SoundEffect { get; set; }

        public SoundEffectInstance SoundEffectInstance { get; set; }

        public float ActualVolume { get; set; }

        public float OriginalVolume { get; set; }

        public bool IsMusic { get; set; }

        public int TimesPlayed { get; set; }

        public DateTime LastTimePlayed { get; set; }

        public float Pitch { get; set; }

        public void Initialize(SoundPrefabPoolConfig config)
        {
            SetName(config.Name);

            SoundEffect = config.SoundEffect;

            SetSoundEffectInstance(config.SoundEffect.CreateInstance());

            SetOriginalVolume(config.OriginalVolume);

            Pitch = config.Pitch;
        }

        private void SetName(string name)
        {
            Name = name;
        }

        private void SetSoundEffectInstance(SoundEffectInstance soundEffectInstance)
        {
            SoundEffectInstance = soundEffectInstance;
        }

        private void SetOriginalVolume(float originalVolume)
        {
            OriginalVolume = originalVolume;
            ActualVolume = originalVolume;
            SoundEffectInstance.Volume = originalVolume;
        }
    }
}
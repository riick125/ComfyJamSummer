using Microsoft.Xna.Framework.Audio;

namespace ComfyJamSummer.PoolObjects
{
    public class SoundPrefabPoolConfig
    {
        public string Name { get; set; }
        public SoundEffect SoundEffect { get; set; }

        public SoundEffectInstance SoundEffectInstance { get; set; }

        public float OriginalVolume { get; set; }

        public bool IsMusic { get; set; }

        public float Pitch { get; set; }

        public SoundPrefabPoolConfig(string name, SoundEffect soundEffect, float originalVolume, bool isMusic = false, float pitch = 0f)
        {
            this.Name = name;
            this.SoundEffect = soundEffect;
            this.OriginalVolume = originalVolume;
            this.IsMusic = isMusic;
            this.Pitch = pitch;

            if (soundEffect != null)
            {
                this.SoundEffectInstance = soundEffect.CreateInstance();
            }
        }
    }
}
using System;

namespace ComfyJamSummer.Entities.Configs
{
    public class WaveConfig : CreatureConfig
    {
        public Island Island { get; set; }

        public int Index { get; set; }

        public int AliveEnemyQtyLimit { get; set; }

        public float StartCooldown { get; set; }

        public float Duration { get; set; }

        public WaveConfig CloneWave(Island island, int index, int aliveEnemyQtyLimit, float startCooldown, float duration)
        {
            var clone = Activator.CreateInstance(GetType()) as WaveConfig;

            clone.Index = index;
            clone.Island = island;
            clone.AliveEnemyQtyLimit = aliveEnemyQtyLimit;
            clone.StartCooldown = startCooldown;
            clone.Duration = duration;

            return clone;
        }
    }
}
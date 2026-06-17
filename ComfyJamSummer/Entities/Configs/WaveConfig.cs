using System;

namespace ComfyJamSummer.Entities.Configs
{
    public class WaveConfig : CreatureConfig
    {
        public Island Island { get; set; }

        public int Index { get; set; }

        public int AliveEnemyQtyLimit { get; set; }

        public int EnemiesSpawnQty { get; set; }

        public float StartCooldown { get; set; }

        public WaveConfig CloneWave(Island island, int index, int aliveEnemyQtyLimit, int enemiesSpawnQty, float startCooldown)
        {
            var clone = Activator.CreateInstance(GetType()) as WaveConfig;

            clone.Index = index;
            clone.Island = island;
            clone.AliveEnemyQtyLimit = aliveEnemyQtyLimit;
            clone.EnemiesSpawnQty = enemiesSpawnQty;
            clone.StartCooldown = startCooldown;

            return clone;
        }
    }
}
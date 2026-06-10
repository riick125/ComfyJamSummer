using ComfyJamSummer.Entities.Configs;
using ComfyJamSummer.Entities.Creatures;
using ComfyJamSummer.Enums;

namespace ComfyJamSummer.Entities
{
    public class Enemy : Creature
    {
        public EnemyType Type { get; set; }

        public Enemy CloneEnemy(CreatureConfig config)
        {
            var clone = base.CloneCreature(config) as Enemy;

            return clone;
        }
    }
}
using ComfyJamSummer.Enums;
using Microsoft.Xna.Framework;

namespace ComfyJamSummer.Entities.Configs
{
    public class EnemyConfig : CreatureConfig
    {
        public EnemyType Type { get; set; }

        public EnemyConfig CloneEnemy(EnemyType type, float hp, float dmg, float speed, float atkSpeed, Vector2 pos, ColliderType colliderType)
        {
            var clone = base.Clone(hp, dmg, speed, atkSpeed, pos, colliderType) as EnemyConfig;

            clone.Type = type;

            return clone;
        }
    }
}
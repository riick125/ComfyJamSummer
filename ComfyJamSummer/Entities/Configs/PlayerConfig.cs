using ComfyJamSummer.Enums;
using Microsoft.Xna.Framework;

namespace ComfyJamSummer.Entities.Configs
{
    public class PlayerConfig : Config
    {
        public PlayerConfig ClonePlayer(float hp, float dmg, float speed, float atkSpeed, Vector2 pos, ColliderType colliderType)
        {
            var clone = base.Clone(hp, dmg, speed, atkSpeed, pos, colliderType) as PlayerConfig;

            return clone;
        }
    }
}
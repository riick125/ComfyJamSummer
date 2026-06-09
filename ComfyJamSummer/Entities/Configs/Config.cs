using System;
using ComfyJamSummer.Enums;
using Microsoft.Xna.Framework;

namespace ComfyJamSummer.Entities.Configs
{
    public class Config
    {
        public ColliderType ColliderType { get; set; }

        public Vector2 Position { get; set; }

        public float HP { get; set; }

        public float Damage { get; set; }

        public float AtkSpeed { get; set; }

        public float Speed { get; set; }

        public Config Clone(float hp, float dmg, float speed, float atkSpeed, Vector2 pos, ColliderType colliderType)
        {
            var clone = Activator.CreateInstance(GetType()) as Config;

            clone.ColliderType = colliderType;
            clone.HP = hp;
            clone.Damage = dmg;
            clone.Speed = speed;
            clone.AtkSpeed = atkSpeed;
            clone.Position = pos;

            return clone;
        }
    }
}
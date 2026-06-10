using System;
using ComfyJamSummer.Enums;
using Microsoft.Xna.Framework;

namespace ComfyJamSummer.Entities.Configs
{
    public class CreatureConfig
    {
        public ColliderType ColliderType { get; set; }

        public Vector2 Position { get; set; }

        public float HP { get; set; }

        public float Damage { get; set; }

        public float AtkSpeed { get; set; }

        public float Speed { get; set; }

        public CreatureConfig Clone(Vector2 pos)
        {
            var clone = Activator.CreateInstance(GetType()) as CreatureConfig;
            clone.HP = HP;
            clone.Damage = Damage;
            clone.AtkSpeed = AtkSpeed;
            clone.Speed = Speed;
            clone.ColliderType = ColliderType;
            clone.Position = pos;

            return clone;
        }
    }
}
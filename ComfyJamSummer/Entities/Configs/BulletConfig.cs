using ComfyJamSummer.Enums;
using Microsoft.Xna.Framework;
using System;

namespace ComfyJamSummer.Entities.Configs
{
    public class BulletConfig
    {
        public OffensiveEffectCollisionEnum Target { get; set; }

        public Vector2 Position { get; set; }

        public float Damage { get; set; }

        public float Speed { get; set; }

        public float MaxAngleSpread { get; set; }

        public float Rotation { get; set; }

        public bool Collided { get; set; }

        public float LifeTime { get; set; }

        public Vector2 Direction { get; set; }

        public bool CreateImpactEffect { get; set; }

        public BulletConfig ClonePlayer(Gun gun, OffensiveEffectCollisionEnum target, Vector2 direction, float rotation, bool createImpactEffect = true)
        {
            return HydrateValues(gun.BulletSpeed, gun.Damage, gun.MaxAngleSpread, gun.MuzzlePosition, target, direction, rotation, createImpactEffect);
        }

        public BulletConfig CloneEnemy(Enemy enemy, OffensiveEffectCollisionEnum target, Vector2 direction, float rotation = 0, bool createImpactEffect = true)
        {
            return HydrateValues(enemy.BulletSpeed, enemy.Damage, enemy.MaxAngleSpread, enemy.Position, target, direction, rotation, createImpactEffect);
        }

        BulletConfig HydrateValues(float bulletSpeed, float dmg, float maxAngleSpread, Vector2 pos, OffensiveEffectCollisionEnum target, Vector2 direction, float rotation, bool createImpactEffect)
        {
            var clone = Activator.CreateInstance(GetType()) as BulletConfig;

            clone.Position = pos;
            clone.Damage = dmg;
            clone.Speed = bulletSpeed;
            clone.MaxAngleSpread = maxAngleSpread;
            clone.Target = target;
            clone.Direction = direction;
            clone.Rotation = rotation;
            clone.CreateImpactEffect = createImpactEffect;
            clone.LifeTime = 5f;

            return clone;
        }
    }
}
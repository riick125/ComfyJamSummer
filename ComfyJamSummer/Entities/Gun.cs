using ComfyJamSummer.Components.Visuals;
using ComfyJamSummer.Entities.Base;
using ComfyJamSummer.Entities.Creatures;
using Microsoft.Xna.Framework;

namespace ComfyJamSummer.Entities
{
    public class Gun : Animated
    {
        public Creature Creature { get; set; }

        public bool IsReloading { get; set; }

        public float ReloadTime { get; set; }

        public float TimeLeftToEndReload { get; set; }

        public float Damage { get; set; }

        public Vector2 Offset { get; set; }

        public float TimeLeftToNextShot { get; set; }

        public float FireRate { get; set; }

        public int ActualAmmo { get; set; }

        public int MagSize { get; set; }

        public float BulletSpeed { get; set; }

        public Vector2 MuzzlePosition { get; set; }

        public float MinAngleSpread { get; set; }

        public float ActualAngleSpread { get; set; }

        public float MaxAngleSpread { get; set; }

        public float TimeLeftToEndShootAnimation { get; set; }

        public float ShootAnimationDuration { get; set; }

        public Gun CloneGun(Creature creature, Vector2 pos = default)
        {
            var clone = base.CloneAnimated(pos) as Gun;
            clone.Creature = creature;
            clone.Offset = Offset;
            clone.ActualAmmo = ActualAmmo;
            clone.ReloadTime = ReloadTime;

            clone.Damage = Damage;

            clone.FireRate = FireRate;
            clone.ShootAnimationDuration = clone.FireRate;

            clone.MagSize = MagSize;

            clone.ReloadTime = ReloadTime;

            clone.BulletSpeed = BulletSpeed;

            clone.Animator.SetLocalOffset(new Vector2(0, SpriteHeight / 8));

            clone.AddComponent(new LittleShake(clone.FireRate));

            return clone;
        }
    }
}
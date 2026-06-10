using ComfyJamSummer.Components.Gameplay;
using ComfyJamSummer.Components.Visuals;
using ComfyJamSummer.Entities.Base;
using ComfyJamSummer.Entities.Creatures;
using ComfyJamSummer.Helpers;
using Microsoft.Xna.Framework;

namespace ComfyJamSummer.Entities
{
    public class Gun : Animated
    {
        public Player Player { get; set; }

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

        public float PointingAngle { get; set; }

        public float MinAngleSpread { get; set; }

        public float ActualAngleSpread { get; set; }

        public float MaxAngleSpread { get; set; }

        public float TimeLeftToEndShootAnimation { get; set; }

        public float ShootAnimationDuration { get; set; }

        public Gun CloneGun(Player player, Vector2 pos = default)
        {
            var clone = base.CloneAnimated(pos) as Gun;
            clone.Player = player;
            clone.Offset = player != null ? new Vector2(player.SpriteWidth / 4, 0) : Vector2.Zero;
            clone.ActualAmmo = ActualAmmo;
            clone.ReloadTime = ReloadTime;
            clone.ActualAngleSpread = ActualAngleSpread;
            clone.MaxAngleSpread = MaxAngleSpread;

            clone.Damage = Damage;

            clone.FireRate = FireRate;
            clone.ShootAnimationDuration = clone.FireRate;

            clone.MagSize = MagSize;

            clone.ReloadTime = ReloadTime;

            clone.BulletSpeed = BulletSpeed;

            clone.AddComponent(new GunController(UtilHelper.GameManager(), UtilHelper.Prefabs()));

            if (clone.Animator != null)
            {
                clone.Animator.SetLocalOffset(new Vector2(0, SpriteHeight / 8));
            }
            else
            {
                clone.Renderer.SetLocalOffset(new Vector2(0, SpriteHeight / 8));
            }

            clone.AddComponent(new LittleShake(clone.FireRate));

            return clone;
        }
    }
}
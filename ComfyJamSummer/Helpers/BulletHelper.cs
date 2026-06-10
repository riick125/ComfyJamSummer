using ComfyJamSummer.Entities;
using ComfyJamSummer.Entities.Creatures;
using ComfyJamSummer.Enums;
using Microsoft.Xna.Framework;
using Nez;

namespace ComfyJamSummer.Helpers
{
    public static class BulletHelper
    {
        public static void Create(Creature shooter, Creature target)
        {
            if (shooter == null || target == null) return;

            var direction = CreateDirection(shooter, target);

            if (direction == default)
            {
                return;
            }

            Process(shooter, direction);
        }

        public static void Create(Creature shooter, Vector2 direction)
        {
            if (shooter == null) return;

            Process(shooter, direction);
        }

        static void Process(Creature shooter, Vector2 direction)
        {
            var prefabs = shooter.Prefabs;

            Bullet bullet = null;

            switch (shooter)
            {
                case Player player:
                    var bulletConfig = prefabs?.BulletConfig?.ClonePlayer(player.Gun, OffensiveEffectCollisionEnum.Enemy, direction, player.Gun.PointingAngle);

                    bullet = prefabs?.Bullet?.CloneBullet(bulletConfig);
                    break;

                case Enemy enemy:
                    var bulletConfigEnemy = prefabs?.BulletConfig?.CloneEnemy(enemy, OffensiveEffectCollisionEnum.Player, direction);

                    bullet = prefabs?.BulletEnemy?.CloneBulletEnemy(bulletConfigEnemy);
                    break;
            }

            if (bullet != null)
            {
                Core.Scene?.AddEntity(bullet);
            }
        }

        static Vector2 CreateDirection(Creature shooter, Creature target)
        {
            var prefabs = shooter.Prefabs;

            if (prefabs == null)
            {
                return default;
            }

            var direction = target.Position - shooter.Position;
            direction.Normalize();

            if (!DirectionHelper.Validate(direction))
            {
                return default;
            }

            return direction;
        }
    }
}
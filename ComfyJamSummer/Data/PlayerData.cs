using AutoMapper;
using ComfyJamSummer.Entities;
using ComfyJamSummer.Entities.Configs;
using ComfyJamSummer.Enums;
using ComfyJamSummer.Prefab;
using Microsoft.Xna.Framework;
using Nez;

namespace ComfyJamSummer.Data
{
    public class PlayerData : BaseData
    {
        public PlayerData(Prefabs prefabs, IMapper mapper) : base(Constants.PLAYER_DATA_PATH, prefabs, mapper)
        {
        }

        public Player Create()
        {
            var player = CreateDummyRenderer<Player>(width: 16, height: 16, Color.CornflowerBlue, Constants.CREATURE_RENDER_LAYER);

            var circleCollider = new CircleCollider(10)
            {
                CollidesWithLayers = (int)CollisionLayer.Map | (int)CollisionLayer.Enemy,
                PhysicsLayer = (int)CollisionLayer.Player,
                Tag = CreatureCollider.Body.ToString()
            };

            player.AddComponent(circleCollider);

            player.AddMover();

            return player;
        }

        public Gun CreateGun()
        {
            var gun = CreateDummyRenderer<Gun>(width: 7, height: 7, Color.Gray, Constants.CREATURE_RENDER_LAYER - 1);

            gun.Damage = 33.3f;
            gun.ReloadTime = 1.05f;
            gun.BulletSpeed = 400;

            gun.FireRate = 0.12f;
            gun.ShootAnimationDuration = gun.FireRate;

            gun.MagSize = 32;
            gun.ActualAmmo = gun.MagSize;
            gun.ActualAngleSpread = 3.2f;
            gun.MaxAngleSpread = 5.5f;

            return gun;
        }

        public Bullet CreateBullet()
        {
            var bullet = CreateDummyRenderer<Bullet>(width: 4, height: 4, Color.Yellow, Constants.CREATURE_RENDER_LAYER - 1);

            bullet.LifeTime = 7f;

            return bullet;
        }

        public PlayerConfig CreateBaseConfig()
        {
            return new PlayerConfig()
            {
                HP = PlayerValues.HP,
                Speed = PlayerValues.SPEED,
                ColliderType = ColliderType.Player
            };
        }

        public PlayerConfig InitializePlayer(Vector2 pos)
        {
            return _prefabs?.PlayerConfig?.ClonePlayer(pos);
        }
    }
}
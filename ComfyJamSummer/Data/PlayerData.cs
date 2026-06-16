using AutoMapper;
using ComfyJamSummer.Entities;
using ComfyJamSummer.Entities.Configs;
using ComfyJamSummer.Enums;
using ComfyJamSummer.Extensions;
using ComfyJamSummer.Prefab;
using Microsoft.Xna.Framework;
using Nez;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ComfyJamSummer.Data
{
    public class PlayerData : BaseData
    {
        string _gunDir = "sprites/gameplay/guns/";
        string _bulletDir = "sprites/gameplay/bullets/";

        public PlayerData(Prefabs prefabs, IMapper mapper) : base(Constants.PLAYER_DATA_PATH, prefabs, mapper)
        {
        }

        public Player Create()
        {
            var player = new Player() { SpriteWidth = 18, SpriteHeight = 20 };

            var anim = new List<CreatureAnim>() { CreatureAnim.Idle, CreatureAnim.Walk };

            var cast = anim.Cast<Enum>().ToList();

            var dir = _rootDir + "player_";

            CreateAnimatorWithEnum(player, cast, dir, Constants.CREATURE_RENDER_LAYER);

            player.CreateCircleCollider(physicsLayer: CollisionLayer.Player, radius: 10, tag: CreatureCollider.Body.ToString(), offset: new Vector2(0, player.SpriteHeight / 8));

            player.AddMover();

            return player;
        }

        public Gun CreateGun()
        {
            var gun = new Gun() { SpriteWidth = 19, SpriteHeight = 15 };

            var anim = GetValues<SmgAnim>();

            var dir = _gunDir + "smg_";

            CreateAnimatorWithEnum(gun, anim, dir, Constants.CREATURE_RENDER_LAYER - 1);

            gun.Damage = 33.3f;
            gun.ReloadTime = 1.05f;
            gun.BulletSpeed = 400;

            gun.FireRate = 0.12f;
            gun.ShootAnimationDuration = gun.FireRate;

            gun.MagSize = 32;
            gun.ActualAmmo = gun.MagSize;
            gun.ActualAngleSpread = 2.2f;
            gun.MaxAngleSpread = 4f;

            return gun;
        }

        public Bullet CreateBullet()
        {
            var bullet = new Bullet() { SpriteWidth = 20, SpriteHeight = 12 };

            var anim = GetValues<BulletAnim>();

            var dir = _bulletDir + "friendly_bullet_";

            CreateAnimatorWithEnum(bullet, anim, dir, Constants.CREATURE_RENDER_LAYER - 1);

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

        public PlayerConfig InitializePlayer(uint islandId, Vector2 pos)
        {
            return _prefabs?.PlayerConfig?.ClonePlayer(islandId, pos);
        }
    }
}
using AutoMapper;
using ComfyJamSummer.Components.Gameplay;
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
            var width = 16;
            var height = 16;

            var player = new Player()
            {
                SpriteWidth = width,
                SpriteHeight = height
            };

            var circleCollider = new CircleCollider(16)
            {
                CollidesWithLayers = (int)CollisionLayer.Map | (int)CollisionLayer.Enemy,
                PhysicsLayer = (int)CollisionLayer.Player
            };

            player.AddComponent(circleCollider);

            player.AddMover();

            CreateSingleColorSpriteRenderer(player, Color.CornflowerBlue, Constants.CREATURE_RENDER_LAYER);

            return player;
        }

        public PlayerConfig InitializePlayer(Vector2 pos)
        {
            var maxHp = PlayerValues.HP;
            var speed = PlayerValues.SPEED;

            return _prefabs.PlayerConfig.ClonePlayer(maxHp, 0, speed, 0, pos, ColliderType.Player);
        }
    }
}
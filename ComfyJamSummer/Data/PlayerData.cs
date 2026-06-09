using AutoMapper;
using ComfyJamSummer.Entities;
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
            var maxHp = PlayerValues.HP;
            var speed = PlayerValues.SPEED;

            var player = new Player()
            {
                SpriteWidth = width,
                SpriteHeight = height,
                ActualHP = maxHp,
                MaxHP = maxHp,
                Speed = speed
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
    }
}
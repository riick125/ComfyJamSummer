using AutoMapper;
using ComfyJamSummer.Entities;
using ComfyJamSummer.Entities.Configs;
using ComfyJamSummer.Entities.Objects;
using ComfyJamSummer.Enums;
using ComfyJamSummer.Prefab;
using Microsoft.Xna.Framework;
using Nez;
using System;
using static Assimp.Metadata;

namespace ComfyJamSummer.Data
{
    public class NpcObjectData : BaseData
    {
        public NpcObjectData(Prefabs prefabs, IMapper mapper) : base(Constants.NPC_DATA_PATH, prefabs, mapper)
        {
        }

        public Crab CreateCrab()
        {
            var anims = GetValues<CrabAnim>();

            var dir = _rootDir + "crab/crab_";

            var crab = CreateAnimatorWithEnum<Crab>(32, 28, anims, dir, Constants.CREATURE_RENDER_LAYER);

            return crab;
        }

        public Rocket CreateRocket()
        {
            var anims = GetValues<RocketAnim>();

            var dir = Constants.MAP_DATA_PATH + "rocket/rocket_";

            var rocket = CreateAnimatorWithEnum<Rocket>(24, 41, anims, dir, Constants.CREATURE_RENDER_LAYER);

            return rocket;
        }

        public BreadBag CreateBreadBag()
        {
            var dir = Constants.MAP_DATA_PATH + "bread_bag";

            var bag = new BreadBag();

            var texture = Core.Content.LoadTexture(dir);

            var renderer = CreateSpriteRenderer(bag, texture, Constants.CREATURE_RENDER_LAYER);

            bag.SpriteWidth = (int)renderer.Width;
            bag.SpriteHeight = (int)renderer.Height;

            return bag;
        }

        public StarFish CreateStarFish()
        {
            var starFish = CreateDummyRenderer<StarFish>(12, 12, Color.Orange, Constants.CREATURE_RENDER_LAYER);

            return starFish;
        }

        public CreatureConfig InitializeCrab(uint islandId, Vector2 pos)
        {
            return _prefabs.CrabConfig.Clone(islandId, pos);
        }

        public CreatureConfig CreateBaseConfig()
        {
            return new CreatureConfig()
            {
                Damage = CrabValues.DMG,
                Speed = CrabValues.SPEED,
                AtkSpeed = CrabValues.ATK_SPEED
            };
        }
    }
}
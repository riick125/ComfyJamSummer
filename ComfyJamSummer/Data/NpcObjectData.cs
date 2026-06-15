using AutoMapper;
using ComfyJamSummer.Entities;
using ComfyJamSummer.Entities.Configs;
using ComfyJamSummer.Enums;
using ComfyJamSummer.Prefab;
using Microsoft.Xna.Framework;
using System;

namespace ComfyJamSummer.Data
{
    public class NpcObjectData : BaseData
    {
        public NpcObjectData(Prefabs prefabs, IMapper mapper) : base(Constants.NPC_DATA_PATH, prefabs, mapper)
        {
        }

        public Crab CreateCrab()
        {
            var crab = CreateDummyRenderer<Crab>(12, 12, Color.MonoGameOrange, Constants.CREATURE_RENDER_LAYER);

            return crab;
        }

        public Rocket CreateRocket()
        {
            var rocket = CreateDummyRenderer<Rocket>(32, 64, Color.Gray, Constants.CREATURE_RENDER_LAYER);

            return rocket;
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
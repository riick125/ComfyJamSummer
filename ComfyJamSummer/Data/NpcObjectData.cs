using AutoMapper;
using ComfyJamSummer.Entities;
using ComfyJamSummer.Entities.Configs;
using ComfyJamSummer.Enums;
using ComfyJamSummer.Prefab;
using Microsoft.Xna.Framework;

namespace ComfyJamSummer.Data
{
    public class NpcObjectData : BaseData
    {
        public NpcObjectData(Prefabs prefabs, IMapper mapper) : base(Constants.NPC_DATA_PATH, prefabs, mapper)
        {
        }

        public Crab CreateCrab()
        {
            var crab = CreateDummyRenderer<Crab>(12, 12, Color.Orange, Constants.CREATURE_RENDER_LAYER);

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

        //public CreatureConfig InitializeCrab(Vector2 pos)
        //{
        //    var maxHp = PlayerValues.HP;
        //    var speed = PlayerValues.SPEED;

        //    return _prefabs.CreatureConfig.Clone(maxHp, 0, speed, 0, pos, ColliderType.Player);
        //}
    }
}
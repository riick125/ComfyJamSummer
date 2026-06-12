using AutoMapper;
using ComfyJamSummer.Entities.Base;
using ComfyJamSummer.Enums;
using ComfyJamSummer.Prefab;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;

namespace ComfyJamSummer.Data
{
    public class CollectibleData : BaseData
    {
        public CollectibleData(Prefabs prefabs, IMapper mapper) : base(Constants.COLLECTIBLE_DATA_PATH, prefabs, mapper)
        {
        }

        public List<Collectible> CreateAll()
        {
            var result = new List<Collectible>();

            var all = Enum.GetValues<CollectibleType>();

            foreach (var item in all)
            {
                var collectible = CreateDummyRenderer<Collectible>(8, 7, Color.Green, Constants.CREATURE_RENDER_LAYER);

                collectible.Type = item;

                result.Add(collectible);
            }

            return result;
        }
    }
}
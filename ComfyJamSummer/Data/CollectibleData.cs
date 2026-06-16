using AutoMapper;
using ComfyJamSummer.Entities.Base;
using ComfyJamSummer.Enums;
using ComfyJamSummer.JsonsData.AsepriteData;
using ComfyJamSummer.Prefab;
using Microsoft.Xna.Framework;
using Newtonsoft.Json;
using Nez;
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
                var name = item.ToString().ToLower();

                var jsonDir = $"Content/jsons/aseprite/collectibles/{name}.json";

                var spriteData = LoadAsepriteJson(jsonDir);

                if (spriteData == null)
                {
                    continue;
                }

                var dir = $"{_rootDir}{name}";

                var texture = Core.Content.LoadTexture(dir);

                var collectible = new Collectible()
                {
                    SpriteWidth = spriteData.Width,
                    SpriteHeight = spriteData.Height,
                    Type = item
                };

                CreateAnimatorOneAnimation(collectible, dir, "Idle", Constants.CREATURE_RENDER_LAYER);

                result.Add(collectible);
            }

            return result;
        }
    }
}
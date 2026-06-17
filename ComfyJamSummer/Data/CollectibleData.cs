using AutoMapper;
using ComfyJamSummer.Entities.Base;
using ComfyJamSummer.Entities.Collectibles;
using ComfyJamSummer.Enums;
using ComfyJamSummer.Prefab;
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

                Collectible collectible;

                switch (item)
                {
                    case CollectibleType.Fried_Chicken:
                        collectible = CreateSpecial<FriedChicken>(spriteData.Width, spriteData.Height, item);
                        break;

                    case CollectibleType.Sliced_Bread:
                        collectible = CreateSpecial<SlicedBread>(spriteData.Width, spriteData.Height, item);
                        break;

                    case CollectibleType.Sandwich:
                        collectible = CreateSpecial<Sandwich>(spriteData.Width, spriteData.Height, item);
                        break;

                    default:
                        collectible = new Collectible()
                        {
                            SpriteWidth = spriteData.Width,
                            SpriteHeight = spriteData.Height,
                            Type = item
                        };
                        break;
                }

                CreateAnimatorOneAnimation(collectible, dir, "Idle", Constants.CREATURE_RENDER_LAYER);

                result.Add(collectible);
            }

            return result;
        }

        T CreateSpecial<T>(int width, int height, CollectibleType type) where T : Collectible, new()
        {
            return new T()
            {
                SpriteWidth = width,
                SpriteHeight = height,
                Type = type
            };
        }
    }
}
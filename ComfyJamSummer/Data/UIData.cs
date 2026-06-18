using AutoMapper;
using ComfyJamSummer.Entities.TextureData;
using ComfyJamSummer.Enums;
using ComfyJamSummer.JsonsData;
using ComfyJamSummer.Prefab;
using Nez;
using System;
using System.Collections.Generic;

namespace ComfyJamSummer.Data
{
    public class UIData : BaseData
    {
        public UIData(Prefabs prefabs, IMapper mapper) : base(Constants.UI_DATA_PATH, prefabs, mapper)
        {
            Pool<CustomTextureData>.WarmCache(100);
        }

        public List<CustomTextureData> CreateAll()
        {
            var result = new List<CustomTextureData>();

            var all = Enum.GetValues<UISprite>();

            foreach (var item in all)
            {
                var name = item.ToString().ToLower();

                var dir = $"{_rootDir}{name}";

                try
                {
                    var texture = Core.Content.LoadTexture(dir);

                    var data = Pool<CustomTextureData>.Obtain();

                    var config = new SpriteJsonData()
                    {
                        SpriteWidth = texture.Width,
                        SpriteHeight = texture.Height,
                        Name = name,
                    };

                    data.Initialize(config, texture);

                    result.Add(data);
                }
                catch (Exception)
                {
                }
            }

            return result;
        }
    }
}
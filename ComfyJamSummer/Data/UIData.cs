using System.Collections.Generic;
using AutoMapper;
using ComfyJamSummer.Entities.TextureData;
using ComfyJamSummer.JsonsData;
using ComfyJamSummer.Prefab;
using ComfyJamSummer.Results;

namespace ComfyJamSummer.Data
{
    public class UIData : BaseData
    {
        JsonDataResult<SpriteJsonData> _jsonData;

        public UIData(Prefabs prefabs, IMapper mapper) : base(Constants.UI_DATA_PATH, prefabs, mapper)
        {
        }

        public List<CustomTextureData> CreateAll()
        {
            JsonDataResult<SpriteJsonData> jsonData = new JsonDataResult<SpriteJsonData>("");

            return base.CreateAllJsonData(jsonData);
        }
    }
}
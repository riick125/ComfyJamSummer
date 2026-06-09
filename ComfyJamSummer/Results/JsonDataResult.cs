using System.Collections.Generic;
using System.IO;
using System.Linq;
using ComfyJamSummer.JsonsData;
using ComfyJamSummer.JsonsData.Xnb;
using Nez;

namespace ComfyJamSummer.Results
{
    public class JsonDataResult<T> where T : SpriteJsonData, new()
    {
        public BaseSpriteJsonXnbData<T> Data { get; set; }

        public JsonDataResult(string fileName)
        {
            var realPath = Path.Combine(Core.Content.RootDirectory, $"{Constants.JSON_DATA_REAL_PATH}{fileName}.xnb");

            if (!string.IsNullOrEmpty(fileName) && File.Exists(realPath))
            {
                Data = Core.Content?.Load<BaseSpriteJsonXnbData<T>>($"{Constants.JSON_DATA_PATH}{fileName.ToLower()}");
            }
        }

        public bool Validate()
        {
            return Data != null && Data.Values != null && Data.Values.Any();
        }

        public List<T> GetAllData()
        {
            if (!Validate())
            {
                return new List<T>();
            }

            return Data.Values;
        }

        public T GetData()
        {
            if (!Validate())
            {
                return new T();
            }

            return Data.Values.FirstOrDefault();
        }
    }
}
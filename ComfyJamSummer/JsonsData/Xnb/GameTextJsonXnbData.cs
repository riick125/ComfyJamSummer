using System.Collections.Generic;
using ComfyJamSummer.Data.Jsons;
using ComfyJamSummer.Enums;
using Newtonsoft.Json;

namespace ComfyJamSummer.JsonsData.Xnb
{
    public class GameTextJsonXnbData<T> : BaseJsonXnbData<GameTextJsonData>
    {
        public GameTextArchiveName ArchiveName { get; set; }
    }

    #region custom classes
    public class UiJsonXnbData : BaseSpriteJsonXnbData<SpriteJsonData>
    {

    }
    #endregion

    #region base classes
    public class BaseJsonXnbData<T>
    {
        public List<T> Values { get; set; }

        public static T FromJson(string jsonString)
        {
            return JsonConvert.DeserializeObject<T>(jsonString);
        }
    }

    public class BaseSpriteJsonXnbData<G> where G : SpriteJsonData
    {
        public List<G> Values { get; set; }

        public static T FromJson<T>(string jsonString) where T : BaseSpriteJsonXnbData<G>
        {
            return JsonConvert.DeserializeObject<T>(jsonString);
        }
    }
    #endregion
}
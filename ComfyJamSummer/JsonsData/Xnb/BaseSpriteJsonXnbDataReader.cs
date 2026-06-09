using Microsoft.Xna.Framework.Content;

namespace ComfyJamSummer.JsonsData.Xnb
{
    #region base class
    public class BaseSpriteJsonXnbDataReader<T, G> : ContentTypeReader<T>
where T : BaseSpriteJsonXnbData<G>
where G : SpriteJsonData
    {
        protected override T Read(ContentReader input, T existingInstance)
        {
            var jsonString = input.ReadString();
            return BaseSpriteJsonXnbData<G>.FromJson<T>(jsonString);
        }
    }
    #endregion

    #region custom classes
    public class UiJsonXnbDataReader : BaseSpriteJsonXnbDataReader<UiJsonXnbData, SpriteJsonData> { }
    #endregion
}
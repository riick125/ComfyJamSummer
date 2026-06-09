using ComfyJamSummer.JsonsData;
using Microsoft.Xna.Framework.Graphics;
using Nez;

namespace ComfyJamSummer.Entities.TextureData
{
    public class CustomTextureData : IPoolable
    {
        string _name;

        public string Name { get { return !string.IsNullOrEmpty(_name) ? _name.ToLower() : string.Empty; } }

        public string ScreenName { get; set; }

        public Texture2D Texture { get; set; }

        public int SpriteWidth { get; set; }

        public int SpriteHeight { get; set; }

        public void Initialize(SpriteJsonData config, Texture2D texture)
        {
            _name = config.Name.ToLower();
            ScreenName = config.ScreenName;
            Texture = texture;
            SpriteWidth = config.SpriteWidth;
            SpriteHeight = config.SpriteHeight;
        }

        public void Reset()
        {
            _name = null;
            Texture = null;
            SpriteWidth = 0;
            SpriteHeight = 0;
        }
    }
}
using Nez;
using Nez.Textures;

namespace ComfyJamSummer.Helpers
{
    public static class SpriteHelper
    {
        public static Sprite[] LoadSpritesFromAtlas(string directory, string animationName, int width, int height)
        {
            var texture = Core.Content.LoadTexture($"{directory.ToLower()}{animationName.ToLower()}");
            return Sprite.SpritesFromAtlas(texture, width, height).ToArray();
        }

        public static Sprite[] LoadSpritesFromAtlas(string fileDirectory, int width, int height)
        {
            var texture = Core.Content.LoadTexture($"{fileDirectory.ToLower()}");
            return Sprite.SpritesFromAtlas(texture, width, height).ToArray();
        }

        public static Sprite[] LoadSpritesFromSheet(string path, int width, int height)
        {
            var texture = Core.Content.LoadTexture(path.ToLower());
            return Sprite.SpritesFromAtlas(texture, width, height).ToArray();
        }
    }
}
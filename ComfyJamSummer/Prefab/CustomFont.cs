using ComfyJamSummer.Configs;
using ComfyJamSummer.Enums;
using Nez;
using Nez.BitmapFonts;

namespace ComfyJamSummer.Prefab
{
    public class CustomFont : SceneComponent
    {
        BitmapFont font_in_game;
        BitmapFont font_giant;
        BitmapFont font_big;
        BitmapFont font_normal;
        BitmapFont font_small;
        BitmapFont font_smallest;
        BitmapFont font_tiny;

        public CustomFont(CustomFontConfig config)
        {
            Load(config);
        }

        public BitmapFont FontInGame { get => font_in_game; }
        public BitmapFont FontGiant { get => font_giant; }
        public BitmapFont FontBig { get => font_big; }
        public BitmapFont FontNormal { get => font_normal; }
        public BitmapFont FontSmall { get => font_small; }
        public BitmapFont FontTiny { get => font_tiny; }

        public BitmapFont FontSmallest()
        {
            switch (Game1.ChosenResolution)
            {
                case GameResolution._1280x720:
                case GameResolution._1366x768:
                    return font_smallest;

                default:
                    return font_tiny;
            }
        }

        private void Load(CustomFontConfig config)
        {
            if (Core.Content == null)
            {
                return;
            }

            var content = Core.Content;

            font_in_game = content.LoadBitmapFont(config.InGameDirectory);
            font_giant = content.LoadBitmapFont(config.GiantDirectory);
            font_big = content.LoadBitmapFont(config.BigDirectory);
            font_normal = content.LoadBitmapFont(config.NormalDirectory);
            font_small = content.LoadBitmapFont(config.SmallDirectory);
            font_smallest = content.LoadBitmapFont(config.SmallestDirectory);
            font_tiny = content.LoadBitmapFont(config.TinyDirectory);
        }
    }
}
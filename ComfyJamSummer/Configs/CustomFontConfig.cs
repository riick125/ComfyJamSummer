namespace ComfyJamSummer.Configs
{
    public class CustomFontConfig
    {
        public string InGameDirectory { get; private set; } = "Content/Fonts/in_game/nice_font_in_game.fnt";
        public string GiantDirectory { get; private set; } = "Content/Fonts/giant/nice_font_giant.fnt";
        public string BigDirectory { get; private set; } = "Content/Fonts/big/nice_font_big.fnt";
        public string NormalDirectory { get; private set; } = "Content/Fonts/normal/nice_font.fnt";
        public string SmallDirectory { get; private set; } = "Content/Fonts/small/nice_font_small.fnt";
        public string SmallestDirectory { get; private set; } = "Content/Fonts/smallest/nice_font_smallest.fnt";
        public string TinyDirectory { get; private set; } = "Content/Fonts/smallest/nice_font_smallest.fnt";

        public CustomFontConfig(string giantDirectory, string bigDirectory, string normalDirectory, string smallDirectory, string smallestDirectory, string tinyDirectory)
        {
            GiantDirectory = giantDirectory;
            BigDirectory = bigDirectory;
            NormalDirectory = normalDirectory;
            SmallDirectory = smallDirectory;
            SmallestDirectory = smallestDirectory;
            TinyDirectory = tinyDirectory;
        }

        public CustomFontConfig()
        {
        }
    }
}
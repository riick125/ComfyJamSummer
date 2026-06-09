using Nez.UI;

namespace ComfyJamSummer.UI.CustomImages
{
    public class SpriteImage
    {
        public Image Image { get; set; }
        public SpriteDrawable SpriteDrawable { get; set; }

        public SpriteImage(Image image, SpriteDrawable spriteDrawable)
        {
            Image = image;
            SpriteDrawable = spriteDrawable;
        }
    }
}

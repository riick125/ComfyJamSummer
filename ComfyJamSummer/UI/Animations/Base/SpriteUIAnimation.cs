using Nez.Textures;
using Nez.UI;

namespace ComfyJamSummer.UI.Animations.Base
{
    public class SpriteUIAnimation
    {
        public readonly Sprite[] Sprites;
        public readonly SpriteDrawable[] Drawables;
        public Image[] Images;
        public readonly float FrameRate;

        public SpriteUIAnimation(Sprite[] sprites, float frameRate)
        {
            Sprites = sprites;
            FrameRate = frameRate;

            Images = new Image[Sprites.Length];
            Drawables = new SpriteDrawable[Sprites.Length];

            for (int i = 0; i < Sprites.Length; i++)
            {
                var sprite = Sprites[i];

                Images[i] = new Image();

                var drawable = new SpriteDrawable(sprite);
                Drawables[i] = drawable;

                Images[i].SetDrawable(drawable);
                Images[i].SetOrigin(Images[i].GetWidth() / 2, Images[i].GetHeight() / 2);
            }
        }
    }
}

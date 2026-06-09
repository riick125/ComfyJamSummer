using ComfyJamSummer.Components.Gameplay;
using ComfyJamSummer.Helpers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Nez.UI;

namespace ComfyJamSummer.UI.CustomElements
{
    public class CustomImage : Image
    {
        public SqueezeAnimation SqueezeAnimation { get; set; }
        public float StartShakingDelay { get; set; }

        public float ShakeFrequency { get; set; }

        protected readonly float _originalWidth, _originalHeight;

        public float OriginalWidth { get { return _originalWidth; } }

        public float OriginalHeight { get { return _originalHeight; } }

        public Vector2 OriginalPosition { get; set; }

        public CustomImage(Texture2D texture, int width = 0, int height = 0, bool shouldSqueeze = false) : base(texture)
        {
            if (shouldSqueeze)
            {
                SqueezeAnimation = UtilHelper.CreateSqueezeAnimation(this, Nez.Random.Range(0.9f, 0.91f));
            }

            StartShakingDelay = Nez.Random.Range(0f, 0.75f);

            ShakeFrequency = Nez.Random.Range(2.5f, 3f);

            width = width > 0 ? width : texture.Width;
            height = height > 0 ? height : texture.Height;

            _originalWidth = width;
            _originalHeight = height;

            SetOrigin(width / 2f, height / 2f);
        }

        public override Element SetPosition(float x, float y)
        {
            if (OriginalPosition == default)
            {
                OriginalPosition = new Vector2(x, y);
            }

            return base.SetPosition(x, y);
        }
    }

    public class CustomLabel : Label
    {
        public SqueezeAnimation SqueezeAnimation { get; set; }
        public CustomLabel(string text, LabelStyle style, bool shouldSqueeze = false) : base(text, style)
        {
            if (shouldSqueeze)
            {
                SqueezeAnimation = UtilHelper.CreateSqueezeAnimation(this, Nez.Random.Range(0.9f, 0.91f));
            }
        }
    }
}
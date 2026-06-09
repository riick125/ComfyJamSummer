using ComfyJamSummer.Components.Gameplay;
using ComfyJamSummer.Helpers;
using ComfyJamSummer.Prefab;
using Nez.UI;

namespace ComfyJamSummer.UI.CustomButtons
{
    public class CustomImageButton : ImageButton
    {
        protected Prefabs _prefabs;

        public SqueezeAnimation SqueezeAnimation { get; set; }
        public float StartShakingDelay { get; set; }

        public float ShakeFrequency { get; set; }

        public CustomImageButton(Prefabs prefabs, ImageButtonStyle style, bool shouldSqueeze = false) : base(style)
        {
            if (shouldSqueeze)
            {
                SqueezeAnimation = UtilHelper.CreateSqueezeAnimation(this, Nez.Random.Range(0.9f, 0.91f));
            }

            StartShakingDelay = Nez.Random.Range(0f, 0.00085f);

            ShakeFrequency = Nez.Random.Range(2.5f, 3f);

            _prefabs = prefabs;
        }
    }
}
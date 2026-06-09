using Nez.UI;

namespace ComfyJamSummer.PoolObjects
{
    public class SqueezeAnimationPoolConfig
    {
        public Element Element { get; set; }

        public float MinOriginalValue { get; set; }

        public float StartDelay { get; set; }

        public SqueezeAnimationPoolConfig(Element element, float minOriginalValue = 0.88f, float startDelay = 0f)
        {
            MinOriginalValue = minOriginalValue;
            Element = element;
            StartDelay = startDelay;
        }
    }
}
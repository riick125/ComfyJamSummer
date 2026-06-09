using Microsoft.Xna.Framework.Graphics;
using Nez;

namespace ComfyJamSummer.CustomPostProcessors
{
    public class SaturationPostProcessor : PostProcessor
    {
        private float _saturationFactor;

        public SaturationPostProcessor(int executionOrder, float saturationFactor) : base(executionOrder)
        {
            Effect = Core.Content.LoadEffect("effects/IncreaseSaturationEffect.mgfxo");

            _saturationFactor = saturationFactor;

            Effect.Parameters["SaturationFactor"].SetValue(_saturationFactor);
        }

        public void SetSaturationFactor(float saturationFactor)
        {
            Effect.Parameters["SaturationFactor"].SetValue(saturationFactor);
        }

        public override void Process(RenderTarget2D source, RenderTarget2D destination)
        {
            base.Process(source, destination);
        }
    }
}

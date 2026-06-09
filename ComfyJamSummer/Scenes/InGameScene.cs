using ComfyJamSummer.Components.Extensions;
using ComfyJamSummer.CustomPostProcessors;
using ComfyJamSummer.Scenes.Base;

namespace ComfyJamSummer.Scenes
{
    public class InGameScene : CustomScene
    {
        private SaturationPostProcessor _saturationPostProcessor;

        public SaturationPostProcessor SaturationPostProcessor { get => _saturationPostProcessor; set => _saturationPostProcessor = value; }

        public override void Initialize()
        {
            base.Initialize();

            _saturationPostProcessor = new SaturationPostProcessor(2, Constants.SATURATION_FACTOR_NORMAL);
            AddPostProcessor(_saturationPostProcessor);

            AddSceneComponent(new BesideTextRegistry());
        }
    }
}
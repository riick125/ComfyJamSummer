using ComfyJamSummer.Helpers;
using ComfyJamSummer.Prefab;
using Nez;

namespace ComfyJamSummer.Components
{
    public class CustomSceneComponent : SceneComponent
    {
        protected Prefabs _prefabs;

        public CustomSceneComponent()
        {
            _prefabs = UtilHelper.Prefabs();
        }

        public override void OnEnabled()
        {
            base.OnEnabled();

            if (_prefabs == null)
            {
                _prefabs = UtilHelper.Prefabs();
            }
        }
    }
}
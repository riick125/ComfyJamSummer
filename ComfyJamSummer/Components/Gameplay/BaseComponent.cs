using ComfyJamSummer.Manager;
using ComfyJamSummer.Prefab;
using Nez;

namespace ComfyJamSummer.Components.Gameplay
{
    public class BaseComponent : Component
    {
        protected GameManager _manager;
        protected Prefabs _prefabs;

        public BaseComponent(GameManager manager, Prefabs prefabs)
        {
            _manager = manager;
            _prefabs = prefabs;
        }
    }
}
using ComfyJamSummer.Entities.Base;
using ComfyJamSummer.Manager;
using ComfyJamSummer.Prefab;
using Nez;
using Nez.AI.FSM;

namespace ComfyJamSummer.AI
{
    public class BaseAIState<T> : State<T> where T : Animated
    {
        protected readonly Scene _scene;

        protected readonly Camera _camera;

        protected readonly Prefabs _prefabs;

        protected readonly GameManager _manager;

        public BaseAIState(Prefabs prefabs, GameManager manager)
        {
            _prefabs = prefabs;
            _manager = manager;

            _scene = Core.Scene;
            _camera = _scene?.Camera;
        }

        public override void Update(float deltaTime)
        {
        }

        protected bool Validate()
        {
            if (_scene == null || _camera == null || _context == null)
                return false;

            if (_prefabs == null)
                return false;

            if (_manager == null)
                return false;

            return !_manager.CantDoAnyAction;
        }
    }
}
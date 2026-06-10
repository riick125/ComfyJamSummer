using ComfyJamSummer.Entities.Creatures;
using ComfyJamSummer.Manager;
using ComfyJamSummer.Prefab;
using Nez;

namespace ComfyJamSummer.Components.Gameplay
{
    public class BaseComponent : Component
    {
        protected Scene _scene;
        protected Camera _camera;
        protected GameManager _manager;
        protected Prefabs _prefabs;

        public BaseComponent(GameManager manager, Prefabs prefabs)
        {
            _manager = manager;
            _prefabs = prefabs;
        }

        public override void OnAddedToEntity()
        {
            base.OnAddedToEntity();

            _scene = this.Entity?.Scene;

            _camera = this.Entity?.Scene?.Camera;
        }

        protected bool Validate(Entity entity)
        {
            if (_scene == null || _camera == null)
                return false;

            if (_prefabs == null)
                return false;

            if (_manager == null)
                return false;

            if (entity == null)
            {
                return false;
            }
            else
            {
                if (entity.IsDestroyed)
                {
                    return false;
                }

                switch (entity)
                {
                    case Creature creature:
                        if (!creature.IsAlive)
                        {
                            return false;
                        }
                        break;
                }
            }

            return !_manager.CantDoAnyAction;
        }
    }
}
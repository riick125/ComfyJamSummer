using ComfyJamSummer.Components.Extensions;
using ComfyJamSummer.Components.General;
using ComfyJamSummer.Entities.Base;
using ComfyJamSummer.Helpers;
using ComfyJamSummer.Manager;
using ComfyJamSummer.Prefab;
using Nez;

namespace ComfyJamSummer.Scenes.Base
{
    public class CustomScene : BaseScene
    {
        int width = 1280, height = 720;

        protected GameManager _gameManager;

        protected Prefabs _prefabs;

        BesideTextRegistry _besideTextRegistry;

        public BesideTextRegistry BesideTextRegistry
        {
            get
            {
                if (_besideTextRegistry == null)
                {
                    _besideTextRegistry = this.GetSceneComponent<BesideTextRegistry>();
                }

                return _besideTextRegistry;
            }
        }

        public CustomScene()
        {
            CreateWithDefaultRenderer();
        }

        public Animated CreateEntityCustom(string name)
        {
            var entity = new Animated();
            entity.Name = name;
            return AddEntity(entity);
        }
    }
}
using ComfyJamSummer.Components.Extensions;
using ComfyJamSummer.Entities.Base;

namespace ComfyJamSummer.Scenes.Base
{
    public class CustomScene : BaseScene
    {
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

        public override void Initialize()
        {
            base.Initialize();

            _prefabs?.Load();
        }

        public Animated CreateEntityCustom(string name)
        {
            var entity = new Animated();
            entity.Name = name;
            return AddEntity(entity);
        }
    }
}
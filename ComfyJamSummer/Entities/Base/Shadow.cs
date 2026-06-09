using ComfyJamSummer.Components.General;
using Nez;

namespace ComfyJamSummer.Entities.Base
{
    public class Shadow : Entity
    {
        public float OffsetY { get; set; }

        public RickMover Mover { get { return this.GetComponent<RickMover>(); } }

        public CrazyScaleComponent CrazyScaleComponent { get { return this.GetComponent<CrazyScaleComponent>(); } }

        public override void OnAddedToScene()
        {
            base.OnAddedToScene();

            AddComponent(new CrazyScaleComponent());
        }
    }
}
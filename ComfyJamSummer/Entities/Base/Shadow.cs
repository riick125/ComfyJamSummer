using ComfyJamSummer.Components.General;
using Nez;

namespace ComfyJamSummer.Entities.Base
{
    public class Shadow : Animated
    {
        public float OffsetY { get; set; }

        public RickMover Mover { get { return this.GetComponent<RickMover>(); } }

        public override void OnAddedToScene()
        {
            base.OnAddedToScene();

            AddComponent(new CrazyScaleComponent());
        }
    }
}
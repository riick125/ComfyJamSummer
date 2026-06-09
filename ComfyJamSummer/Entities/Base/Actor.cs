using ComfyJamSummer.Components.General;
using ComfyJamSummer.Enums;
using Nez;
using System.Linq;

namespace ComfyJamSummer.Entities.Base
{
    public class Actor : Animated
    {
        public RickMover Mover { get { return this.GetComponent<RickMover>(); } }

        public CircleCollider BodyCollider { get { return this.GetComponents<CircleCollider>().FirstOrDefault(x => x.Tag == CreatureCollider.Body.ToString()); } }

        public float Speed { get; set; }

        public void AddMover()
        {
            if (Mover != null)
            {
                return;
            }

            AddComponent<RickMover>();
        }
    }
}
using ComfyJamSummer.Components.General;
using ComfyJamSummer.Enums;
using Nez;
using System.Linq;

namespace ComfyJamSummer.Entities.Base
{
    public class Actor : Animated
    {
        public float ActualHP { get; set; }

        public float MaxHP { get; set; }

        public virtual bool IsAlive { get { return ActualHP > 0; } }

        public float PreviousHP { get; set; }

        public float LastReduceValueTaken { get; set; }

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
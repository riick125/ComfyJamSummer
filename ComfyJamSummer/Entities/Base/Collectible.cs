using ComfyJamSummer.Components.Extensions;
using ComfyJamSummer.Enums;
using Microsoft.Xna.Framework;
using Nez;
using System.Linq;

namespace ComfyJamSummer.Entities.Base
{
    public class Collectible : InteractableObject
    {
        public CollectibleType Type { get; set; }

        public bool FollowingCatcher { get; internal set; }

        public Vector2 Speed { get; set; } = new Vector2(225, 225);

        public float TimeLeftToBeCollected { get; set; }

        private float _timeLeftToBeCollected = 0.35f;

        public bool CanBeCollected { get { return TimeLeftToBeCollected <= 0 && !FollowingCatcher; } }

        public CircleCollider BodyCollider { get { return GetComponents<CircleCollider>().FirstOrDefault(x => x.Tag == CollisionTag.Body.ToString()); } }

        public CircleCollider CatchAreaCollider { get { return GetComponents<CircleCollider>().FirstOrDefault(x => x.Tag == CollisionTag.CatchArea.ToString()); } }

        public CircleCollider ShowPopupItemInfoAreaCollider { get { return GetComponents<CircleCollider>().FirstOrDefault(x => x.Tag == CollisionTag.ShowPopupItemInfoArea.ToString()); } }

        protected BounceComponent _bounceComponent;

        public BounceComponent BulletCapsuleComponent
        {
            get
            {
                return this.GetComponent<BounceComponent>();

            }
            private set { }
        }
        public bool Squeezed { get; set; }
        private float _squeezeDelay = 0.015f;

        public Collectible CloneCollectible(uint islandId, CollectibleType type, Vector2 pos)
        {
            var name = type.ToString().ToLower().Replace("_", " ");

            var clone = base.CloneInteractable(islandId, pos, 0, 0, $"Press [E] to collect {name}") as Collectible;

            clone.SetScale(0);
            clone.RotationDegrees = Nez.Random.Range(30, 100);

            return clone;
        }
    }
}
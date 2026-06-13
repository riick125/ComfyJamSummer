using ComfyJamSummer.Components.Extensions;
using ComfyJamSummer.Components.Visuals;
using ComfyJamSummer.Enums;
using ComfyJamSummer.Helpers;
using Microsoft.Xna.Framework;
using Nez;
using System;
using System.Linq;

namespace ComfyJamSummer.Entities.Base
{
    public class Collectible : InteractableObject
    {
        public CollectibleType Type { get; set; }

        public bool FollowingCatcher { get; internal set; }

        public Vector2 Speed { get; set; } = new Vector2(225, 225);

        public float TimeLeftToBeCollected { get; set; }

        public bool CanBeCollected { get { return TimeLeftToBeCollected <= 0 && !FollowingCatcher; } }

        public CircleCollider BodyCollider { get { return GetComponents<CircleCollider>().FirstOrDefault(x => x.Tag == CollisionTag.Body.ToString()); } }

        public CircleCollider CatchAreaCollider { get { return GetComponents<CircleCollider>().FirstOrDefault(x => x.Tag == CollisionTag.CatchArea.ToString()); } }

        public CircleCollider ShowPopupItemInfoAreaCollider { get { return GetComponents<CircleCollider>().FirstOrDefault(x => x.Tag == CollisionTag.ShowPopupItemInfoArea.ToString()); } }

        protected BounceComponent _bounceComponent;

        public BounceComponent BounceComponent
        {
            get
            {
                return this.GetComponent<BounceComponent>();

            }
            private set { }
        }

        public Collectible CloneCollectible(uint islandId, Vector2 pos, Vector2 fallDestination)
        {
            var name = Type.ToString().ToLower().Replace("_", " ");

            var clone = base.CloneInteractable(islandId, pos, 0, 0, $"Press [E] to collect {name}") as Collectible;
            clone.FallDestination = fallDestination;

            clone.AddComponent(new JuicyAppear(UtilHelper.GameManager(), UtilHelper.Prefabs()));

            var distanceY = Math.Abs(pos.Y - fallDestination.Y);

            clone.AddComponent(new FakeShadowComponent(8, distanceY, false));

            clone.AddComponent(new BounceComponent(islandId, new Vector2(Nez.Random.Range(75, 150)), 3));

            return clone;
        }

        public override void OnAddedToScene()
        {
            base.OnAddedToScene();

            var direction = DirectionHelper.PerpendicularDirection(Position, FallDestination);

            FallDirection = direction;
        }
    }
}
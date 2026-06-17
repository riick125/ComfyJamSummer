using ComfyJamSummer.Components.Extensions;
using ComfyJamSummer.Components.Gameplay;
using ComfyJamSummer.Components.Visuals;
using ComfyJamSummer.Entities.Configs;
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

        public bool IsCollected { get; set; }

        protected bool CanBeCollected => Validate() && CanPressInteractButton && BounceComponent != null && BounceComponent.IsOnFloor;

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

        public Collectible CloneCollectible(InteractableConfig config, Vector2 fallDestination)
        {
            var name = Type.ToString().ToLower().Replace("_", " ");

            config.InteractText = $"Press [E] to collect {name}";

            var clone = base.CloneInteractable(config) as Collectible;
            clone.FallDestination = fallDestination;
            clone.Type = Type;

            var manager = UtilHelper.GameManager();

            var prefabs = UtilHelper.Prefabs();

            clone.AddComponent(new JuicyAppear(manager, prefabs));

            clone.AddComponent(new CollectibleController(manager, prefabs));

            var distanceY = Math.Abs(config.Position.Y - fallDestination.Y);

            clone.AddComponent(new FakeShadowComponent(8, distanceY, false));

            clone.AddComponent(new BounceComponent(config.IslandId, new Vector2(Nez.Random.Range(75, 150)), 3));

            clone.AddComponent(new CircleCollider(8) { IsTrigger = true, Tag = CollisionTag.CatchArea.ToString() });

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
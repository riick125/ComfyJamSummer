using ComfyJamSummer.Entities.Base;
using ComfyJamSummer.Entities.Configs;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Nez;

namespace ComfyJamSummer.Entities.Collectibles
{
    public class FriedChicken : Collectible
    {
        public FriedChicken CloneFried(InteractableConfig config, Vector2 fallDestination)
        {
            var clone = base.CloneCollectible(config, fallDestination) as FriedChicken;

            return clone;
        }

        public override void Update()
        {
            base.Update();

            if (!CanBeCollected)
            {
                return;
            }

            if (Input.IsKeyPressed(Keys.E) && !IsInteracting)
            {
                StartInteractingWithPlayer();
                FollowingCatcher = true;
            }
        }
    }
}
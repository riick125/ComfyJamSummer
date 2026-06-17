using ComfyJamSummer.Entities.Base;
using ComfyJamSummer.Entities.Configs;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Nez;

namespace ComfyJamSummer.Entities.Collectibles
{
    public class SlicedBread : Collectible
    {
        public SlicedBread CloneBread(InteractableConfig config, Vector2 fallDestination)
        {
            var clone = base.CloneCollectible(config, fallDestination) as SlicedBread;
            clone.TalkAreaCollider.SetRadius(12);
            return clone;
        }

        public override void Update()
        {
            base.Update();

            if (!Validate())
            {
                return;
            }
            else if (CanBeCollected)
            {
                if (Input.IsKeyPressed(Keys.E) && !IsInteracting)
                {
                    StartInteractingWithPlayer();
                    FollowingCatcher = true;
                }
            }
        }
    }
}
using ComfyJamSummer.Entities.Base;
using ComfyJamSummer.Entities.Configs;
using ComfyJamSummer.Helpers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Nez;

namespace ComfyJamSummer.Entities.Collectibles
{
    public class Sandwich : Collectible
    {
        float _healPercentage = 0.25f;

        public Sandwich CloneSandwich(InteractableConfig config, Vector2 fallDestination)
        {
            var clone = base.CloneCollectible(config, fallDestination) as Sandwich;

            clone._healPercentage = _healPercentage;

            return clone;
        }

        public override void Update()
        {
            base.Update();

            if (!Validate())
            {
                return;
            }

            if (!IsCollected && CanBeCollected)
            {
                if (Input.IsKeyPressed(Keys.E) && !IsInteracting)
                {
                    StartInteractingWithPlayer();
                    FollowingCatcher = true;
                }
            }
            else
            {
                if (Input.IsKeyPressed(Keys.Space))
                {
                    var player = UtilHelper.Player();

                    if (player != null)
                    {
                        player.Heal(player.MaxHP * _healPercentage);
                        player.DevouredSandwiches++;
                    }

                    var crab = UtilHelper.Crab();

                    if (crab != null)
                    {
                        crab.ReactToPlayerEatingSandwich(player, this.Id);
                    }

                    this.Destroy();

                    player.Sandwich = null;
                }
            }
        }
    }
}
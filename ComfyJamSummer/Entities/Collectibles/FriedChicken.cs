using ComfyJamSummer.Entities.Base;
using ComfyJamSummer.Enums;
using Microsoft.Xna.Framework;

namespace ComfyJamSummer.Entities.Collectibles
{
    public class FriedChicken : Collectible
    {
        public FriedChicken CloneFried(uint islandId, Vector2 pos, Vector2 fallDestination)
        {
            var clone = base.CloneCollectible(islandId, pos, fallDestination) as FriedChicken;

            return clone;
        }
    }
}
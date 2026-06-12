using ComfyJamSummer.Entities.Base;
using ComfyJamSummer.Enums;
using Microsoft.Xna.Framework;

namespace ComfyJamSummer.Entities.Collectibles
{
    public class FriedChicken : Collectible
    {
        public FriedChicken CloneFried(uint islandId, Vector2 pos)
        {
            var clone = base.CloneCollectible(islandId, CollectibleType.Fried_Chicken, pos) as FriedChicken;

            return clone;
        }
    }
}
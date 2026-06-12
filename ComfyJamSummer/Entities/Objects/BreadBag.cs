using ComfyJamSummer.Entities.Base;
using Microsoft.Xna.Framework;

namespace ComfyJamSummer.Entities.Objects
{
    public class BreadBag : InteractableObject
    {
        public BreadBag CloneBread(uint islandId, Vector2 pos)
        {
            var clone = base.CloneInteractable(islandId, pos, 0, 0) as BreadBag;

            return clone;
        }
    }
}
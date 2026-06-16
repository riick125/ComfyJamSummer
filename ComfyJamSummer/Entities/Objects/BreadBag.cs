using ComfyJamSummer.Entities.Base;
using ComfyJamSummer.Enums;
using ComfyJamSummer.Extensions;
using Microsoft.Xna.Framework;

namespace ComfyJamSummer.Entities.Objects
{
    public class BreadBag : InteractableObject
    {
        public BreadBag CloneBread(uint islandId, Vector2 pos)
        {
            var clone = base.CloneInteractable(islandId, pos, 0, 0) as BreadBag;

            clone.DepthHeight = clone.SpriteHeight / 2;

            clone.CreateCollider(CollisionLayer.Map, clone.SpriteWidth * 0.9f, clone.SpriteHeight / 6, offset: new Vector2(0, clone.SpriteHeight / 3.2f));

            return clone;
        }
    }
}
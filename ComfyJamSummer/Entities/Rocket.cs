using ComfyJamSummer.Entities.Base;
using ComfyJamSummer.Enums;
using ComfyJamSummer.Helpers;
using Microsoft.Xna.Framework;
using Nez;

namespace ComfyJamSummer.Entities
{
    public class Rocket : InteractableObject
    {
        public Rocket CloneRocket(uint islandId, Vector2 pos)
        {
            var clone = base.CloneInteractable(islandId, pos, 0, 0, "Press [E] to escape!") as Rocket;

            clone.AddComponent(new BoxCollider(clone.SpriteWidth, clone.SpriteHeight / 6f)
            {
                LocalOffset = new Vector2(0, clone.SpriteHeight / 3.5f),
                CollidesWithLayers = (int)CollisionLayer.Player,
                PhysicsLayer = (int)CollisionLayer.Map
            });

            AnimHelper.Play(clone.Animator, RocketAnim.Build_1);

            return clone;
        }
    }
}
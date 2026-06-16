using ComfyJamSummer.Enums;
using Microsoft.Xna.Framework;
using Nez;

namespace ComfyJamSummer.Extensions
{
    public static class AnimatedExt
    {
        public static Collider CreateCollider(this Entity entity, CollisionLayer physicsLayer, float x, float y, float width, float height, string tag = null, Vector2 offset = default)
        {
            var collidesWithLayers = GetLayers(physicsLayer);

            var collider = new BoxCollider(x, y, width, height);

            HydrateValues(entity, collider, physicsLayer, collidesWithLayers, tag, offset);

            return collider;
        }

        public static Collider CreateCollider(this Entity entity, CollisionLayer physicsLayer, float width, float height, string tag = null, Vector2 offset = default)
        {
            var collidesWithLayers = GetLayers(physicsLayer);

            var collider = new BoxCollider(width, height);

            HydrateValues(entity, collider, physicsLayer, collidesWithLayers, tag, offset);

            return collider;
        }

        public static Collider CreateCircleCollider(this Entity entity, CollisionLayer physicsLayer, float radius, string tag = null, Vector2 offset = default)
        {
            var collidesWithLayers = GetLayers(physicsLayer);

            var circleCollider = new CircleCollider(radius);

            HydrateValues(entity, circleCollider, physicsLayer, collidesWithLayers, tag, offset);

            return circleCollider;
        }

        static int GetLayers(CollisionLayer physicsLayer)
        {
            var collidesWithLayers = 0;

            switch (physicsLayer)
            {
                case CollisionLayer.Player:
                    collidesWithLayers = (int)CollisionLayer.Map | (int)CollisionLayer.Enemy;
                    break;

                case CollisionLayer.Map:
                    collidesWithLayers = (int)CollisionLayer.Player;
                    break;
            }

            return collidesWithLayers;
        }

        static void HydrateValues(Entity entity, Collider collider, CollisionLayer physicsLayer, int collidesWithLayers, string tag = null, Vector2 offset = default)
        {
            if (collider == null)
            {
                return;
            }

            collider.CollidesWithLayers = collidesWithLayers;
            collider.PhysicsLayer = (int)physicsLayer;
            collider.Tag = tag != "" ? tag : collider.Tag;
            collider.LocalOffset = offset != default ? offset : collider.LocalOffset;

            entity.AddComponent(collider);
        }
    }
}
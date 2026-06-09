using System.Linq;
using Microsoft.Xna.Framework;
using Nez;

namespace ComfyJamSummer.Components.General
{
    public class RickMover : Component
    {
        ColliderTriggerHelper _triggerHelper;

        public override void OnAddedToEntity()
        {
            _triggerHelper = new ColliderTriggerHelper(Entity);
        }

        public bool CalculateMovement(ref Vector2 motion, out CollisionResult collisionResult)
        {
            collisionResult = new CollisionResult();

            if (Entity != null)
            {
                // no collider? just move and forget about it
                if (Entity.GetComponent<Collider>() == null || _triggerHelper == null)
                    return false;

                // 1. move all non-trigger Colliders and get closest collision
                var colliders = Entity.GetComponents<Collider>();

                if (colliders != null)
                {
                    for (var i = 0; i < colliders.Count; i++)
                    {
                        var collider = colliders[i];

                        if (collider == null)
                        {
                            continue;
                        }

                        if (collider.Entity == null)
                        {
                            continue;
                        }

                        // skip triggers for now. we will revisit them after we move.
                        if (collider.IsTrigger)
                            continue;

                        // fetch anything that we might collide with at our new position
                        var bounds = collider.Bounds;
                        bounds.X += motion.X;
                        bounds.Y += motion.Y;
                        var neighbors =
                            Physics.BoxcastBroadphaseExcludingSelf(collider, ref bounds, collider.CollidesWithLayers).ToList();

                        foreach (var neighbor in neighbors)
                        {
                            if (neighbor == null)
                            {
                                continue;
                            }

                            if (neighbor.Entity == null)
                            {
                                continue;
                            }

                            // skip triggers for now. we will revisit them after we move.
                            if (neighbor.IsTrigger)
                                continue;

                            if (collider.CollidesWith(neighbor, motion, out CollisionResult _InternalcollisionResult))
                            {
                                // hit. back off our motion
                                motion -= _InternalcollisionResult.MinimumTranslationVector;

                                // If we hit multiple objects, only take on the first for simplicity sake.
                                if (_InternalcollisionResult.Collider != null)
                                    collisionResult = _InternalcollisionResult;
                            }
                        }
                    }

                    ListPool<Collider>.Free(colliders);
                }
            }

            return collisionResult.Collider != null;
        }

        public void ApplyMovement(Vector2 motion)
        {
            if (Entity == null)
            {
                return;
            }

            if (Entity.Transform == null)
            {
                return;
            }

            Entity.Transform.Position += motion;

            _triggerHelper?.Update();
        }

        public bool Move(Vector2 motion, out CollisionResult collisionResult)
        {
            CalculateMovement(ref motion, out collisionResult);

            ApplyMovement(motion);

            return collisionResult.Collider != null;
        }
    }
}
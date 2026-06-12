using Microsoft.Xna.Framework;

namespace ComfyJamSummer.Helpers
{
    public static class DirectionHelper
    {
        public static bool ValidateVelocity(Vector2 direction, Vector2 vel)
        {
            if (float.IsNaN(direction.X) || float.IsNaN(direction.Y))
            {
                return false;
            }
            else if (float.IsInfinity(vel.X) || float.IsInfinity(vel.Y))
            {
                return false;
            }

            return true;
        }

        public static bool Validate(Vector2 direction)
        {
            if (float.IsNaN(direction.X) || float.IsNaN(direction.Y))
            {
                return false;
            }

            return true;
        }

        public static Vector2 PerpendicularDirection(Vector2 source, Vector2 destination, float force = 32)
        {
            var perpendicularDirection = destination - source;
            perpendicularDirection.Normalize();

            perpendicularDirection = new Vector2(-perpendicularDirection.Y, perpendicularDirection.X);

            var surroundingPosition = destination + perpendicularDirection * force;

            var destinationDirection = surroundingPosition - source;
            destinationDirection.Normalize();

            return destinationDirection;
        }
    }
}
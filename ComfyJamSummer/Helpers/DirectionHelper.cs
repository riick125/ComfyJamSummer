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
    }
}
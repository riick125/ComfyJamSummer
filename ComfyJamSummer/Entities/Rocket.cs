using ComfyJamSummer.Entities.Base;
using Microsoft.Xna.Framework;

namespace ComfyJamSummer.Entities
{
    public class Rocket : Animated
    {
        public Rocket CloneRocket(Vector2 pos)
        {
            var clone = base.CloneAnimated(pos) as Rocket;

            return clone;
        }
    }
}
using ComfyJamSummer.Entities.Base;
using Microsoft.Xna.Framework;

namespace ComfyJamSummer.Entities
{
    public class StarFish : Animated
    {
        public StarFish CloneStarFish(Vector2 pos)
        {
            var clone = base.CloneAnimated(pos) as StarFish;

            return clone;
        }
    }
}
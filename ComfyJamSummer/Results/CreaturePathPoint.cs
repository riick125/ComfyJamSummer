using Microsoft.Xna.Framework;
using Point = Microsoft.Xna.Framework.Point;

namespace ComfyJamSummer.Results
{
    public class CreaturePathPoint
    {
        public CreaturePathPoint(uint creatureId, Point point)
        {
            CreatureId = creatureId;
            Point = point;
        }

        public uint CreatureId { get; set; }

        public Point Point { get; set; }
    }
}
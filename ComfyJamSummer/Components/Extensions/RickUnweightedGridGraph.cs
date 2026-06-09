using System.Collections.Generic;
using System.Linq;
using ComfyJamSummer.Entities.Creatures;
using ComfyJamSummer.Results;
using Nez.Tiled;
using Point = Microsoft.Xna.Framework.Point;

namespace ComfyJamSummer.Components.Extensions
{
    public class RickUnweightedGridGraph : IRickUnweightedGraph<Point>
    {

        static readonly Point[] CARDINAL_DIRS = {
            new Point(1, 0),
            new Point(0, -1),
            new Point(-1, 0),
            new Point(0, 1),
        };

        static readonly Point[] COMPASS_DIRS = {
            new Point(1, 0),
            new Point(1, -1),
            new Point(0, -1),
            new Point(-1, -1),
            new Point(-1, 0),
            new Point(-1, 1),
            new Point(0, 1),
            new Point(1, 1),
        };

        public HashSet<Point> Walls = new HashSet<Point>();
        public HashSet<CreaturePathPoint> Creatures = new HashSet<CreaturePathPoint>();

        int _width, _height;
        Point[] _dirs;
        List<Point> _neighbors = new List<Point>(4);


        public RickUnweightedGridGraph(int width, int height, bool allowDiagonalSearch = false)
        {
            _width = width;
            _height = height;
            _dirs = allowDiagonalSearch ? COMPASS_DIRS : CARDINAL_DIRS;
        }

        public RickUnweightedGridGraph(TmxLayer tiledLayer, bool allowDiagonalSearch = false)
        {
            _width = tiledLayer.Width;
            _height = tiledLayer.Height;
            _dirs = allowDiagonalSearch ? COMPASS_DIRS : CARDINAL_DIRS;

            for (var y = 0; y < tiledLayer.Map.Height; y++)
            {
                for (var x = 0; x < tiledLayer.Map.Width; x++)
                {
                    if (tiledLayer.GetTile(x, y) != null)
                        Walls.Add(new Point(x, y));
                }
            }
        }

        public bool IsNodeInBounds(Point node)
        {
            return 0 <= node.X && node.X < _width && 0 <= node.Y && node.Y < _height;
        }

        public bool IsNodePassable(Creature creature, Point node)
        {
            var result = !Walls.Contains(node) && !Creatures.Any(x => x.CreatureId != creature.Id && x.Point == node);
            return result;
        }

        public List<Point> Search(Creature creature, Point start, Point goal) => RickBreadthFirstPathfinder.Search(creature, this, start, goal);

        public IEnumerable<Point> GetNeighbors(Creature creature, Point node)
        {
            _neighbors.Clear();

            foreach (var dir in _dirs)
            {
                var next = new Point(node.X + dir.X, node.Y + dir.Y);
                if (IsNodeInBounds(next) && IsNodePassable(creature, next))
                    _neighbors.Add(next);
            }

            return _neighbors;
        }
    }
}

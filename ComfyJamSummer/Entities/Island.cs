using ComfyJamSummer.Enums;
using Microsoft.Xna.Framework;
using Nez;

namespace ComfyJamSummer.Entities
{
    public class Island : Entity
    {
        private SpawnArea _westArea, _eastArea;

        public int WavesQuantity { get; private set; }

        public string TmxDirectory { get; set; }

        public TiledMapRenderer Renderer { get { return this.GetComponent<TiledMapRenderer>(); } }

        public float Width
        {
            get
            {

                if (Renderer == null)
                {
                    return 0;
                }

                return Renderer.Width;
            }
        }

        public float Height
        {
            get
            {

                if (Renderer == null)
                {
                    return 0;
                }

                return Renderer.Height;
            }
        }

        public float MaxPositionX => Position.X + Renderer.Width - TileWidth;

        public float MaxPositionY => Position.Y + Renderer.Height - TileHeight;

        public int TileWidth => Renderer == null ? 0 : Renderer.TiledMap.TileWidth;

        public int TileHeight => Renderer == null ? 0 : Renderer.TiledMap.TileWidth;

        public Island CloneIsland(int wavesQty, Vector2 pos)
        {
            var clone = base.Clone(pos) as Island;
            clone.WavesQuantity = wavesQty;
            clone.TmxDirectory = TmxDirectory;

            var clonedTiledMap = Core.Content.LoadTiledMap(clone.TmxDirectory);

            clone.RemoveComponent<TiledMapRenderer>();

            var renderer = clone.AddComponent(new TiledMapRenderer(clonedTiledMap) { RenderLayer = Constants.MAP_RENDER_LAYER });

            renderer.CollisionLayer = renderer.TiledMap?.TileLayers[TiledLayerNames.WALLS];

            renderer.AddColliders();

            var colliders = renderer.GetColliders();

            if (colliders != null)
            {
                foreach (var item in colliders)
                {
                    item.CollidesWithLayers = (int)CollisionLayer.Player;
                    item.PhysicsLayer = (int)CollisionLayer.Map;
                }
            }

            return clone;
        }

        public override void OnAddedToScene()
        {
            base.OnAddedToScene();

            var highHeight = Height * 1.4f;
            var midHeight = highHeight / 2;

            var minWest = CenterPosition() + new Vector2(-(Width * 1.5f), -highHeight);

            var maxWest = new Vector2(minWest.X + Width / 4, minWest.Y + midHeight);

            _westArea = new SpawnArea(minWest, maxWest);

            var minEast = CenterPosition() + new Vector2(Width / 4, -highHeight);

            var maxEast = new Vector2(minEast.X + (Width * 1.5f), minWest.Y + midHeight);

            _eastArea = new SpawnArea(minEast, maxEast);
        }

        public Vector2 CenterPosition()
        {
            if (Renderer == null)
            {
                return default;
            }

            return this.Position + new Vector2(Width / 2, Height / 2);
        }

        public Vector2 GetRandomWestPosition()
        {
            return ProcessRandomPosition(_westArea);
        }

        public Vector2 GetRandomEastPosition()
        {
            return ProcessRandomPosition(_eastArea);
        }

        Vector2 ProcessRandomPosition(SpawnArea area)
        {
            if (area == null)
            {
                return default;
            }

            var x = Nez.Random.Range(area.MinPosition.X, area.MaxPosition.X);
            var y = Nez.Random.Range(area.MinPosition.Y, area.MaxPosition.Y);

            return new Vector2(x, y);
        }

        public class SpawnArea
        {
            public Vector2 MinPosition { get; set; }
            public Vector2 MaxPosition { get; set; }

            public SpawnArea(Vector2 minPos, Vector2 maxPos)
            {
                MinPosition = minPos;
                MaxPosition = maxPos;
            }
        }
    }
}
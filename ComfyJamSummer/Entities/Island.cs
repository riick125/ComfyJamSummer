using ComfyJamSummer.Components.Gameplay;
using ComfyJamSummer.Enums;
using Microsoft.Xna.Framework;
using Nez;
using Nez.Tiled;
using System.Collections.Generic;
using System.Linq;

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

        public Vector2 MinPosition => new Vector2(Position.X + TileWidth, Position.Y + TileHeight);

        public Vector2 MaxPosition => new Vector2(Position.X + Renderer.Width - TileWidth, Position.Y + Renderer.Height - TileHeight);

        public int TileWidth => Renderer == null ? 0 : Renderer.TiledMap.TileWidth;

        public int TileHeight => Renderer == null ? 0 : Renderer.TiledMap.TileWidth;

        public Island CloneIsland(int wavesQty, Vector2 pos)
        {
            var clone = base.Clone(pos) as Island;
            clone.WavesQuantity = wavesQty;
            clone.TmxDirectory = TmxDirectory;

            var clonedTiledMap = Core.Content.LoadTiledMap(clone.TmxDirectory);

            clone.RemoveComponent<TiledMapRenderer>();

            clone.AddComponent(new TiledMapRenderer(clonedTiledMap) { RenderLayer = Constants.MAP_RENDER_LAYER });

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

        public TmxLayerTile GetTile(string layerName, int x, int y)
        {
            if (string.IsNullOrEmpty(layerName))
            {
                return null;
            }

            var layer = GetLayer(layerName);

            return layer?.GetTile(x, y);
        }

        public Vector2 GetCornerPosition(string layerName, GenericDirectionPlus direction)
        {
            var layer = GetLayer(layerName);

            List<Vector2> filtered = null;

            if (layer != null)
            {
                var tiles = layer.Tiles.Where(x => x != null).Select(x => Position + new Vector2(x.X * TileWidth, x.Y * TileHeight)).ToList();


                var horizontalSpacing = Width * 0.15f;
                var verticalSpacing = Height * 0.15f;

                var center = CenterPosition();

                switch (direction)
                {
                    case GenericDirectionPlus.Top:
                        break;

                    case GenericDirectionPlus.TopRight:
                        filtered = tiles
                            .Where(x => x.X >= (center.X + horizontalSpacing) &&
                            x.X < (MaxPosition.X - TileWidth) &&
                            x.Y > (MinPosition.Y + TileHeight) && x.Y < (center.Y - verticalSpacing)).ToList();

                        return filtered[Nez.Random.Range(0, filtered.Count)];

                    case GenericDirectionPlus.TopLeft:
                        filtered = tiles
                            .Where(x => x.X <= (center.X - horizontalSpacing) &&
                            x.X > (MinPosition.X + (TileWidth * 2)) &&
                            x.Y > (MinPosition.Y + (TileHeight * 2)) && x.Y < (center.Y - verticalSpacing)).ToList();
                        break;

                    case GenericDirectionPlus.Bottom:
                        break;

                    case GenericDirectionPlus.BottomRight:
                        break;

                    case GenericDirectionPlus.BottomLeft:
                        break;

                    case GenericDirectionPlus.Right:
                        break;

                    case GenericDirectionPlus.Left:
                        break;
                }
            }

            if (filtered != null && filtered.Any())
            {
                return filtered[Nez.Random.Range(0, filtered.Count)];
            }

            return default;
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

        TmxLayer GetLayer(string layerName)
        {
            return Renderer?.TiledMap?.GetLayer<TmxLayer>(layerName);
        }
    }
}
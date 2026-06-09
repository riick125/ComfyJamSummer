using ComfyJamSummer.Enums;
using Microsoft.Xna.Framework;
using MonoGame.Extended.Tiled;
using Nez;

namespace ComfyJamSummer.Entities
{
    public class Island : Entity
    {
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

        public Island CloneIsland(Vector2 pos)
        {
            var clone = base.Clone(pos) as Island;
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
                    item.CollidesWithLayers = (int)CollisionLayer.Player | (int)CollisionLayer.Enemy;
                    item.PhysicsLayer = (int)CollisionLayer.Map;
                }
            }

            return clone;
        }

        public Vector2 CenterPosition()
        {
            if (Renderer == null)
            {
                return default;
            }

            return this.Position + new Vector2(Width / 2, Height / 2);
        }
    }
}
using Microsoft.Xna.Framework;
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

            clone.AddComponent(new TiledMapRenderer(clonedTiledMap, TiledLayerNames.WALLS) { RenderLayer = Constants.MAP_RENDER_LAYER });

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
using Microsoft.Xna.Framework;
using Nez;

namespace ComfyJamSummer.Entities
{
    public class Island : Entity
    {
        public string TmxDirectory { get; set; }

        public TiledMapRenderer Renderer { get { return this.GetComponent<TiledMapRenderer>(); } }

        public Island CloneIsland(Vector2 pos)
        {
            var clone = base.Clone(pos) as Island;
            clone.TmxDirectory = TmxDirectory;

            var clonedTiledMap = Core.Content.LoadTiledMap(clone.TmxDirectory);

            clone.RemoveComponent<TiledMapRenderer>();

            clone.AddComponent(new TiledMapRenderer(clonedTiledMap) { RenderLayer = Constants.MAP_RENDER_LAYER });

            return clone;
        }
    }
}
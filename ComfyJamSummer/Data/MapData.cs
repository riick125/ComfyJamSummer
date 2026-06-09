using AutoMapper;
using ComfyJamSummer.Entities;
using ComfyJamSummer.Prefab;
using Nez;

namespace ComfyJamSummer.Data
{
    public class MapData : BaseData
    {
        public MapData(Prefabs prefabs, IMapper mapper) : base(Constants.MAP_DATA_PATH, prefabs, mapper)
        {
        }

        public Island Create()
        {
            var island = new Island();

            var dir = _rootDir + "island.tmx";

            if (FileExists(dir))
            {
                island.TmxDirectory = dir;

                var tmxMap = Core.Content.LoadTiledMap(dir);

                island.AddComponent(new TiledMapRenderer(tmxMap, TiledLayerNames.WALLS) { RenderLayer = Constants.MAP_RENDER_LAYER});
            }

            return island;
        }
    }
}
using AutoMapper;
using ComfyJamSummer.Entities;
using ComfyJamSummer.Entities.Base;
using ComfyJamSummer.Entities.TextureData;
using ComfyJamSummer.Prefab;
using Microsoft.Xna.Framework;
using Nez;

namespace ComfyJamSummer.Data
{
    public class MapData : BaseData
    {
        public MapData(Prefabs prefabs, IMapper mapper) : base(Constants.MAP_TMX_PATH, prefabs, mapper)
        {
        }

        public Island Create()
        {
            var island = new Island();

            var folder = "tmx/";

            var fileName = "island.tmx";

            if (FileExists(folder + fileName))
            {
                var dir = _rootDir + fileName;

                island.TmxDirectory = dir;

                var tmxMap = Core.Content.LoadTiledMap(dir);

                island.AddComponent(new TiledMapRenderer(tmxMap, TiledLayerNames.WALLS) { RenderLayer = Constants.MAP_RENDER_LAYER });
            }

            return island;
        }

        public Animated CreateStone()
        {
            var dir = Constants.MAP_DATA_PATH + "stone";

            var texture = Core.Content.LoadTexture(dir);

            var stone = new Animated() { SpriteWidth = 23, SpriteHeight = 15, DepthHeight = -3 };

            CreateSpriteRenderer(stone, texture, Constants.CREATURE_RENDER_LAYER);

            return stone;
        }

        public Animated CreateWaterTile()
        {
            var dir = Constants.MAP_DATA_PATH + "water";

            var tile = new Animated() { SpriteWidth = 16, SpriteHeight = 16 };
            CreateAnimatorOneAnimation(tile, dir, "Idle", Constants.BACKGROUND_RENDER_LAYER);

            return tile;
        }
    }
}
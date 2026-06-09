using AutoMapper;
using ComfyJamSummer.Prefab;
using Microsoft.Xna.Framework.Graphics;
using Nez;

namespace ComfyJamSummer.Data
{
    public class LogoData : BaseData
    {
        public LogoData(Prefabs prefabs, IMapper mapper) : base(Constants.ART_DATA_PATH, prefabs, mapper)
        {
        }

        public Texture2D CreateKakapoHeadLogo()
        {
            return Core.Content.LoadTexture($"{_rootDir}kakapo_head_logo");
        }

        public Texture2D CreateKakapoTextLogo()
        {
            return Core.Content.LoadTexture($"{_rootDir}kakapo_text_logo");
        }
    }
}

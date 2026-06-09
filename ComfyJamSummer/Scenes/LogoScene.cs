using ComfyJamSummer.Scenes.Base;
using ComfyJamSummer.UI;
using Microsoft.Xna.Framework;
using Nez;

namespace ComfyJamSummer.Scenes
{
    public class LogoScene : CustomScene
    {
        public override void Initialize()
        {
            base.Initialize();

            ClearColor = Constants.BG_COLOR_LOGO;

            Camera.SetPosition(new Vector2(Screen.Width / 2, Screen.Height / 2));
        }

        public override void OnStart()
        {
            base.OnStart();

            CreateEntityCustom(UINames.COMPANY_LOGO).AddComponent(new CompanyLogoUI(_gameManager, _prefabs));
        }
    }
}

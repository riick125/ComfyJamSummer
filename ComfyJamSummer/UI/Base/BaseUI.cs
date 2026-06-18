using ComfyJamSummer.Helpers;
using ComfyJamSummer.Manager;
using ComfyJamSummer.Prefab;
using Nez;
using Nez.UI;

namespace ComfyJamSummer.UI.Base
{
    public class BaseUI : UICanvas
    {
        protected LabelStyle _lblStyleSmall, _lblStyleNormal,
            lblStyleBig, _lblStyleGiant;

        protected Container _container;

        protected Prefabs _prefabs;

        protected GameManager _manager;

        public BaseUI(GameManager manager, Prefabs prefabs)
        {
            IsFullScreen = true;
            RenderLayer = Game1.ScreenSpaceRenderLayer;

            var customFont = UtilHelper.CustomFont();

            _container = Stage.AddElement(new Container() { FillParent = true });

            _prefabs = prefabs;

            _manager = manager;

            if (customFont != null)
            {
                _lblStyleSmall = new LabelStyle(customFont.FontSmall, Constants.WHITE_COLOR);

                _lblStyleNormal = new LabelStyle(customFont.FontNormal, Constants.WHITE_COLOR);

                lblStyleBig = new LabelStyle(customFont.FontBig, Constants.WHITE_COLOR);

                _lblStyleGiant = new LabelStyle(customFont.FontGiant, Constants.WHITE_COLOR);
            }
        }

        protected bool Validate()
        {
            if (_prefabs == null || _manager == null)
            {
                return false;
            }

            if (_manager.CantDoAnyAction)
            {
                return false;
            }

            return true;
        }
    }
}
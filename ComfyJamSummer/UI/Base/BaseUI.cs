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

        protected Scene _scene;

        protected Camera _camera;

        protected Prefabs _prefabs;

        protected GameManager _manager;

        protected CustomFont _customFont;

        public BaseUI(GameManager manager, Prefabs prefabs)
        {
            IsFullScreen = true;
            RenderLayer = Game1.ScreenSpaceRenderLayer;

            _customFont = UtilHelper.CustomFont();

            _container = Stage.AddElement(new Container() { FillParent = true });

            _prefabs = prefabs;

            _manager = manager;

            if (_customFont != null)
            {
                _lblStyleSmall = new LabelStyle(_customFont.FontSmall, Constants.WHITE_COLOR);

                _lblStyleNormal = new LabelStyle(_customFont.FontNormal, Constants.WHITE_COLOR);

                lblStyleBig = new LabelStyle(_customFont.FontBig, Constants.WHITE_COLOR);

                _lblStyleGiant = new LabelStyle(_customFont.FontGiant, Constants.WHITE_COLOR);
            }
        }

        public override void OnAddedToEntity()
        {
            base.OnAddedToEntity();

            _scene = this.Entity.Scene;

            _camera = _scene.Camera;
        }

        protected bool Validate()
        {
            if (_prefabs == null || _manager == null || _scene == null || _camera == null)
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
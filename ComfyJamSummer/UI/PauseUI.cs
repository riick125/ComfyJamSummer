using ComfyJamSummer.Enums;
using ComfyJamSummer.Extensions;
using ComfyJamSummer.Manager;
using ComfyJamSummer.Prefab;
using ComfyJamSummer.UI.Base;
using Nez;
using Nez.UI;

namespace ComfyJamSummer.UI
{
    public class PauseUI : BaseUI
    {
        Label _lblPaused;

        public PauseUI(GameManager manager, Prefabs prefabs) : base(manager, prefabs)
        {
            _container.SetBackground(new PrimitiveDrawable(Constants.BG_COLOR * 0.95f));

            Enabled = false;           
        }

        public override void OnAddedToEntity()
        {
            base.OnAddedToEntity();

            var font = Game1.SaveData?.ChosenResolution > GameResolution._1366x768 ? _customFont.FontGiant : _customFont.FontBig;

            _lblPaused = _container.AddElement(new Label("PAUSED", font));

            UIHelper.CentralizeElementPosXInScreen(_lblPaused, Screen.Height * 0.25f);
        }
    }
}
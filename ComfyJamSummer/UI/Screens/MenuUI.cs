using ComfyJamSummer.Enums;
using ComfyJamSummer.Extensions;
using ComfyJamSummer.Manager;
using ComfyJamSummer.Prefab;
using ComfyJamSummer.UI.Base;
using Nez.UI;

namespace ComfyJamSummer.UI.Screens
{
    public class MenuUI : BaseUI
    {
        Label _lblPressToPlay;

        public MenuUI(GameManager manager, Prefabs prefabs) : base(manager, prefabs)
        {
            _container.SetBackground(new PrimitiveDrawable(Constants.BG_COLOR * 0.95f));
        }

        public override void OnAddedToEntity()
        {
            base.OnAddedToEntity();

            var font = Game1.SaveData?.ChosenResolution > GameResolution._1366x768 ? _customFont.FontGiant : _customFont.FontBig;

            _lblPressToPlay = _container.AddElement(new Label("Press any key to play!", font));

            UIHelper.CentralizeElementInScreen(_lblPressToPlay);
        }
    }
}
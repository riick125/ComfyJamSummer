using ComfyJamSummer.Components.Gameplay;
using ComfyJamSummer.Extensions;
using ComfyJamSummer.Helpers;
using ComfyJamSummer.Manager;
using ComfyJamSummer.Prefab;
using ComfyJamSummer.UI.Base;
using Nez;
using Nez.UI;

namespace ComfyJamSummer.UI
{
    public class WaveUI : BaseUI
    {
        string _txtStarting = "Next wave starts in {0} seconds...";

        string _txtMakeASandwich = "You need to feed the crab to advance to the next wave.";

        Label _lblStarting, _lblSandwich;

        BattleComponent _battleComponent;

        public WaveUI(GameManager manager, Prefabs prefabs) : base(manager, prefabs)
        {

        }

        public override void OnAddedToEntity()
        {
            base.OnAddedToEntity();

            _customFont = UtilHelper.CustomFont();

            if (_customFont != null)
            {
                _lblStarting = _container.AddElement(new Label(_txtStarting, _customFont.FontNormal));
                UIHelper.CentralizeElementPosXInScreen(_lblStarting, Screen.Height * 0.25f);
                _lblStarting.SetVisible(false);

                _lblSandwich = _container.AddElement(new Label(_txtMakeASandwich, _customFont.FontNormal));
                _lblSandwich.SetVisible(false);
                UIHelper.CentralizeElementPosXInScreen(_lblSandwich, Screen.Height * 0.25f);
            }

            _battleComponent = UtilHelper.GetComponent<BattleComponent>();
        }

        public override void Update()
        {
            base.Update();

            if (_battleComponent == null)
            {
                return;
            }

            if (_battleComponent.ActualWave == null)
            {
                return;
            }

            _lblSandwich.SetVisible(_battleComponent != null && _battleComponent.WaitingToFeedCrab);

            var seconds = _battleComponent.ActualWave.TimeLeftToStart <= 1 ? 1 : _battleComponent.ActualWave.TimeLeftToStart;

            if (_lblSandwich.IsVisible())
            {

            }
            _lblStarting.SetText(string.Format(_txtStarting,(int)seconds));
            _lblStarting.SetVisible(!_battleComponent.ActualWave.Started && !_lblSandwich.IsVisible());
        }
    }
}

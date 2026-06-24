using ComfyJamSummer.Components.Gameplay;
using ComfyJamSummer.Extensions;
using ComfyJamSummer.Helpers;
using ComfyJamSummer.Manager;
using ComfyJamSummer.Prefab;
using ComfyJamSummer.UI.Base;
using Microsoft.Xna.Framework;
using Nez;
using Nez.UI;
using System;

namespace ComfyJamSummer.UI
{
    public class WaveUI : BaseUI
    {
        string _txtStarting = "Next wave starts in {0} seconds...";

        string _txtMakeASandwich = "You need to feed the crab to advance to the next wave.";

        string _txtWave = "Wave: {0}";

        Label _lblStarting, _lblSandwich, _lblWave;

        BattleComponent _battleComponent;

        float _weakAlpha = 0.68f;

        public WaveUI(GameManager manager, Prefabs prefabs) : base(manager, prefabs)
        {
        }

        public override void OnAddedToEntity()
        {
            base.OnAddedToEntity();

            _customFont = UtilHelper.CustomFont();

            _battleComponent = UtilHelper.GetComponent<BattleComponent>();

            if (_customFont != null)
            {
                _lblStarting = _container.AddElement(new Label(_txtStarting, _customFont.FontNormal));
                UIHelper.CentralizeElementPosXInScreen(_lblStarting, Screen.Height * 0.25f);
                _lblStarting.SetVisible(false);

                _lblSandwich = _container.AddElement(new Label(_txtMakeASandwich, _customFont.FontNormal));
                _lblSandwich.SetVisible(false);
                UIHelper.CentralizeElementPosXInScreen(_lblSandwich, Screen.Height * 0.25f);

                _lblWave = _container.AddElement(new Label(_txtWave, new LabelStyle(_customFont.FontNormal, Constants.WHITE_COLOR)));
                _lblWave.SetVisible(false);
            }
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

            _lblStarting.SetText(string.Format(_txtStarting, (int)seconds));
            _lblStarting.SetVisible(!_battleComponent.ActualWave.Started && !_lblSandwich.IsVisible());

            if (_lblWave != null)
            {
                var playerCanvas = UIHelper.GetCanvas(UINames.PLAYER) as PlayerUI;

                if (playerCanvas != null)
                {
                    _lblWave.SetVisible(true);

                    var color = _battleComponent.ActualWave.Started ? Constants.RED_COLOR : Constants.WHITE_COLOR * _weakAlpha;

                    if (_battleComponent.ActualWave.Started)
                    {
                        color = _battleComponent.ActualWave.StillGoing ? Constants.RED_COLOR : Constants.WHITE_COLOR * _weakAlpha;
                    }

                    _lblWave.SetFontColor(color);

                    var waveNumber = _battleComponent.ActualWave.Index;

                    if (_battleComponent.ActualWave.InitiatedCountdown)
                    {
                        if (!_battleComponent.ActualWave.Started && waveNumber > 1)
                        {
                            waveNumber--;
                        }
                    }

                    waveNumber = Math.Clamp(waveNumber, 1, int.MaxValue);

                    _lblWave.SetText(string.Format(_txtWave, waveNumber));
                    _lblWave.SetPosition(playerCanvas.LblPoints.GetX(), playerCanvas.LblPoints.GetY() + playerCanvas.LblPoints.Height() * 1.25f);
                }
            }
        }
    }
}

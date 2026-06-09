using ComfyJamSummer.Enums;
using ComfyJamSummer.Helpers;
using ComfyJamSummer.Manager;
using ComfyJamSummer.Prefab;
using ComfyJamSummer.UI.Base;
using ComfyJamSummer.UI.CustomLabels;
using Microsoft.Xna.Framework;
using Nez;

namespace ComfyJamSummer.UI
{
    public class SavingGameUI : BaseUI
    {
        private InflateImage _imgSaving;

        private string _txtSaving;
        private InflateLabel _lblSaving;

        private bool _finishing;
        private float _timeElapsed, _minLifeTime = 3f;

        private float _alpha = 0.9f, _alphaDecreaseVelocity = 1.5f;

        public SavingGameUI(GameManager gameManager, Prefabs prefabs) : base(gameManager, prefabs)
        {
            if (prefabs != null)
            {
                //_imgSaving = new InflateImage(prefabs.GetUITexture(UISpritesEnums.bit_rick_head), 1.1f, inflateSpeed: 1.3f);

                _imgSaving.SetPosition((Screen.Width / 2), (Screen.Height * 0.98f) - _imgSaving.PreferredHeight);

                _imgSaving.SetColor(Color.White * _alpha);

                _container.AddElement(_imgSaving);

                //var texts = prefabs.GetTextsByName(prefabs.GeneralGameplayGameTextJsonData, GameTextsArchivesNamesEnum.general);

                //_txtSaving = prefabs.GetTextByName(texts, GeneralGameTextsNamesEnum.saving_game);

                var customFont = UtilHelper.CustomFont();

                if (customFont != null)
                {
                    var font = Game1.SaveData.ChosenResolution > GameResolution._1366x768 ? customFont.FontNormal : customFont.FontSmall;

                    _lblSaving = new InflateLabel(_txtSaving, new Nez.UI.LabelStyle(font, Constants.SPRITE_COLOR), 1.1f, inflateSpeed: 1.3f);

                    _lblSaving.SetPosition((_imgSaving.GetX()), (_imgSaving.GetY() + (_imgSaving.PreferredHeight)));

                    _lblSaving.SetFontColor(Constants.SPRITE_COLOR * _alpha);

                    _container.AddElement(_lblSaving);
                }
            }
        }

        public override void Update()
        {
            base.Update();

            if (this.Entity.IsDestroyed)
            {
                return;
            }

            _timeElapsed += Time.AltDeltaTime;

            if (_timeElapsed >= _minLifeTime && _finishing)
            {
                _container.ClearChildren();

                _container.Remove();

                this.RemoveComponent();

                this.Entity.Destroy();

                return;
            }
            else if (_timeElapsed >= (_minLifeTime * 0.8f))
            {
                _alpha -= _alphaDecreaseVelocity * Time.AltDeltaTime;

                _alpha = Mathf.Clamp01(_alpha);

                _imgSaving.SetColor(Color.White * _alpha);
                _lblSaving.SetFontColor(Constants.SPRITE_COLOR * _alpha);

                if (_alpha == 0)
                {
                    Finish();
                }
            }

            _imgSaving.Process();
            _lblSaving.Process();
        }

        public void Finish()
        {
            if (!_finishing)
            {
                _finishing = true;
            }
        }
    }
}
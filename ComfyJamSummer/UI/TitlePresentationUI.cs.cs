using ComfyJamSummer.Extensions;
using ComfyJamSummer.Helpers;
using Nez;
using Nez.UI;

namespace ComfyJamSummer.UI
{
    public class TitlePresentationUI : UICanvas
    {
        private Container _container;

        private Label _lblTitle;
        private LabelStyle _lblTitleStyle;

        private Label _lblSubTitle;
        private LabelStyle _lblSubTitleStyle;

        private float _startDelay;
        private bool _appearing;

        private float _executionTime;

        private float _titleAlpha, _titleMaxAlpha;
        private float _alphaGain = 2f;

        private bool _giantText, _textOnCenter;

        private float _alphaDecreaserPercentage = 1f;

        public TitlePresentationUI(string title, float executionTime = 1.75f, bool giantText = true, bool textOnCenter = false, float startDelay = 0.25f)
        {
            Initialize(title, executionTime, giantText, textOnCenter, startDelay);
        }

        public TitlePresentationUI(string title, string subTitle, float executionTime = 1.75f, bool giantText = true, bool textOnCenter = false, float startDelay = 0.25f)
        {
            Initialize(title, executionTime, giantText, textOnCenter, startDelay, subTitle);
        }

        private void Initialize(string title, float executionTime, bool giantText, bool textOnCenter, float startDelay, string subtitle = null)
        {
            _startDelay = startDelay;
            _giantText = giantText;
            _textOnCenter = textOnCenter;
            _executionTime = executionTime;

            IsFullScreen = Game1.SaveData.IsFullScreen;

            _container = new Container() { FillParent = true };

            Stage.AddElement(_container);

            CreateTitle(title, subtitle);
        }

        private void CreateTitle(string title, string subTitle = null)
        {
            _appearing = true;
            _titleAlpha = 0f;
            _titleMaxAlpha = 1f;
            var customFont = UtilHelper.CustomFont();

            if (customFont == null)
            {
                this.RemoveComponent();
                return;
            }

            var font = _giantText ? customFont.FontGiant : customFont.FontBig;

            var text = font.WrapText(title, 400f);

            _lblTitleStyle = new LabelStyle(font, Constants.WHITE_COLOR * _titleAlpha);

            _lblTitle = new Label(title, _lblTitleStyle);

            if (_textOnCenter)
            {
                UIHelper.CentralizeElementPosXInScreen(_lblTitle, Nez.Screen.Height * 0.25f);
            }
            else
            {
                UIHelper.CentralizeElementPosXInScreen(_lblTitle, Nez.Screen.Height * 0.12f);
            }

            _container.AddElement(_lblTitle);

            if (!string.IsNullOrEmpty(subTitle))
            {
                var spacing = Screen.Height * 0.005f;

                _lblSubTitleStyle = new LabelStyle(customFont.FontNormal, Constants.WHITE_COLOR * _titleAlpha);

                _lblSubTitle = new Label(subTitle, _lblSubTitleStyle);
                _lblSubTitle.SetPosition((_lblTitle.GetX() + _lblTitle.PreferredWidth / 2) - _lblSubTitle.PreferredWidth / 2, (_lblTitle.GetY() + _lblTitle.PreferredHeight) + spacing);

                _container.AddElement(_lblSubTitle);
            }
        }

        public override void Update()
        {
            base.Update();

            if (this.Entity.IsDestroyed)
            {
                return;
            }

            this.Entity.SetPosition(this.Entity.Scene.Camera.Position);

            var player = UtilHelper.Player();

            _alphaDecreaserPercentage = 1f;

            var deltaTime = Time.AltDeltaTime;

            if (_startDelay <= 0)
            {
                UIHelper.ProcessShaking(_lblTitle, 2.5f);

                UIHelper.ProcessShaking(_lblSubTitle, 2.5f);

                _lblTitle.SetFontColor(Constants.WHITE_COLOR * (_titleAlpha * _alphaDecreaserPercentage));

                _lblSubTitle?.SetFontColor(Constants.WHITE_COLOR * (_titleAlpha * _alphaDecreaserPercentage));

                if (_appearing)
                {
                    _titleAlpha += _alphaGain * deltaTime;

                    if (_titleAlpha >= _titleMaxAlpha)
                    {
                        _appearing = false;
                    }
                }
                else
                {
                    if (_executionTime <= 0f)
                    {
                        _titleAlpha -= _alphaGain * deltaTime;

                        if (_titleAlpha <= 0)
                        {
                            _container.ClearChildren();

                            _container.Clear();

                            this.Entity.Destroy();
                        }
                    }
                    else
                    {
                        _executionTime -= deltaTime;
                    }
                }

                _titleAlpha = Mathf.Clamp(_titleAlpha, 0, _titleMaxAlpha);
            }
            else
            {
                _startDelay -= deltaTime;
            }
        }
    }
}

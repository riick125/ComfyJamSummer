using Nez;
using Nez.UI;

namespace ComfyJamSummer.UI.CustomLabels
{
    public class InflateLabel : Label
    {
        private readonly float _originalScale;

        private readonly float _maxScale;

        private bool _isInflating = true;

        private float _inflateSpeed;

        private float _cooldownBetweenInflate = 0.05f, _timeLeftToNextInflate;

        private float _velMeterScale;
        private readonly float _velMeterMinValue;

        private float? _originalPosX, _originalPosY;

        public InflateLabel(string text, LabelStyle style, float scaleMultiplier, float originalScale = 1f, float inflateSpeed = 1.9f) : base(text, style)
        {
            inflateSpeed = Mathf.Clamp(inflateSpeed, 0.5f, 1.9f);

            _inflateSpeed = inflateSpeed;

            _velMeterMinValue = inflateSpeed * 0.25f;

            _maxScale = originalScale * scaleMultiplier;

            _originalScale = originalScale;
        }

        public override Element SetPosition(float x, float y)
        {
            if (!_originalPosX.HasValue && !_originalPosY.HasValue)
            {
                _originalPosX = x;
                _originalPosY = y;
            }

            return base.SetPosition(x, y);
        }

        public void Process()
        {
            var lblStyle = GetStyle();

            if (lblStyle != null)
            {
                var deltaTime = Time.AltDeltaTime;

                if (_timeLeftToNextInflate <= 0)
                {
                    ChangeVelMeter();

                    var scale = lblStyle.FontScaleX;

                    if (_isInflating)
                    {
                        scale += _velMeterScale * deltaTime;

                        if (scale >= _maxScale)
                        {
                            _isInflating = false;
                            _velMeterScale = _velMeterMinValue;
                            _timeLeftToNextInflate = _cooldownBetweenInflate;
                        }
                    }
                    else
                    {
                        scale -= _velMeterScale * deltaTime;

                        if (scale <= _originalScale)
                        {
                            _isInflating = true;
                            _velMeterScale = _velMeterMinValue;
                            _timeLeftToNextInflate = _cooldownBetweenInflate;
                        }
                    }

                    scale = Mathf.Clamp(scale, _originalScale, _maxScale);

                    this.SetFontScale(scale);

                    var measureString = lblStyle.Font.MeasureString(this.GetText()) * scale;
                    var offsetX = measureString.X / 2;
                    var offsetY = measureString.Y / 2;

                    if (_originalPosX == null || _originalPosY == null)
                    {
                        return;
                    }

                    this.SetPosition(_originalPosX.Value - offsetX, _originalPosY.Value - offsetY);
                }
                else
                {
                    _timeLeftToNextInflate -= deltaTime;
                }
            }
        }


        private void ChangeVelMeter()
        {
            var deltaTime = Time.DeltaTime;

            _velMeterScale += (_inflateSpeed * 0.15f) * deltaTime;

            _velMeterScale = Mathf.Clamp(_velMeterScale, _velMeterMinValue, _inflateSpeed);
        }
    }
}
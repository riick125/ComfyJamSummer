using Microsoft.Xna.Framework.Graphics;
using Nez;
using Nez.UI;

namespace ComfyJamSummer.UI.CustomLabels
{
    public class InflateImage : Image
    {
        private readonly float _originalScale;

        private readonly float _maxScale;

        private bool _isInflating = true;

        private float _inflateSpeed;

        private float _cooldownBetweenInflate = 0.05f, _timeLeftToNextInflate;

        private float _velMeterScale;
        private readonly float _velMeterMinValue;

        private float? _originalPosX, _originalPosY;

        public InflateImage(Texture2D texture, float scaleMultiplier, float originalScale = 1f, float inflateSpeed = 30) : base(texture)
        {
            inflateSpeed = Mathf.Clamp(inflateSpeed, 0.5f, 100f);

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
            var deltaTime = Time.AltDeltaTime;

            if (_timeLeftToNextInflate <= 0)
            {
                ChangeVelMeter();

                var scaleX = GetScaleX();
                var scaleY = GetScaleY();

                if (_isInflating)
                {
                    scaleX += _velMeterScale * deltaTime;
                    scaleY += _velMeterScale * deltaTime;

                    if (scaleX >= _maxScale && scaleY >= _maxScale)
                    {
                        _isInflating = false;
                        _velMeterScale = _velMeterMinValue;
                        _timeLeftToNextInflate = _cooldownBetweenInflate;
                    }
                }
                else
                {
                    scaleX -= _velMeterScale * deltaTime;
                    scaleY -= _velMeterScale * deltaTime;

                    if (scaleX <= _originalScale && scaleY <= _originalScale)
                    {
                        _isInflating = true;
                        _velMeterScale = _velMeterMinValue;
                        _timeLeftToNextInflate = _cooldownBetweenInflate;
                    }
                }

                scaleX = Mathf.Clamp(scaleX, _originalScale, _maxScale);
                scaleY = Mathf.Clamp(scaleY, _originalScale, _maxScale);

                this.SetSize(this.PreferredWidth * scaleX, this.PreferredHeight * scaleY);
                SetScale(scaleX, scaleY);

                var offsetX = this.PreferredWidth * scaleX / 2;
                var offsetY = this.PreferredHeight * scaleY / 2;

                this.SetPosition(_originalPosX.Value - offsetX, _originalPosY.Value - offsetY);
            }
            else
            {
                _timeLeftToNextInflate -= deltaTime;
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
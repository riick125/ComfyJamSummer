using System.Collections.Generic;
using System.Linq;
using ComfyJamSummer.Helpers;
using ComfyJamSummer.PoolObjects;
using ComfyJamSummer.UI.CustomButtons;
using ComfyJamSummer.UI.CustomElements;
using Microsoft.Xna.Framework;
using Nez;
using Nez.UI;

namespace ComfyJamSummer.Components.Gameplay
{
    public class SqueezeAnimation
    {
        private bool _isActive;

        private List<SqueezeProcess> _processes;

        private Element _element;

        public void Initialize(SqueezeAnimationPoolConfig config)
        {
            _element = config.Element;

            _element.SetOrigin(_element.PreferredWidth / 2, _element.PreferredHeight / 2);

            _processes = new List<SqueezeProcess>();

            var verticalProcess = Pool<SqueezeProcess>.Obtain();
            var horizontalProcess = Pool<SqueezeProcess>.Obtain();

            verticalProcess.Initialize(config, SqueezeDirection.Vertical, 36);
            horizontalProcess.Initialize(config, SqueezeDirection.Horizontal, 44);

            _processes.Add(verticalProcess);
            _processes.Add(horizontalProcess);
        }

        public void Squeeze()
        {
            if (_isActive)
            {
                return;
            }

            _processes.ForEach(x => { x.Start(); });

            _isActive = true;
        }

        public void SqueezeByDirection(SqueezeDirection direction)
        {
            if (_isActive)
            {
                return;
            }

            _processes.Where(x => x.Direction == direction).ToList().ForEach(x => { x.Start(); });

            _isActive = true;
        }

        public void Update()
        {
            if (_isActive)
            {
                var deltaTime = Time.DeltaTime != 0 ? Time.DeltaTime : Time.AltDeltaTime;

                foreach (var process in _processes)
                {
                    process.Update(deltaTime);
                }

                _isActive = !_processes.All(x => x.IsFinished);
            }
        }

        public class SqueezeProcess
        {
            private float _minOriginalValue, _maxValue = 1f;
            private float _minValue;
            private float _velMeterScale;
            private float _velMeterMinValue;
            private float _velScaleIncreaser;
            private float _cooldownBetweenSqueezes = 0.05f, _timeLeftToNextSqueeze;
            private bool _isSqueezing;

            private bool _finished;

            public bool IsFinished { get { return _finished; } }

            private Element _element;
            private float _originalWidth, _originalHeight;
            private float _originalPosX, _originalPosY;

            private SqueezeDirection _direction;
            public SqueezeDirection Direction { get { return _direction; } }

            private float _startDelay, _timeLeftToEndDelay;

            private float _scale;

            public void Initialize(SqueezeAnimationPoolConfig config, SqueezeDirection direction, float force)
            {
                _minOriginalValue = config.MinOriginalValue;
                _element = config.Element;
                _originalWidth = config.Element.PreferredWidth;
                _originalHeight = config.Element.PreferredHeight;
                _originalPosY = config.Element.GetY();
                _direction = direction;

                _minValue = _minOriginalValue + Nez.Random.Range(-0.05f, 0.05f);

                force = Mathf.Clamp(force, 25, 50);

                _velScaleIncreaser = force;
                _velMeterMinValue = force * 0.25f;
                _startDelay = config.StartDelay;

                switch (_element)
                {
                    case TextButton textButton:
                        var style = textButton.GetStyle();

                        if (style != null)
                        {
                            _maxValue = style.FontScaleX;
                        }
                        break;

                    case Label lbl:
                        var styleLbl = lbl.GetStyle();

                        if (styleLbl != null)
                        {
                            _maxValue = styleLbl.FontScaleX;
                        }
                        break;
                }

                _scale = _maxValue;
            }

            public void Start()
            {
                _finished = false;

                _scale = _maxValue;
                _minValue = _minOriginalValue + Nez.Random.Range(-0.015f, 0.015f);
                _isSqueezing = true;
                _timeLeftToEndDelay = _startDelay;
                _velMeterScale = _velMeterMinValue;
            }

            public void Update(float deltaTime)
            {
                if (_finished)
                {
                    return;
                }

                if (_originalPosX == 0)
                {
                    _originalPosX = _element.GetX();
                    return;
                }

                if (_originalPosY == 0)
                {
                    _originalPosY = _element.GetY();
                    return;
                }

                if (_timeLeftToEndDelay <= 0)
                {
                    if (_timeLeftToNextSqueeze <= 0)
                    {
                        _scale = GetScaleOfElement(_element, _scale);

                        ChangeVelMeter();

                        if (_isSqueezing)
                        {
                            _scale -= _velMeterScale * deltaTime;

                            if (_scale <= _minValue)
                            {
                                _isSqueezing = false;
                                _velMeterScale = _velMeterMinValue;
                                _timeLeftToNextSqueeze = _cooldownBetweenSqueezes;
                            }
                        }
                        else
                        {
                            _scale += _velMeterScale * deltaTime;

                            if (_scale >= _maxValue)
                            {
                                _isSqueezing = true;
                                _finished = true;
                                _velMeterScale = _velMeterMinValue;
                                _timeLeftToNextSqueeze = _cooldownBetweenSqueezes;
                            }
                        }

                        _scale = Mathf.Clamp(_scale, _minValue, _maxValue);

                        var updatedScale = _direction == SqueezeDirection.Vertical ? new Vector2(_element.GetScaleX(), _scale) : new Vector2(_scale, _element.GetScaleY());

                        var newWidth = _originalWidth * updatedScale.X;
                        var newHeight = _originalHeight * updatedScale.Y;

                        var offsetX = (_originalWidth - newWidth) * 0.5f;
                        var offsetY = (_originalHeight - newHeight) * 0.5f;

                        SqueezeElement(_element, newWidth, newHeight, updatedScale);

                        _element.SetPosition(_originalPosX + offsetX, _originalPosY + offsetY);
                    }
                    else
                    {
                        _timeLeftToNextSqueeze -= deltaTime;
                    }
                }
                else
                {
                    _timeLeftToEndDelay -= deltaTime;
                }
            }

            private void SqueezeElement(Element element, float newWidth, float newHeight, Vector2 updatedScale)
            {
                switch (element)
                {
                    case TextButton textButton:
                        var style = textButton.GetStyle();

                        if (style != null)
                        {
                            style.FontScaleX = _direction == SqueezeDirection.Horizontal ? _scale : _maxValue;
                            style.FontScaleY = _direction == SqueezeDirection.Vertical ? _scale : _maxValue;

                            textButton.SetStyle(style);
                        }
                        break;

                    case Label label:
                        var lblStyle = label.GetStyle();

                        if (lblStyle != null)
                        {
                            lblStyle.FontScaleX = _direction == SqueezeDirection.Horizontal ? _scale : _maxValue;
                            lblStyle.FontScaleY = _direction == SqueezeDirection.Vertical ? _scale : _maxValue;

                            label.SetStyle(lblStyle);
                        }
                        break;

                    default:
                        element.SetSize(newWidth, newHeight);
                        element.SetScale(updatedScale.X, updatedScale.Y);
                        break;
                }
            }

            private float GetScaleOfElement(Element element, float scale)
            {
                switch (element)
                {
                    case TextButton textButton:
                        var style = textButton.GetStyle();

                        if (style != null)
                        {
                            scale = _direction == SqueezeDirection.Horizontal ? style.FontScaleX : style.FontScaleY;
                        }
                        break;

                    case Label label:
                        var lblStyle = label.GetStyle();

                        if (lblStyle != null)
                        {
                            scale = _direction == SqueezeDirection.Horizontal ? lblStyle.FontScaleX : lblStyle.FontScaleY;
                        }
                        break;

                    default:
                        scale = _direction == SqueezeDirection.Vertical ? element.GetScaleY() : element.GetScaleX();
                        break;
                }

                return scale;
            }

            private void ChangeVelMeter()
            {
                var deltaTime = Time.DeltaTime;

                _velMeterScale += (_velScaleIncreaser * 0.15f) * deltaTime;

                _velMeterScale = Mathf.Clamp(_velMeterScale, _velMeterMinValue, _velScaleIncreaser);
            }
        }
    }
    public enum SqueezeDirection
    {
        Vertical,
        Horizontal
    }
}
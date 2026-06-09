using System;
using System.Collections.Generic;
using ComfyJamSummer.Components.Gameplay;
using ComfyJamSummer.Extensions;
using ComfyJamSummer.Helpers;
using ComfyJamSummer.Prefab;
using Microsoft.Xna.Framework;
using Nez;
using Nez.UI;
using Label = Nez.UI.Label;

namespace ComfyJamSummer.UI.CustomImages
{
    public class ShakableImageButton : ImageButton, IInputListener, IGamepadFocusable
    {
        protected Prefabs _prefabs;

        protected float deltaTime;

        protected Vector2 _originalPos;
        protected float _timeLeftToEndShake;
        protected float _shakeIntensity, _actualShakeIntensity;
        protected Vector2 _shakeOffset;
        protected float _shakeDegredation = 0.85f;
        protected bool _isShaking;

        protected List<AttachedLabel> _attachedElements;
        public bool IsMuted { get; private set; }

        public float StartFloatingDelay { get; set; }
        public float FloatFrequency { get; set; }

        public SqueezeAnimation SqueezeAnimation { get; set; }

        public ShakableImageButton(Prefabs prefabs, ImageButtonStyle style, float shakeIntensity = 10f, bool isMuted = false) : base(style)
        {
            SqueezeAnimation = UtilHelper.CreateSqueezeAnimation(this, Nez.Random.Range(0.9f, 0.91f));

            StartFloatingDelay = Nez.Random.Range(0.0005f, 0.0085f);

            FloatFrequency = Nez.Random.Range(1.75f, 2.25f);

            IsMuted = isMuted;

            _shakeIntensity = shakeIntensity;

            _attachedElements = new List<AttachedLabel>();

            _prefabs = prefabs;

            _actualShakeIntensity = _shakeIntensity * Nez.Random.Range(0.9f, 1.25f);
        }

        public bool IsShaking { get { return _isShaking; } }

        public void SetOriginalPosition(Vector2 pos)
        {
            if (_originalPos != default)
            {
                return;
            }

            _originalPos = pos;
        }

        public void AddAttachedElement(AttachedLabel element)
        {
            if (element == null)
            {
                return;
            }

            _attachedElements.Add(element);
        }

        public void Shake(float shakeTime = 0.25f)
        {
            if (!_isShaking)
            {
                _timeLeftToEndShake = shakeTime;

                _isShaking = true;
            }
        }

        public virtual void Initialize(float posX, float posY, bool rescale = true)
        {
            if (rescale)
            {
                var scale = UtilHelper.GetGameScale();

                if (scale > 2)
                {
                    UIHelper.RescaleUIElementSize(this);
                }
            }

            this.SetPosition(posX, posY);

            _originalPos = new Vector2(posX, posY);
        }

        public virtual void Update()
        {
            deltaTime = Time.DeltaTime;

            if (_originalPos != default)
            {
                UIHelper.ProcessFloating(this);

                foreach (var item in _attachedElements)
                {
                    UIHelper.ProcessFloating(item);
                }
            }

            if (_isShaking)
            {
                if (_timeLeftToEndShake > 0)
                {
                    _timeLeftToEndShake -= deltaTime;

                    if (Math.Abs(_actualShakeIntensity) > 0f)
                    {
                        if (_shakeOffset.X != 0f || _shakeOffset.Y != 0f)
                        {
                            _shakeOffset.Normalize();
                        }
                        else
                        {
                            _shakeOffset.X = _shakeOffset.X + Nez.Random.NextFloat() - 0.5f;
                            _shakeOffset.Y = _shakeOffset.Y + Nez.Random.NextFloat() - 0.5f;
                        }

                        _shakeOffset *= _actualShakeIntensity;
                        _actualShakeIntensity *= -_shakeDegredation;
                    }

                    var vel = Vector2.Zero;

                    vel += _shakeOffset;

                    if (vel != default)
                    {
                        this.SetPosition(this.GetX() + vel.X, this.GetY() + vel.Y);

                        foreach (var element in _attachedElements)
                        {
                            element.SetPosition(element.GetX() + vel.X, element.GetY() + vel.Y);
                        }
                    }
                }
                else
                {
                    this.SetPosition(_originalPos.X, _originalPos.Y);

                    foreach (var element in _attachedElements)
                    {
                        element.SetPosition(element.OriginalPosition.X, element.OriginalPosition.Y);
                    }

                    _isShaking = false;
                    _shakeOffset = Vector2.Zero;
                    _actualShakeIntensity = _shakeIntensity * Nez.Random.Range(0.95f, 1.25f);
                }
            }
        }
        void IInputListener.OnMouseEnter()
        {
            // play sound

            _mouseOver = true;
        }

        void IInputListener.OnMouseExit()
        {
            _mouseOver = _mouseDown = false;
        }

        #region IGamepadFocusable

        void IGamepadFocusable.OnFocused()
        {
            OnFocused();
        }


        void IGamepadFocusable.OnUnfocused()
        {
            OnUnfocused();
        }


        void IGamepadFocusable.OnActionButtonPressed()
        {
            OnActionButtonPressed();
        }


        void IGamepadFocusable.OnActionButtonReleased()
        {
            OnActionButtonReleased();
        }
        #endregion

        public class AttachedElement : Element
        {
            public Vector2 OriginalPosition { get; set; }
        }

        public class AttachedLabel : Label
        {
            public Vector2 OriginalPosition { get; set; }


            public float StartFloatingDelay { get; set; }
            public float FloatFrequency { get; set; }

            public bool CanGoUp { get; set; }

            public float FloatingHeight { get; set; }

            public AttachedLabel(string text, LabelStyle style, float startFloatingDelay, float floatFrequency, bool canGoUp = true) : base(text, style)
            {
                StartFloatingDelay = startFloatingDelay;

                FloatFrequency = floatFrequency;

                CanGoUp = canGoUp;
            }
        }
    }
}
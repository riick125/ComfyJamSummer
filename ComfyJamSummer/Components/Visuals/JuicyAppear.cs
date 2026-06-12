using ComfyJamSummer.Components.Gameplay;
using ComfyJamSummer.Entities.Base;
using ComfyJamSummer.Manager;
using ComfyJamSummer.Prefab;
using Microsoft.Xna.Framework;
using Nez;

namespace ComfyJamSummer.Components.Visuals
{
    public class JuicyAppear : BaseComponent, IUpdatable
    {
        Animated _target;

        float _scaleIncreaseSpeed, _colorIncreaseSpeed;
        float _rotationSpeed;

        float _startDelay;
        readonly float _minStartDelay, _maxStartDelay;

        readonly float _maxInflateValue;

        JuicyAppearState _state;

        enum JuicyAppearState
        {
            Starting,
            Appearing,
            Inflating,
            Deflating
        }

        public JuicyAppear(GameManager manager, Prefabs prefabs) : base(manager, prefabs)
        {
            _rotationSpeed = 1200;

            _scaleIncreaseSpeed = 4f;
            _colorIncreaseSpeed = 7f;

            _minStartDelay = 0.0001f;
            _maxStartDelay = 0.0009f;

            _startDelay = Nez.Random.Range(_minStartDelay, _maxStartDelay);

            _maxInflateValue = 1.15f;
        }

        public override void OnAddedToEntity()
        {
            base.OnAddedToEntity();

            if (this.Entity != null)
            {
                _target = this.Entity as Animated;

                _target.SetScale(0);
                _target.RotationDegrees = Nez.Random.Range(30, 100);
            }
        }

        public void Update()
        {
            if (!Validate(_target))
                return;

            var deltaTime = Time.DeltaTime;

            var scale = _target.Scale.X;

            switch (_state)
            {
                case JuicyAppearState.Starting:
                    if (_startDelay > 0)
                    {
                        _startDelay -= deltaTime;
                    }
                    else
                    {
                        _state = JuicyAppearState.Appearing;
                    }
                    break;

                case JuicyAppearState.Appearing:
                    scale += _scaleIncreaseSpeed * deltaTime;
                    scale = Mathf.Clamp(scale, 0, 1f);

                    _target.Alpha += _colorIncreaseSpeed * deltaTime;
                    _target.Alpha = Mathf.Clamp(_target.Alpha, 0, 1f);

                    _target.RotationDegrees += _rotationSpeed * deltaTime;

                    _target.SetScale(scale);
                    _target.GetAnyRenderer().SetColor(Color.White * _target.Alpha);

                    if (scale >= 1f && _target.Alpha >= 1f)
                    {
                        _target.RotationDegrees = 0;
                        _state = JuicyAppearState.Inflating;
                    }
                    break;

                case JuicyAppearState.Inflating:
                    scale += (_scaleIncreaseSpeed * 1.5f) * deltaTime;
                    scale = Mathf.Clamp(scale, 0, _maxInflateValue);

                    if (scale >= _maxInflateValue)
                    {
                        _state = JuicyAppearState.Deflating;
                    }
                    break;

                case JuicyAppearState.Deflating:
                    scale -= (_scaleIncreaseSpeed * 0.75f) * deltaTime;
                    scale = Mathf.Clamp(scale, 1f, _maxInflateValue);

                    if (scale <= 1f)
                    {
                        SetEnabled(false);
                        this.RemoveComponent();
                    }
                    break;
            }
        }
    }
}

using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Nez;

namespace ComfyJamSummer.Components.General
{
    public class CrazyScaleComponent : Component, IUpdatable
    {
        private bool _isActive;

        public bool IsActive { get { return _isActive; } }

        private List<SqueezeProcess> _processes;

        public CrazyScaleComponent()
        {
            _processes = new List<SqueezeProcess>();
        }

        public override void OnAddedToEntity()
        {
            base.OnAddedToEntity();

            _processes.Add(new SqueezeProcess(this.Entity, SqueezeDirection.Vertical, 36));
            _processes.Add(new SqueezeProcess(this.Entity, SqueezeDirection.Horizontal, 44, 0.11f));
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
                var deltaTime = Time.DeltaTime;

                foreach (var process in _processes)
                {
                    process.Update(deltaTime);
                }

                _isActive = !_processes.All(x => x.IsFinished);
            }
        }

        public enum SqueezeDirection
        {
            Vertical,
            Horizontal
        }

        public class SqueezeProcess
        {
            private readonly float _minOriginalValue = 0.6f, _maxValue = 1f;
            private float _minValue;
            private float _velMeterScale;
            private readonly float _velMeterMinValue;
            private float _velScaleIncreaser;
            private float _cooldownBetweenSqueezes = 0.05f, _timeLeftToNextSqueeze;
            private bool _isSqueezing;

            private bool _finished;

            public bool IsFinished { get { return _finished; } }

            private Entity _entity;

            private SqueezeDirection _direction;
            public SqueezeDirection Direction { get { return _direction; } }

            private float _startDelay, _timeLeftToEndDelay;

            public SqueezeProcess(Entity entity, SqueezeDirection direction, float force = 30, float startDelay = 0f)
            {
                _entity = entity;
                _direction = direction;

                _minValue = _minOriginalValue + Nez.Random.Range(-0.05f, 0.05f);

                force = Mathf.Clamp(force, 25, 50);

                _velScaleIncreaser = force;
                _velMeterMinValue = force * 0.25f;
                _startDelay = startDelay;
            }

            public void Start()
            {
                _finished = false;

                _minValue = _minOriginalValue + Nez.Random.Range(-0.08f, 0.08f);
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

                if (_timeLeftToEndDelay <= 0)
                {
                    if (_timeLeftToNextSqueeze <= 0)
                    {
                        var scale = _direction == SqueezeDirection.Vertical ? _entity.Scale.Y : _entity.Scale.X;

                        ChangeVelMeter();

                        if (_isSqueezing)
                        {
                            scale -= _velMeterScale * deltaTime;

                            if (scale <= _minValue)
                            {
                                _isSqueezing = false;
                                _velMeterScale = _velMeterMinValue;
                                _timeLeftToNextSqueeze = _cooldownBetweenSqueezes;
                            }
                        }
                        else
                        {
                            scale += _velMeterScale * deltaTime;

                            if (scale >= _maxValue)
                            {
                                _isSqueezing = true;
                                _finished = true;
                                _velMeterScale = _velMeterMinValue;
                                _timeLeftToNextSqueeze = _cooldownBetweenSqueezes;
                            }
                        }

                        scale = Mathf.Clamp(scale, _minValue, _maxValue);

                        var updatedScale = _direction == SqueezeDirection.Vertical ? new Vector2(_entity.Scale.X, scale) : new Vector2(scale, _entity.Scale.Y);

                        _entity.SetScale(updatedScale);
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

            private void ChangeVelMeter()
            {
                var deltaTime = Time.DeltaTime;

                _velMeterScale += (_velScaleIncreaser * 0.15f) * deltaTime;

                _velMeterScale = Mathf.Clamp(_velMeterScale, _velMeterMinValue, _velScaleIncreaser);
            }
        }
    }
}
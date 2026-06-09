using Microsoft.Xna.Framework;
using Nez;
using System;

namespace ComfyJamSummer.Components.Visuals
{
    public class LittleShake : Component, IUpdatable
    {
        float _timeLeftToEndShake, _shakeTime;
        float _shakeIntensity = 1.25f;
        Vector2 _shakeOffset;
        float _shakeDegredation = 0.7f;

        public LittleShake(float shakeTime, float _shakeIntensity = 1.25f, float _shakeDegredation = 0.7f)
        {
        }

        public void Shake()
        {
            _timeLeftToEndShake = _shakeTime;
        }

        public void Update()
        {
            if (this.Entity.IsDestroyed)
            {
                return;
            }

            var deltaTime = Time.DeltaTime;

            if (_timeLeftToEndShake > 0)
            {
                _timeLeftToEndShake -= deltaTime;

                if (Math.Abs(_shakeIntensity) > 0f)
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

                    _shakeOffset *= _shakeIntensity;
                    _shakeIntensity *= -_shakeDegredation;
                }

                this.Entity.SetPosition(this.Entity.Position + _shakeOffset);
            }
            else
            {
                _shakeIntensity = 1.25f;
            }
        }
    }
}

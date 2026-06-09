using System;
using ComfyJamSummer.Helpers;
using Microsoft.Xna.Framework;
using Nez;

namespace ComfyJamSummer.Components.General
{
    public class ShakeComponent : Component, IUpdatable
    {
        private float _timeLeftToEndShake;
        private float _shakeIntensity, _shakeIntensityOriginalValue;
        private Vector2 _shakeOffset;
        private float _shakeDegredation;
        private const float MinShakeIntensity = 1e-4f;


        public ShakeComponent(float shakeIntensity = 1.35f, float shakeDegradation = 0.7f)
        {
            _shakeIntensity = shakeIntensity;
            _shakeDegredation = shakeDegradation;
            _shakeIntensityOriginalValue = _shakeIntensity;
        }

        public void Shake(float shakeTime = 0.3f, float shakeIntensity = 1.35f)
        {
            if (_timeLeftToEndShake > 0)
            {
                return;
            }

            _shakeIntensity = shakeIntensity;
            _shakeIntensityOriginalValue = shakeIntensity;
            _timeLeftToEndShake = shakeTime;
        }

        public void Stop()
        {
            _timeLeftToEndShake = 0;
        }

        public void Update()
        {
            var manager = UtilHelper.GameManager();

            if (manager == null)
            {
                return;
            }

            if (manager.IsGamePaused)
            {
                return;
            }

            if (Entity.IsDestroyed)
            {
                return;
            }

            if (_timeLeftToEndShake > 0)
            {
                var deltaTime = Time.DeltaTime;

                _timeLeftToEndShake -= deltaTime;

                if (Math.Abs(_shakeIntensity) > MinShakeIntensity)
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
                    _shakeIntensity *= 1 - _shakeDegredation; // Degradar a intensidade, mantendo-a positiva
                }
                else
                {
                    _shakeIntensity = _shakeIntensityOriginalValue; // Definir a intensidade como zero quando muito pequena
                    _shakeOffset = Vector2.Zero; // Zerar o offset
                }

                if (float.IsNaN(_shakeOffset.X) || float.IsNaN(_shakeOffset.Y))
                {
                    return;
                }

                var vel = Vector2.Zero;

                vel += _shakeOffset;

                if (float.IsNaN(vel.X) || float.IsNaN(vel.Y))
                {
                    return;
                }

                var mover = Entity.GetComponent<RickMover>();

                if (mover != null)
                {
                    var collisionResult = new CollisionResult();

                    mover.Move(vel, out collisionResult);
                }
                else
                {
                    Entity.Position += vel;
                }
            }
            else
            {
                _shakeIntensity = _shakeIntensityOriginalValue;
            }
        }
    }
}

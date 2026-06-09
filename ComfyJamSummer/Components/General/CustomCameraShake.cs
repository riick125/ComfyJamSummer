using System;
using ComfyJamSummer.Helpers;
using Microsoft.Xna.Framework;
using Nez;

namespace ComfyJamSummer.Components.General
{
    public class CustomCameraShake : Component, IUpdatable
    {
        bool _alreadyShaking = false;
        Vector2 LastPosition;
        Vector2 _shakeDirection;
        Vector2 _shakeOffset;
        float _shakeIntensity = 0f;
        float _shakeDegredation = 0.95f;

        /// <summary>
        /// if the shake is already running this will overwrite the current values only if shakeIntensity > the current shakeIntensity.
        /// if the shake is not currently active it will be started.
        /// </summary>
        /// <param name="shakeIntensity">how much should we shake it</param>
        /// <param name="shakeDegredation">higher values cause faster degradation</param>
        /// <param name="shakeDirection">Vector3.zero will result in a shake on just the x/y axis. any other values will result in the passed
        /// in shakeDirection * intensity being the offset the camera is moved</param>
        public void Shake(float shakeIntensity = 15f, float shakeDegredation = 0.9f,
                          Vector2 shakeDirection = default)
        {
            if (_alreadyShaking) return;

            var gameManager = UtilHelper.GameManager();

            if (gameManager == null)
            {
                return;
            }

            if (gameManager.IsGamePaused)
            {
                return;
            }

            if (!_alreadyShaking)
                LastPosition = Entity.Scene.Camera.Position;

            _alreadyShaking = true;
            Enabled = true;
            if (_shakeIntensity < shakeIntensity)
            {
                _shakeDirection = shakeDirection;
                _shakeIntensity = shakeIntensity;
                if (shakeDegredation < 0f || shakeDegredation >= 1f)
                    shakeDegredation = 0.95f;

                _shakeDegredation = shakeDegredation;
            }
        }

        public void ResetStats()
        {
            _alreadyShaking = false;
            _shakeDirection = default;
            _shakeIntensity = 0;
            LastPosition = Vector2.Zero;
            _shakeOffset = Vector2.Zero;
        }

        public virtual void Update()
        {
            var camera = Entity.Scene.Camera;

            var gameManager = UtilHelper.GameManager();

            if (gameManager == null)
            {
                return;
            }

            if (gameManager.IsGamePaused)
            {
                return;
            }

            if (Math.Abs(_shakeIntensity) > 0f)
            {
                _shakeOffset = _shakeDirection;
                if (_shakeOffset.X != 0f || _shakeOffset.Y != 0f)
                {
                    _shakeOffset.Normalize();
                }
                else
                {
                    _shakeOffset.X = _shakeOffset.X + Nez.Random.NextFloat() - 0.5f;
                    _shakeOffset.Y = _shakeOffset.Y + Nez.Random.NextFloat() - 0.5f;
                }

                //TODO: this needs to be multiplied by camera zoom so that less shake gets applied when zoomed in
                _shakeOffset *= _shakeIntensity;
                _shakeIntensity *= -_shakeDegredation;

                if (LastPosition != Vector2.Zero)
                {
                    var dir = LastPosition - camera.Position;
                    dir.Normalize();

                    if (Math.Abs(_shakeIntensity) <= 0.15f)
                    {
                        //camera.SetPosition(camera.Position += dir * 50 * Time.DeltaTime);
                        // Core.Scene.Camera.ZoomOut(0.1f * Time.DeltaTime);
                        //  Core.Scene.Camera.Zoom = Mathf.Clamp(Core.Scene.Camera.Zoom, GameValues.GameZoom, Core.Scene.Camera.MaximumZoom);
                    }
                    if (Math.Abs(_shakeIntensity) <= 0.01f)
                    {
                        // Core.Scene.Camera.Zoom = GameValues.GameZoom;
                        //camera.SetPosition(LastPosition);
                        _shakeIntensity = 0f;
                        Enabled = false;
                        _alreadyShaking = false;
                    }
                }

            }
            else
            {
                _alreadyShaking = false;
            }

            if (_shakeOffset != default)
            {
                Entity.Scene.Camera.SetPosition(Entity.Scene.Camera.Position + _shakeOffset);
            }
        }
    }
}

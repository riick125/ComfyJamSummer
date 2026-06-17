using ComfyJamSummer.Components.Cutscenes.Base;
using ComfyJamSummer.Entities.Base;
using Microsoft.Xna.Framework;
using Nez;

namespace ComfyJamSummer.Components.Cutscenes
{
    public class ZoomAtTargetCutscene : CutsceneComponent
    {
        protected Camera _camera;
        private float _camApproachActualSpeed;
        private float _camApproachSpeed = 10f;
        private readonly float _camApproachMinSpeed, _camApproachMaxSpeed;
        private float _camAccel;
        private bool _lockedOnTarget;

        private Animated _target;

        private float _zoomOriginalSpeed = 15;
        private float _zoomCurrentSpeed;
        private float _zoomMeterMinValue;
        private float _zoomMeter;
        private float _zoomMeterIncreaseVelocity;

        private bool _isZooming = true;
        private float _startCutsceneDelay = 0.15f;

        private float _delayBetweenZooms = 0.75f, _timeLeftToNextZoom;

        private bool _playedFirstAnimation;

        private string _animationToExecuteAfterZoomIn;
        private string _animationToExecuteAfterZoomOut;

        private bool _waitForAction;

        public ZoomAtTargetCutscene(Animated target, float duration, bool waitForAction = false, string title = "", bool isSkippable = false) : base(duration, title, isSkippable: isSkippable)
        {
            _waitForAction = waitForAction;

            _camApproachActualSpeed = _camApproachSpeed * 0.25f;
            _camAccel = _camApproachSpeed * 0.8f;

            _camApproachMinSpeed = _camApproachActualSpeed;
            _camApproachMaxSpeed = _camApproachSpeed;

            _zoomCurrentSpeed = _zoomOriginalSpeed;
            _zoomMeter = _zoomOriginalSpeed * 0.1f;
            _zoomMeterMinValue = _zoomMeter;
            _zoomMeterIncreaseVelocity = _zoomOriginalSpeed * 0.25f;

            if (Core.Scene != null)
            {
                _camera = Core.Scene.Camera;
            }

            _target = target;
        }

        public void SetStartCutsceneDelay(float value)
        {
            _startCutsceneDelay = value;
        }

        public void SetDelayBetweenZooms(float value)
        {
            _delayBetweenZooms = value;
        }

        public void SetAnimations(string animationNameZoomIn, string animationNameZoomOut)
        {
            if (_target == null)
            {
                return;
            }

            if (string.IsNullOrEmpty(_animationToExecuteAfterZoomIn))
            {
                _animationToExecuteAfterZoomIn = animationNameZoomIn;
            }

            if (string.IsNullOrEmpty(_animationToExecuteAfterZoomOut))
            {
                _animationToExecuteAfterZoomOut = animationNameZoomOut;
            }
        }

        public override void Update()
        {
            base.Update();

            var deltaTime = Time.AltDeltaTime;

            if (!IsRunning)
            {
                return;
            }

            if (_startCutsceneDelay > 0)
            {
                _startCutsceneDelay -= Time.DeltaTime;

                return;
            }

            if (_camera == null || _target == null)
            {
                this.Scene.RemoveSceneComponent(this);
                return;
            }

            if (!_lockedOnTarget)
            {
                _camApproachActualSpeed += _camAccel * deltaTime;

                _camApproachActualSpeed = Mathf.Clamp(_camApproachActualSpeed, _camApproachMinSpeed, _camApproachMaxSpeed);

                var distance = Vector2.Distance(_camera.Position, _target.Position);

                if (float.IsNaN(distance))
                {
                    _camera.SetPosition(_target.Position);
                    _lockedOnTarget = true;
                }
                else
                {
                    if (distance < 10f)
                    {
                        _camera.SetPosition(_target.Position);

                        _lockedOnTarget = true;
                    }
                    else
                    {
                        _camera.Position = Vector2.Lerp(_camera.Position, _target.Position, _camApproachActualSpeed * deltaTime);
                    }
                }
            }
            else
            {
                IncreaseZoomMeter();

                var zoomValue = _camera.Zoom;

                if (_timeLeftToNextZoom <= 0)
                {
                    if (_isZooming)
                    {
                        zoomValue += _zoomMeter * Time.DeltaTime;

                        ApplyCameraZoom(zoomValue);

                        if ((zoomValue >= Game1.GameMaxZoom * 0.8f && zoomValue < (Game1.GameMaxZoom * 0.9f)) && _zoomCurrentSpeed == _zoomOriginalSpeed)
                        {
                            if (!string.IsNullOrEmpty(_animationToExecuteAfterZoomIn))
                            {
                                if (!_playedFirstAnimation)
                                {
                                    if (_target.Animator != null && _target.Animator.Animations.ContainsKey(_animationToExecuteAfterZoomIn))
                                    {
                                        _target.Animator.Play(_animationToExecuteAfterZoomIn);
                                    }

                                    _playedFirstAnimation = true;
                                }
                            }

                            _zoomCurrentSpeed = _zoomOriginalSpeed / 2;
                            _zoomMeter = _zoomCurrentSpeed * 0.1f;
                            _zoomMeterMinValue = _zoomMeter;
                            _zoomMeterIncreaseVelocity = _zoomCurrentSpeed * 0.25f;
                        }
                        else if (zoomValue >= Game1.GameMaxZoom)
                        {
                            _isZooming = false;
                            _zoomCurrentSpeed = _zoomOriginalSpeed;
                            _zoomMeter = _zoomOriginalSpeed * 0.1f;
                            _zoomMeterMinValue = _zoomMeter;
                            _zoomMeterIncreaseVelocity = _zoomOriginalSpeed * 0.25f;
                            _timeLeftToNextZoom = _delayBetweenZooms;
                        }
                    }
                    else
                    {
                        zoomValue -= _zoomMeter * Time.DeltaTime;

                        ApplyCameraZoom(zoomValue);

                        if (zoomValue <= Game1.GameZoom)
                        {
                            if (!string.IsNullOrEmpty(_animationToExecuteAfterZoomOut))
                            {
                                if (_target.Animator != null && _target.Animator.Animations.ContainsKey(_animationToExecuteAfterZoomOut))
                                {
                                    _target.Animator.Play(_animationToExecuteAfterZoomOut);
                                }
                            }

                            var removeCutscene = !_waitForAction;

                            Stop(removeCutscene);
                        }
                    }
                }
                else
                {
                    _timeLeftToNextZoom -= Time.DeltaTime;
                }
            }
        }

        private void IncreaseZoomMeter()
        {
            var deltaTime = Time.DeltaTime;

            _zoomMeter += _zoomMeterIncreaseVelocity * deltaTime;

            _zoomMeter = Mathf.Clamp(_zoomMeter, _zoomMeterMinValue, _zoomCurrentSpeed);
        }

        private float ApplyCameraZoom(float zoomValue)
        {
            var newZoom = Mathf.Clamp(zoomValue, Game1.GameZoom, Game1.GameMaxZoom);

            _camera.Zoom = newZoom;

            return newZoom;
        }
    }
}

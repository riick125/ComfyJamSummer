using ComfyJamSummer.Components.Cutscenes.Base;
using ComfyJamSummer.Components.Gameplay;
using ComfyJamSummer.Components.Visuals;
using ComfyJamSummer.Entities;
using ComfyJamSummer.Entities.General;
using ComfyJamSummer.Enums;
using ComfyJamSummer.Helpers;
using Microsoft.Xna.Framework;
using Nez;
using System.Collections.Generic;

namespace ComfyJamSummer.Components.Cutscenes
{
    public class EscapeCutscene : CutsceneComponent
    {
        CutscenePartManager _manager;

        List<CutscenePart> _parts;

        Player _player;
        Rocket _rocket;
        Crab _crab;

        float _speed = 20;
        float _rocketSpeed = 500;
        CustomAccelerator _accelRocket;

        float _camShake = 0.048f, _maxCamShake = 7f;
        float _shakeIncreaseValue = 0.35f;

        public EscapeCutscene(float duration, string title = "", float executionTime = 1.75F, float startDelay = 0.25F, bool isSkippable = false) : base(duration, title, executionTime, startDelay, isSkippable)
        {
            _parts = new List<CutscenePart>();

            _parts.Add(new CutscenePart(EscapeCutscenePart.Entering));
            _parts.Add(new CutscenePart(EscapeCutscenePart.CameraShake, 2));//6.5f));
            _parts.Add(new CutscenePart(EscapeCutscenePart.Flying, 8));
            _parts.Add(new CutscenePart(EscapeCutscenePart.TheEnd));

            _manager = new CutscenePartManager(_parts, true);

            _accelRocket = new CustomAccelerator(_rocketSpeed);

            _accelRocket.SetAccelInitialValuePercentage(0.0005f);
            _accelRocket.SetAccelValuePercentage(0.05f);
        }

        public override void OnEnabled()
        {
            base.OnEnabled();

            _player = UtilHelper.Player();

            _crab = UtilHelper.Crab();

            _rocket = UtilHelper.GetEntity<Rocket>();

            if (_player == null || _crab == null || _rocket == null)
            {
                this.Scene.RemoveSceneComponent(this);
                this.Scene.DestroyAllEntities();

                return;
            }

            _player.RemoveComponent<PlayerController>();
            _crab.RemoveComponent<CrabController>();

            _player.GetComponents<Collider>().ForEach(x => x.RemoveComponent());
            _rocket.GetComponents<Collider>().ForEach(x => x.RemoveComponent());
            _crab.GetComponents<Collider>().ForEach(x => x.RemoveComponent());

            if (Scene.Camera != null)
            {
                Scene.Camera.RemoveComponent<FollowCamera>();
            }
        }

        public override void Update()
        {
            base.Update();

            if (_player == null || _crab == null || _rocket == null)
            {
                this.Scene.RemoveSceneComponent(this);
                this.Scene.DestroyAllEntities();

                return;
            }

            if (Scene.Camera == null)
            {
                return;
            }

            if (_manager == null)
            {
                return;
            }

            if (_manager.Entity != null && _manager.Entity.IsDestroyed)
            {
                this.Scene.RemoveSceneComponent(this);

                return;
            }

            var part = _manager.ActualPart;

            if (part == null)
            {
                return;
            }

            var cameraShake = Scene.Camera.GetComponent<CameraShake>();

            var deltaTime = Time.AltDeltaTime;

            switch (part.GetPartName<EscapeCutscenePart>())
            {
                case EscapeCutscenePart.Entering:
                    if (!Scene.Camera.HasComponent<FollowCamera>())
                    {
                        Scene.Camera.AddComponent(new FollowCamera(_rocket, Scene.Camera, deadzoneMeasurement: FollowCamera.Measurement.ScaledCameraBounds));
                    }

                    AnimHelper.Play(_player.Animator, CreatureAnim.Walk);
                    AnimHelper.Play(_crab.Animator, CreatureAnim.Walk);

                    var directionPlayer = _rocket.Position - _player.Position;
                    directionPlayer.Normalize();

                    var directionCrab = _rocket.Position - _crab.Position;
                    directionCrab.Normalize();

                    if (DirectionHelper.Validate(directionPlayer) && DirectionHelper.Validate(directionCrab))
                    {
                        if (_player.Enabled)
                            _player.Position += directionPlayer * _speed * deltaTime;

                        if (_crab.Enabled)
                            _crab.Position += directionCrab * _speed * deltaTime;
                    }

                    if (Vector2.Distance(_player.Position, _rocket.Position) <= _player.SpriteWidth / 8)
                    {
                        _player.SetEnabled(false);

                        if (_player.Shadow != null)
                        {
                            _player.Shadow.Enabled = false;
                        }
                    }

                    if (Vector2.Distance(_crab.Position, _rocket.Position) <= _crab.SpriteWidth / 8)
                    {
                        _crab.SetEnabled(false);

                        if (_crab.Shadow != null)
                        {
                            _crab.Shadow.Enabled = false;
                        }
                    }

                    if (!_player.Enabled && !_crab.Enabled)
                    {
                        part.Finish();
                    }
                    break;

                case EscapeCutscenePart.CameraShake:
                    AnimHelper.Play(_rocket.Animator, RocketAnim.Flying);

                    _camShake += _shakeIncreaseValue * deltaTime;
                    _camShake = Mathf.Clamp(_camShake, 0, _maxCamShake);

                    if (cameraShake != null)
                    {
                        cameraShake.Shake(_camShake);
                    }
                    break;

                case EscapeCutscenePart.Flying:
                    if (_rocket.Shadow != null)
                    {
                        var fakeShadow = _rocket.GetComponent<FakeShadowComponent>();

                        if (fakeShadow != null)
                        {
                            fakeShadow.Enabled = false;
                        }

                        if ((part.TimeElapsed < part.Duration * 0.55f) && cameraShake != null)
                        {
                            cameraShake.Shake(_camShake);
                        }

                        var scale = _rocket.Shadow.Scale.X;

                        var scaleReduceValue = _rocket.Shadow.Scale.X * 0.35f;

                        scale -= scaleReduceValue * deltaTime;

                        scale = Mathf.Clamp01(scale);

                        _rocket.Shadow.SetScale(scale);
                    }

                    _accelRocket.Process(true);

                    var vel = Vector2.Zero;

                    vel.Y += _accelRocket.Accel * deltaTime;

                    _rocket.Position -= vel;
                    break;

                case EscapeCutscenePart.TheEnd:

                    if (cameraShake != null)
                    {
                        cameraShake.RemoveComponent();
                    }

                    if (Scene.Camera != null)
                    {
                        Scene.Camera.RemoveComponent<FollowCamera>();
                    }
                    break;

                default:
                    break;
            }
        }
    }

    public enum EscapeCutscenePart
    {
        Entering,
        CameraShake,
        Flying,
        TheEnd
    }
}
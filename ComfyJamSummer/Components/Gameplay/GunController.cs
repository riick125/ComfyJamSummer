using ComfyJamSummer.Entities;
using ComfyJamSummer.Entities.Creatures;
using ComfyJamSummer.Enums;
using ComfyJamSummer.Helpers;
using ComfyJamSummer.Manager;
using ComfyJamSummer.Prefab;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Nez;
using System;

namespace ComfyJamSummer.Components.Gameplay
{
    public class GunController : BaseComponent, IUpdatable
    {
        Gun _gun;
        Player _player;
        Vector2 _lastPos;
        float _gunRotationSpeed = 1200;

        public GunController(GameManager manager, Prefabs prefabs) : base(manager, prefabs)
        {
        }

        public override void OnAddedToEntity()
        {
            base.OnAddedToEntity();

            _gun = this.Entity as Gun;

            _player = _gun.Player;
        }

        public void Update()
        {
            var deltaTime = Time.DeltaTime;

            _gun.Animator.SetEnabled(_player.IsAlive);

            if (!Validate())
            {
                return;
            }

            var rendererCreature = _player.GetAnyRenderer();
            var rendererGun = _gun.GetAnyRenderer();

            var offsetX = rendererCreature.FlipX ? _gun.Offset.X * -1 : _gun.Offset.X;
            float circleRadius = rendererCreature.FlipX ? -10 : 10;
            float circleSpeed = 2.0f;

            var angle = (deltaTime * circleSpeed);

            var gunPosition = new Vector2(
                _player.Position.X + circleRadius * (float)Math.Cos(angle),
                _player.Position.Y + circleRadius * (float)Math.Sin(angle)
            );

            var mousePos = Core.Scene.Camera.ScreenToWorldPoint(Input.RawMousePosition);

            var aimPos = mousePos;

            var direction = (aimPos - _gun.Transform.Position);

            var angle2 = (float)Math.Atan2(direction.Y, direction.X);

            if (!_gun.IsReloading)
            {
                SetPosition(_gun, offsetX, gunPosition, aimPos, angle2);

                if (Input.IsKeyPressed(Keys.R) && _gun.ActualAmmo < _gun.MagSize)
                {
                    StartReload();
                }
            }
            else
            {
                _gun.Position = new Vector2(_player.Position.X + circleRadius, _player.Position.Y) + new Vector2(0, _player.SpriteHeight / 8);

                _gun.RotationDegrees += _gunRotationSpeed * deltaTime;

                Reload();
            }

            if (_gun.TimeLeftToEndShootAnimation > 0)
            {
                _gun.TimeLeftToEndShootAnimation -= deltaTime;
            }
            else
            {
                _gun.Animator.Speed = 1;
                AnimHelper.Play(_gun.Animator, SmgAnim.Idle);
            }

            ProcessShoot(direction, angle2);
        }

        void Reload()
        {
            var gun = this.Entity as Gun;

            if (gun == null)
            {
                return;
            }

            var prefabs = gun.Prefabs;

            if (prefabs == null)
            {
                return;
            }

            gun.TimeLeftToEndReload -= Time.DeltaTime;

            if (gun.TimeLeftToEndReload <= 0)
            {
                prefabs.StopSound(SoundFxName.Reloading);

                gun.ActualAmmo += gun.MagSize;
                gun.IsReloading = false;

                gun.ActualAmmo = Math.Clamp(gun.ActualAmmo, 0, gun.MagSize);
            }
        }

        void SetPosition(Gun gun, float offsetX, Vector2 gunPosition, Vector2 aimPosition, float angle)
        {
            gun.Position = gunPosition + new Vector2(offsetX, gun.Offset.Y);
            gun.Transform.Rotation = angle;

            gun.GetAnyRenderer().FlipY = aimPosition.X < _player.Position.X;

            if (_lastPos != gun.Position)
            {
                _lastPos = gun.Position;
            }
        }

        void StartReload()
        {
            _prefabs.PlaySoundRandomPitch(SoundFxName.Reloading, 0.13f);
            _gun.TimeLeftToEndReload = _gun.ReloadTime;
            _gun.IsReloading = true;
        }

        void ProcessShoot(Vector2 direction, float angle)
        {
            var deltaTime = Time.DeltaTime;

            if (_manager.TimeLeftToEndGameStartDelay <= 0)
            {
                if (Input.LeftMouseButtonDown)
                {
                    if (_gun.TimeLeftToNextShot <= 0 && !_gun.IsReloading && _player.IsAlive)
                    {
                        if (_gun.ActualAmmo > 0)
                        {
                            _gun.ActualAngleSpread += _gun.MaxAngleSpread / 3.25f;

                            _gun.ActualAngleSpread = Mathf.Clamp(_gun.ActualAngleSpread, _gun.MinAngleSpread, _gun.MaxAngleSpread);

                            _gun.LittleShake.Shake();

                            _prefabs.PlaySoundRandomPitch(SoundFxName.Smg_Shot, 0.075f);

                            _gun.Animator.Speed = 4;
                            AnimHelper.Play(_gun.Animator, SmgAnim.Shoot);

                            _gun.TimeLeftToEndShootAnimation = _gun.ShootAnimationDuration;

                            direction.Normalize();

                            var shootOffset = _gun.SpriteWidth / 3;

                            var muzzleOffset = new Vector2(
                                (float)Math.Cos(angle) * shootOffset,
                                (float)Math.Sin(angle) * shootOffset
                            );

                            _gun.MuzzlePosition = (_gun.Position - new Vector2(0, 1.5f)) + muzzleOffset;

                            _gun.PointingAngle = angle;

                            BulletHelper.Create(_player, direction);

                            _gun.ActualAmmo--;

                            if (_gun.ActualAmmo <= 0)
                            {
                                StartReload();
                            }

                        }
                        else
                        {
                            StartReload();
                        }

                        _gun.TimeLeftToNextShot = _gun.FireRate;
                    }
                }
                else
                {
                    _gun.ActualAngleSpread -= _gun.MaxAngleSpread / 8 * (deltaTime / 1.2f);

                    _gun.ActualAngleSpread = Mathf.Clamp(_gun.ActualAngleSpread, _gun.MinAngleSpread, _gun.MaxAngleSpread);
                }

                if (_gun.TimeLeftToNextShot > 0)
                {
                    _gun.TimeLeftToNextShot -= deltaTime;
                }
            }
        }

        bool Validate()
        {
            if (this.Entity == null)
            {
                return false;
            }

            if (this.Entity.IsDestroyed)
            {
                return false;
            }

            if (_gun.Animator == null && _gun.Renderer == null)
            {
                return false;
            }

            if (_manager == null)
            {
                return false;
            }

            if (_manager.CantDoAnyAction)
            {
                return false;
            }

            if (!_player.IsAlive)
            {
                return false;
            }

            if (_prefabs == null)
            {
                return false;
            }

            return true;
        }
    }
}
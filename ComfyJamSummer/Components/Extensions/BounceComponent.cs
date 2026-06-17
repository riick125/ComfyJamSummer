using ComfyJamSummer.Components.General;
using ComfyJamSummer.Entities;
using ComfyJamSummer.Entities.Base;
using ComfyJamSummer.Entities.General;
using ComfyJamSummer.Helpers;
using Microsoft.Xna.Framework;
using MonoGame.Extended.ECS;
using Nez;
using System.Linq;

namespace ComfyJamSummer.Components.Extensions
{
    public class BounceComponent : Component, IUpdatable
    {
        Island _island;

        private uint _islandId;

        private Vector2 _velocity;

        private float _gravity = 70;
        readonly float _gravityOriginalValue, _gravityMaxValue;
        private float _gravityShadow;
        private float _maxDistanceOfShadow = 20f;
        CustomAccelerator _bounceAccel;
        private int _bounceCounter, _bounceQuantity;
        private float _bounceDelay = 0.05f, _timeLeftToNextBounce;
        private bool _isOnFloor, _isOutSideOfMap = true;

        CustomAccelerator _goingUpAccel;

        public bool IsOnFloor { get { return this._isOnFloor; } }

        private float _rotationVel = 750f;
        private bool _resetRotation;

        private float _safeDistanceY = 0.1f;

        private InteractableObject _entity;
        private RickMover _entityMover;

        private Shadow _shadow;
        private RickMover _shadowMover;

        public BounceComponent(uint islandId, Vector2 velocity, int bounceQuantity, float rotationVel = 750f, bool isHeavy = false, bool resetRotation = true)
        {
            try
            {
                _islandId = islandId;

                if (isHeavy)
                {
                    _gravity = _gravity * 1.5f;
                }

                _goingUpAccel = new CustomAccelerator(700, Nez.Random.Range(0.23f, 0.25f));

                _bounceAccel = new CustomAccelerator(150);

                _gravityOriginalValue = _gravity;
                _gravityMaxValue = _gravity * 15;

                _resetRotation = resetRotation;

                _gravityShadow = _gravity * Nez.Random.Range(0.055f, 0.07f);

                _velocity = velocity;

                _bounceQuantity = bounceQuantity;
            }
            catch (System.Exception ex)
            {
            }
        }

        public override void OnAddedToEntity()
        {
            base.OnAddedToEntity();

            _island = Entity.Scene.EntitiesOfType<Island>().FirstOrDefault(x => x.Id == _islandId);

            if (this.Entity == null)
            {
                this.RemoveComponent();
                return;
            }

            var typeClass = this.Entity.GetType();

            if (typeClass != typeof(InteractableObject) && !typeClass.IsSubclassOf(typeof(InteractableObject)))
            {
                this.RemoveComponent();
                return;
            }

            _entity = this.Entity as InteractableObject;

            _entity.AddComponent<RickMover>();

            if (_entity.Shadow == null)
            {
                this.RemoveComponent();
                return;
            }

            _shadow = _entity.Shadow;

            _shadow.AddComponent<RickMover>();
        }

        public void Process()
        {
            if (_entity == null)
            {
                return;
            }

            if (_entity.IsDestroyed)
            {
                return;
            }

            if (Game1.GameManager != null)
            {
                if (Game1.GameManager.CantDoAnyAction)
                {
                    return;
                }
            }

            var deltaTime = Time.DeltaTime;

            var island = UtilHelper.GetEntity<Island>();

            if (island == null)
            {
                this.RemoveComponent();
                this.Entity.Destroy();

                return;
            }

            if (_entityMover == null)
            {
                _entityMover = _entity.GetComponent<RickMover>();
            }

            if (_shadowMover == null)
            {
                _shadowMover = _shadow.GetComponent<RickMover>();
            }

            if (_isOutSideOfMap)
            {
                _gravity += (_gravity * 1.25f) * deltaTime;
                _gravity = Mathf.Clamp(_gravity, _gravityOriginalValue, _gravityMaxValue);

                _entity.RotationDegrees += _rotationVel * deltaTime;

                var isGoingUp = _entity.Position.X < _island.Position.X || _entity.Position.X > _island.MaxPosition.X;

                if (isGoingUp)
                {
                    var result = _goingUpAccel.Process();
                    if (result)
                    {
                        var velBounce = Vector2.Zero;

                        velBounce.Y -= _goingUpAccel.Accel * deltaTime;

                        _entity.Position += velBounce;
                    }
                }

                if (_entity.FallDestination != default)
                {
                    var directionShadow = _shadow.Position - _entity.Position;
                    directionShadow.Normalize();

                    var vel = Vector2.Zero;

                    if (Vector2.Distance(_entity.Shadow.Position, _entity.Position) < _entity.SpriteWidth)
                    {
                        _gravity = _gravityOriginalValue;
                        _isOutSideOfMap = false;
                    }
                    else
                    {
                        if (DirectionHelper.ValidateVelocity(directionShadow, vel))
                        {
                            var posEntityX = _entity.Position.X;
                            var posEntityY = _entity.Position.Y;

                            directionShadow.X = _entity.FallDirection.X;

                            vel.X += directionShadow.X * (isGoingUp ? _gravity * 1.5f : _gravity * 0.75f) * deltaTime;
                            vel.Y += directionShadow.Y * _gravity * deltaTime;
                            _entity.Position += vel;
                        }
                        else
                        {
                            _gravity = _gravityOriginalValue;
                            _isOutSideOfMap = false;
                        }
                    }
                }
                else
                {
                    _gravity = _gravityOriginalValue;
                    _isOutSideOfMap = false;
                }
            }
            else
            {
                if (!_isOnFloor)
                {
                    _entity.RotationDegrees += _rotationVel * deltaTime;

                    var collisionResult = new CollisionResult();

                    var collisionResultShadow = new CollisionResult();

                    var velEntity = Vector2.Zero;

                    var velShadow = Vector2.Zero;

                    velEntity += _velocity * deltaTime;

                    velEntity.X *= _entity.FallDirection.X > 0 ? 1 : -1;

                    velEntity.Y += _gravity * deltaTime;

                    velShadow.Y += _gravityShadow * deltaTime;

                    if (_timeLeftToNextBounce <= 0)
                        _entityMover.Move(velEntity, out collisionResult);

                    var posEntityX = _entity.Position.X;
                    var posEntityY = _entity.Position.Y;

                    posEntityX = Mathf.Clamp(posEntityX, island.MinPosition.X + _entity.SpriteWidth, island.MaxPosition.X - _entity.SpriteWidth);

                    posEntityY = Mathf.Clamp(posEntityY, _shadow.Position.Y - _maxDistanceOfShadow, _shadow.Position.Y);

                    _entity.SetPosition(new Vector2(posEntityX, posEntityY));

                    _shadowMover.Move(velShadow, out collisionResultShadow);

                    var posYShadow = _shadow.Position.Y;

                    posYShadow = Mathf.Clamp(posYShadow, island.MinPosition.Y, island.MaxPosition.Y - _entity.SpriteHeight);

                    _shadow.SetPosition(_entity.Position.X, posYShadow);

                    if (_entity.Position.Y + _safeDistanceY >= _shadow.Position.Y)
                    {
                        if (_timeLeftToNextBounce <= 0)
                        {
                            if (_entity.CrazyScaleComponent != null)
                            {
                                _entity.CrazyScaleComponent.Squeeze();
                            }

                            if (_bounceCounter < _bounceQuantity)
                            {
                                _bounceAccel.SetForce(_bounceAccel.Force * 0.94f);

                                _bounceCounter++;
                            }
                            else
                            {
                                _isOnFloor = true;
                            }

                            _timeLeftToNextBounce = _bounceDelay;
                        }
                    }
                    else
                    {
                        if (_timeLeftToNextBounce > 0)
                        {
                            _bounceAccel.Process();

                            var bounceVel = new Vector2(0, _bounceAccel.Accel * deltaTime);

                            _entityMover.Move(-bounceVel, out collisionResult);
                            _timeLeftToNextBounce -= deltaTime;
                        }
                    }
                }
                else
                {
                    if (_resetRotation)
                        _entity.RotationDegrees = 0;

                    _entity.Position = _shadow.Position;

                    Enabled = false;
                }
            }
        }

        public void Update()
        {
            if (!Enabled)
            {
                return;
            }

            Process();
        }
    }
}
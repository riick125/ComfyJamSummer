using ComfyJamSummer.Components.General;
using ComfyJamSummer.Entities;
using ComfyJamSummer.Entities.Base;
using ComfyJamSummer.Helpers;
using Microsoft.Xna.Framework;
using Nez;
using System.Linq;

namespace ComfyJamSummer.Components.Extensions
{
    public class BounceComponent : Component, IUpdatable
    {
        Island _island;

        private uint _islandId;

        private Vector2 _velocity;

        private float _gravity = 53;
        private float _gravityShadow;
        private float _maxDistanceOfShadow = 20f;
        private float _bounceForce = 308;
        private int _bounceCounter, _bounceQuantity;
        private float _bounceDelay = 0.002f, _timeLeftToNextBounce;
        private bool _isOnFloor, _isOutSideOfMap = true;

        public bool IsOnFloor { get { return this._isOnFloor; } }

        private float _rotationVel = 750f;
        private bool _resetRotation;

        private float _safeDistanceY = 0.1f;

        private Animated _entity;
        private RickMover _entityMover;

        private Shadow _shadow;
        private RickMover _shadowMover;

        public BounceComponent(Animated entity, uint islandId, Vector2 velocity, int bounceQuantity, float rotationVel = 750f, bool isHeavy = false, bool resetRotation = true)
        {
            try
            {
                _entity = entity;

                _islandId = islandId;

                _entity.AddComponent<RickMover>();

                if (_entity.Shadow == null)
                {
                    return;
                }

                if (isHeavy)
                {
                    _gravity = _gravity * 1.5f;
                }

                _resetRotation = resetRotation;

                _gravityShadow = _gravity * Nez.Random.Range(0.055f, 0.07f);
                _shadow = _entity.Shadow;

                _shadow.AddComponent<RickMover>();

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
        }

        public void Process()
        {
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
                _entity.RotationDegrees += _rotationVel * deltaTime;


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

                    velEntity.Y += _gravity * deltaTime;

                    velShadow.Y += _gravityShadow * deltaTime;

                    _entityMover.Move(velEntity, out collisionResult);

                    var posXEntity = _entity.Position.X;
                    var posYEntity = _entity.Position.Y;

                    //posXEntity = Mathf.Clamp(posXEntity, island.MinPositionX + _entity.SpriteWidth, island.MaxPositionX - _entity.SpriteWidth);

                    posYEntity = Mathf.Clamp(posYEntity, _shadow.Position.Y - _maxDistanceOfShadow, _shadow.Position.Y);

                    _entity.SetPosition(new Vector2(posXEntity, posYEntity));

                    _shadowMover.Move(velShadow, out collisionResultShadow);

                    var posYShadow = _shadow.Position.Y;

                    //posYShadow = Mathf.Clamp(posYShadow, island.MinPositionY, island.MaxPositionY - _entity.SpriteHeight);

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
                                var bounceVel = new Vector2(0, _bounceForce * deltaTime);

                                _entityMover.Move(-bounceVel, out collisionResult);

                                _bounceForce *= 0.94f;

                                _bounceCounter++;
                            }
                            else
                            {
                                _isOnFloor = true;
                            }

                            _timeLeftToNextBounce = _bounceDelay;
                        }
                        else
                        {
                            _timeLeftToNextBounce -= deltaTime;
                        }
                    }
                }
                else
                {
                    if (_resetRotation)
                        _entity.RotationDegrees = 0;

                    _entity.Position = _shadow.Position;

                    this.RemoveComponent();
                }
            }
        }

        public void Update()
        {
            Process();
        }
    }
}
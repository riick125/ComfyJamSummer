using ComfyJamSummer.Entities.Creatures;
using ComfyJamSummer.Helpers;
using Microsoft.Xna.Framework;
using Nez;

namespace ComfyJamSummer.Components.Visuals
{
    public class Inflate : Component, IUpdatable
    {
        private readonly float _minScale = 1f, _maxScale = 1.3f;

        private float _accel, _accelGrowValue;

        private bool _isInflating = true;

        private bool _isActive;

        public bool IsActive { get { return _isActive; } }

        public Inflate(float growValue = 8f)
        {
            growValue = Mathf.Clamp(growValue, 5f, 10f);

            _accelGrowValue = growValue;

            _accel = _accelGrowValue;
        }

        public void Activate()
        {
            if (_isActive)
            {
                return;
            }

            _isActive = true;
            _isInflating = true;
        }

        public void Update()
        {
            if (!_isActive)
            {
                return;
            }

            var gameManager = UtilHelper.GameManager();

            if (gameManager == null)
            {
                return;
            }

            if (gameManager.CantDoAnyAction)
            {
                return;
            }

            var creature = this.Entity as Creature;

            if (!creature.IsAlive)
            {
                _isActive = false;

                return;
            }

            if (creature.CrazyScaleComponent != null && creature.CrazyScaleComponent.IsActive)
            {
                _isActive = false;

                return;
            }

            var deltaTime = Time.DeltaTime;

            var scale = this.Entity.Scale;

            var vel = Vector2.Zero;

            _accel += _accelGrowValue * deltaTime;

            _accel = Mathf.Clamp(_accel, _accelGrowValue, _maxScale);

            vel += new Vector2(_accel) * deltaTime;

            if (_isInflating)
            {
                scale += vel;

                if (scale.X >= _maxScale)
                {
                    _isInflating = false;
                }
            }
            else
            {
                if (scale.X > _minScale)
                {
                    scale -= vel;
                }
                else
                {
                    _isActive = false;
                }
            }

            scale.X = Mathf.Clamp(scale.X, _minScale, _maxScale);
            scale.Y = Mathf.Clamp(scale.Y, _minScale, _maxScale);

            this.Entity.SetScale(scale);
        }
    }
}
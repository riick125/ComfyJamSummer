using ComfyJamSummer.Entities;
using ComfyJamSummer.Entities.Base;
using ComfyJamSummer.Helpers;
using Microsoft.Xna.Framework;
using Nez;
using Nez.Sprites;

namespace ComfyJamSummer.Components.Visuals
{
    public class FakeShadowComponent : Component, IUpdatable
    {
        Island _island;

        SpriteRenderer _shadowRenderer;

        private int _radius;
        private float _offsetY;
        private float _offsetX;

        private float _timeLeftToUpdateScale, _updateScaleCooldown = 0.05f;

        private float _originalScale;

        public bool FollowPositionY;

        private Color _color;
        private float _alpha = 0.38f;

        Vector2 _position;

        public void SetOffsetY(float amount)
        {
            _offsetY = amount;
        }

        public FakeShadowComponent(int radius, float offsetY, bool followYPosition = true, float offsetX = 0, Color color = default, float alpha = 0)
        {
            if (_color == default)
            {
                _color = Constants.BG_COLOR;
            }

            if (alpha <= 0)
            {
                alpha = _alpha;
            }

            _alpha = alpha;

            _radius = radius;
            _offsetY = offsetY;

            FollowPositionY = followYPosition;
            _offsetX = offsetX;
            _originalScale = 1f;
        }

        public override void OnAddedToEntity()
        {
            base.OnAddedToEntity();

            var entity = Entity as Animated;

            if (entity.Animator == null && entity.Renderer == null)
            {
                this.RemoveComponent(this);
                return;
            }

            _island = UtilHelper.GetEntity<Island>();

            entity.Shadow = UtilHelper.Prefabs().Shadow.Clone() as Shadow;
            entity.Shadow.OffsetY = _offsetY;

            if (!FollowPositionY)
            {
                entity.Shadow.Position = new Vector2(entity.Position.X, entity.Position.Y + _offsetY);
            }

            var texture = UtilHelper.CreatePixelCircle(Core.GraphicsDevice, _radius, _color, _alpha);
            _shadowRenderer = entity.Shadow.AddComponent(new SpriteRenderer(texture));

            var renderLayer = entity.Animator != null ? entity.Animator.RenderLayer : entity.Renderer.RenderLayer;

            _shadowRenderer.SetRenderLayer(renderLayer + 1);

            Core.Scene.AddEntity(entity.Shadow);
        }

        public void Update()
        {
            if (this.Entity == null)
            {
                return;
            }

            var entity = Entity as Animated;

            if (entity == null)
            {
                return;
            }

            if (entity.Shadow == null)
            {
                this.RemoveComponent(this);
                return;
            }

            if (_island == null)
            {
                this.RemoveComponent();
                return;
            }

            var position = new Vector2(entity.Position.X, FollowPositionY ? entity.Position.Y + _offsetY : entity.Shadow.Position.Y);

            position.Y = Mathf.Clamp(position.Y, _island.MinPosition.Y, _island.MaxPosition.Y - (_shadowRenderer.Height * 0.75f));

            entity.Shadow.SetPosition(position);

            if (_timeLeftToUpdateScale <= 0)
            {
                entity.Shadow.SetScale(new Vector2(_originalScale + (Nez.Random.Range(0, 0.05f) * Nez.Random.MinusOneToOne()), _originalScale + (Nez.Random.Range(0, 0.05f) * Nez.Random.MinusOneToOne())));

                _timeLeftToUpdateScale = _updateScaleCooldown;
            }
            else
            {
                _timeLeftToUpdateScale -= Time.DeltaTime;
            }
        }
    }
}
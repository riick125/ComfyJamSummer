using ComfyJamSummer.Components.Gameplay;
using ComfyJamSummer.Entities;
using ComfyJamSummer.Manager;
using ComfyJamSummer.Prefab;
using Microsoft.Xna.Framework.Input;
using Nez;

namespace ComfyJamSummer.Components.Extensions
{
    public class DebugComponent : BaseComponent, IUpdatable
    {
        float _zoomSpeed;
        float _minZoom;

        public DebugComponent(GameManager manager, Prefabs prefabs) : base(manager, prefabs)
        {
            _zoomSpeed = 2f;

            _minZoom = Constants.MIN_GAME_ZOOM - 1f;
        }

        public void Update()
        {
#if !DEBUG
    return;
#endif
            if (!Validate(this.Entity))
                return;

            var deltaTime = Time.DeltaTime;

            var zoomValue = _zoomSpeed * deltaTime;

            if (Input.IsKeyDown(Keys.Down))
            {
                _camera.ZoomOut(zoomValue);
            }
            else if (Input.IsKeyDown(Keys.Up))
            {
                _camera.ZoomIn(zoomValue);
            }

            _camera.Zoom = Mathf.Clamp(_camera.Zoom, _minZoom, Game1.GameMaxZoom);

            if (Input.IsKeyPressed(Keys.Enter))
            {
                Core.Scene.EntitiesOfType<Enemy>().ForEach(x => x.TakeDamage(x.MaxHP));
            }
        }
    }
}
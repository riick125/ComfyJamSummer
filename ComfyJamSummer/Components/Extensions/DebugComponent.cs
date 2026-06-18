using ComfyJamSummer.Components.Gameplay;
using ComfyJamSummer.Entities;
using ComfyJamSummer.Entities.Base;
using ComfyJamSummer.Helpers;
using ComfyJamSummer.Manager;
using ComfyJamSummer.Prefab;
using ComfyJamSummer.Scenes;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended.Collisions.Layers;
using Nez;
using Nez.Sprites;
using System.Linq;
using System.Runtime.CompilerServices;

namespace ComfyJamSummer.Components.Extensions
{
    public class DebugComponent : BaseComponent, IUpdatable
    {
        float _zoomSpeed;
        float _minZoom;

        Player _player;

        Island _island;

        public DebugComponent(GameManager manager, Prefabs prefabs) : base(manager, prefabs)
        {
            _zoomSpeed = 2f;

            _minZoom = Constants.MIN_GAME_ZOOM - 1f;
        }

        public override void OnAddedToEntity()
        {
            base.OnAddedToEntity();

            _player = UtilHelper.Player();

            _island = UtilHelper.GetEntity<Island>();
        }

        public void Update()
        {
#if !DEBUG
    return;
#endif
            if (!Validate(this.Entity))
                return;

            var entities = _scene.EntitiesOfType<Entity>().Where(x => x.HasComponent<SpriteRenderer>() && !x.GetComponent<SpriteRenderer>().DebugRenderEnabled).Select(x => x.GetComponent<SpriteRenderer>());

            foreach (var item in entities)
            {
                item.DebugRenderEnabled = false;
            }

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
                //Core.Scene.EntitiesOfType<Enemy>().ForEach(x => x.TakeDamage(x.MaxHP));

                //var fallDestination = _player.Position + new Vector2(_player.SpriteWidth * Nez.Random.MinusOneToOne(), _player.SpriteHeight * 1.35f);

                //var brandNewDeliciousSandwich = _scene.AddEntity(_prefabs.GetCollectible(_prefabs.InteractableConfig
                //    .CloneNpc(_island.Id, _player.Position), Enums.CollectibleType.Sandwich, fallDestination));
            }

            if (!_player.IsAlive)
            {
                if (Input.IsKeyPressed(Keys.R))
                {
                    Core.StartSceneTransition<FadeTransition>(new FadeTransition(() => new InGameScene()));
                }
            }
        }
    }
}
using ComfyJamSummer.Entities;
using ComfyJamSummer.Enums;
using ComfyJamSummer.Helpers;
using ComfyJamSummer.Manager;
using ComfyJamSummer.Prefab;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Nez;
using System.Linq;

namespace ComfyJamSummer.Components.Gameplay
{
    public class PlayerController : BaseComponent, IUpdatable
    {
        Player _player;

        readonly Keys[] movingKeys = new Keys[] { Keys.W, Keys.A, Keys.S, Keys.D };

        public PlayerController(GameManager manager, Prefabs prefabs) : base(manager, prefabs)
        {
        }

        public override void OnAddedToEntity()
        {
            base.OnAddedToEntity();

            _player = Entity as Player;
        }

        public void Update()
        {
            if (!Validate(_player))
                return;

            Move();

            if (_player.Animator != null)
            {
                _player.Animator.FlipX = Core.Scene.Camera.MouseToWorldPoint().X < _player.Position.X;
            }

            FeedCrab();

            Escape();
        }

        void FeedCrab()
        {
            var crab = UtilHelper.Crab();

            if (crab == null)
            {
                return;
            }

            if (!crab.IsAlive || crab.TalkAreaCollider == null || crab.IsInteracting)
            {
                return;
            }

            if (_player.Sandwich == null)
            {
                return;
            }

            if (_player.Sandwich.IsDestroyed)
            {
                return;
            }

            if (crab.CanPressInteractButton && Input.IsKeyPressed(Keys.E))
            {
                crab.EatSandwich();

                _player.Sandwich.Destroy();
                _player.Sandwich = null;
            }
        }

        void Escape()
        {
            var rocket = UtilHelper.GetEntity<Rocket>();

            if (rocket == null)
            {
                return;
            }

            rocket.Launch(_player);
        }

        void Move()
        {
            var vel = Vector2.Zero;

            var direction = Vector2.Zero;

            var pressedKeys = Input.CurrentKeyboardState.GetPressedKeys().Where(x => movingKeys.Contains(x)).ToArray();

            for (int i = 0; i < pressedKeys.Length; i++)
            {
                var key = pressedKeys[i];

                switch (key)
                {
                    case Keys.A:
                        direction.X = -1;
                        break;

                    case Keys.D:
                        direction.X = 1;
                        break;

                    case Keys.S:
                        direction.Y = 1;
                        break;

                    case Keys.W:
                        direction.Y = -1;
                        break;
                }
            }

            AnimHelper.Play(_player?.Animator, direction == default ? CreatureAnim.Idle : CreatureAnim.Walk);

            vel += direction * _player.Speed * Time.DeltaTime;

            if (DirectionHelper.ValidateVelocity(direction, vel))
            {
                CollisionResult collisionResult;

                _player.Mover.Move(vel, out collisionResult);
            }
        }
    }
}
using ComfyJamSummer.Entities;
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

            //_camera.SetPosition(_player.Position);
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

            vel += direction * _player.Speed * Time.DeltaTime;

            if (DirectionHelper.ValidateVelocity(direction, vel))
            {
                CollisionResult collisionResult;

                _player.Mover.Move(vel, out collisionResult);
            }
        }
    }
}
using ComfyJamSummer.Entities;
using ComfyJamSummer.Helpers;
using ComfyJamSummer.Manager;
using ComfyJamSummer.Prefab;
using Microsoft.Xna.Framework;

namespace ComfyJamSummer.AI.Enemies
{
    public class EnemyMoveState : BaseAIState<Enemy>
    {
        public EnemyMoveState(Prefabs prefabs, GameManager manager) : base(prefabs, manager)
        {
        }

        float _stalkDistanceLimit;

        public override void Begin()
        {
            base.Begin();

            _stalkDistanceLimit = _context.SpriteWidth * 20;
        }

        public override void Update(float deltaTime)
        {
            if (!Validate())
            {
                return;
            }

            var player = UtilHelper.GetEntity<Player>();

            if (player == null)
            {
                return;
            }

            if (Vector2.Distance(_context.Position, player.Position) <= _stalkDistanceLimit)
            {
                _machine.ChangeState<EnemyAttackState>();
            }
            else
            {
                _context?.Stalk(_context.Player);
            }
        }
    }
}
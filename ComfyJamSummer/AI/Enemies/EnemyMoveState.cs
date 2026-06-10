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

            if (Vector2.Distance(_context.Position, player.Position) <= player.SpriteHeight * 14)
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
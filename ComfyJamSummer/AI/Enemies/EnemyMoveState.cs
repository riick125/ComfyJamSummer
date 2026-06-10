using ComfyJamSummer.Entities;
using ComfyJamSummer.Manager;
using ComfyJamSummer.Prefab;

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

            if (_machine.ElapsedTimeInState >= _context.IdleTimeState)
            {
                _machine.ChangeState<EnemyAttackState>();
            }
            else
            {
                _context?.Stalk(_context.Player, _context.RadiusAlertArea);
            }
        }
    }
}
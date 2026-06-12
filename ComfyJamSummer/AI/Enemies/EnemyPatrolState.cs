using ComfyJamSummer.Entities;
using ComfyJamSummer.Helpers;
using ComfyJamSummer.Manager;
using ComfyJamSummer.Prefab;

namespace ComfyJamSummer.AI.Enemies
{
    public class EnemyPatrolState : BaseAIState<Enemy>
    {
        public EnemyPatrolState(Prefabs prefabs, GameManager manager) : base(prefabs, manager)
        {
        }

        public override void Update(float deltaTime)
        {
            if (!Validate())
            {
                return;
            }

            var player = UtilHelper.Player();

            if (player == null)
            {
                return;
            }

            if (_machine.ElapsedTimeInState >= _context.PatrolTimeState)
            {
                if (_context.TimeLeftToNextAtk <= 0)
                {
                    _machine.ChangeState<EnemyAttackState>();
                    return;
                }
            }

            _context.Patrol();
        }
    }
}
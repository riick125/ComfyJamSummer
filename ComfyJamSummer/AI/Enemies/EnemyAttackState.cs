using ComfyJamSummer.Entities;
using ComfyJamSummer.Manager;
using ComfyJamSummer.Prefab;

namespace ComfyJamSummer.AI.Enemies
{
    public class EnemyAttackState : BaseAIState<Enemy>
    {
        public EnemyAttackState(Prefabs prefabs, GameManager manager) : base(prefabs, manager)
        {
        }

        public override void Update(float deltaTime)
        {
            if (!Validate())
            {
                return;
            }

            if (_machine.ElapsedTimeInState >= _context.AtkTimeState)
            {
                _machine.ChangeState<EnemyIdleState>();
            }
            else
            {
                _context?.Attack();
            }
        }
    }
}
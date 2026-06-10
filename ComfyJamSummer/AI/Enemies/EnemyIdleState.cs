using ComfyJamSummer.Entities;
using ComfyJamSummer.Manager;
using ComfyJamSummer.Prefab;

namespace ComfyJamSummer.AI.Enemies
{
    public class EnemyIdleState : BaseAIState<Enemy>
    {
        public EnemyIdleState(Prefabs prefabs, GameManager manager) : base(prefabs, manager)
        {
        }

        public override void Begin()
        {
            base.Begin();

            _context?.Idle();
        }

        public override void Update(float deltaTime)
        {
            if (!Validate())
            {
                return;
            }

            if (_machine.ElapsedTimeInState >= _context.IdleTimeState)
            {
                _machine.ChangeState<EnemyMoveState>();
            }
        }
    }
}
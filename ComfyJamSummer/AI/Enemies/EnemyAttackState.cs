using ComfyJamSummer.Entities;
using ComfyJamSummer.Enums;
using ComfyJamSummer.Helpers;
using ComfyJamSummer.Manager;
using ComfyJamSummer.Prefab;

namespace ComfyJamSummer.AI.Enemies
{
    public class EnemyAttackState : BaseAIState<Enemy>
    {
        public EnemyAttackState(Prefabs prefabs, GameManager manager) : base(prefabs, manager)
        {
        }

        public override void Begin()
        {
            base.Begin();

            _context.AtkTimeState = _context.AtkSpeed;
            AnimHelper.Play(_context.Animator, BirbAnim.Atk, Nez.Sprites.SpriteAnimator.LoopMode.Once);
        }

        public override void Update(float deltaTime)
        {
            if (!Validate())
            {
                return;
            }

            //if (_machine.ElapsedTimeInState >= _context.AtkTimeState && _context.Animator.AnimationState == Nez.Sprites.SpriteAnimator.State.Completed)
            if (_context.TimeLeftToNextAtk > 0)
            {
                _machine.ChangeState<EnemyIdleState>();
            }
            else
            {
                var player = UtilHelper.Player();

                _context?.Attack(player);
            }
        }
    }
}
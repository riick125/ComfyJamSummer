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

            if (_context.TimeLeftToNextAtk <= 0)
            {
                AnimHelper.Play(_context.Animator, BirbAnim.Atk, Nez.Sprites.SpriteAnimator.LoopMode.ClampForever);
            }
        }

        public override void Update(float deltaTime)
        {
            if (!Validate())
            {
                return;
            }

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
using ComfyJamSummer.Entities;
using ComfyJamSummer.Enums;
using ComfyJamSummer.Helpers;
using ComfyJamSummer.Manager;
using ComfyJamSummer.Prefab;
using System.Linq;

namespace ComfyJamSummer.AI.Enemies
{
    public class CrabIdleState : BaseAIState<Crab>
    {
        public CrabIdleState(Prefabs prefabs, GameManager manager) : base(prefabs, manager)
        {
        }

        public override void Update(float deltaTime)
        {
            base.Update(deltaTime);

            if (!AnimHelper.CurrentAnim(_context.Animator, CrabAnim.Eat))
            {
                AnimHelper.Play(_context.Animator, _context.IsHungry ? CrabAnim.Hungry : CrabAnim.Idle);
            }
            else if (_context.Animator.AnimationState == Nez.Sprites.SpriteAnimator.State.Completed)
            {
                AnimHelper.Play(_context.Animator, _context.IsHungry ? CrabAnim.Hungry : CrabAnim.Idle);
            }

            if (_context.LostPatience)
            {
                _machine.ChangeState<CrabPissedOffState>();
            }
            else if (!_context.IsHungry && !_context.CantBuild)
            {
                var rocket = UtilHelper.GetEntity<Rocket>();

                if (rocket != null && rocket.BuildPhases.All(x=> x.IsDone))
                {
                    _machine.ChangeState<CrabIdleState>();

                    return;
                }

                _machine.ChangeState<CrabBuildState>();
            }
        }
    }
}
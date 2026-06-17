using ComfyJamSummer.Entities;
using ComfyJamSummer.Enums;
using ComfyJamSummer.Helpers;
using ComfyJamSummer.Manager;
using ComfyJamSummer.Prefab;
using Nez;
using static Nez.Sprites.SpriteAnimator;

namespace ComfyJamSummer.AI.Enemies
{
    public class CrabBuildState : BaseAIState<Crab>
    {
        float _timeLeftToNextHammer;

        int _hammerHitsToLoseSatiation = 25, _actualHammerHits;

        public CrabBuildState(Prefabs prefabs, GameManager manager) : base(prefabs, manager)
        {
        }

        public override void Begin()
        {
            base.Begin();

            _timeLeftToNextHammer = 1f;
        }

        public override void Update(float deltaTime)
        {
            if (!Validate())
            {
                return;
            }

            if (AnimHelper.CurrentAnim(_context.Animator, CrabAnim.Eat) && _context.Animator.AnimationState != State.Completed)
            {
                return;
            }

#if DEBUG
            deltaTime *= 8;
#endif

            if (_context.CantBuild || _context.IsHungry)
            {
                _machine.ChangeState<CrabIdleState>();
            }
            else if (_context.LostPatience)
            {
                _machine.ChangeState<CrabPissedOffState>();
            }
            else
            {
                AnimHelper.Play(_context.Animator, CrabAnim.Build);

                if (_timeLeftToNextHammer <= 0)
                {
                    var rocket = UtilHelper.GetEntity<Rocket>();

                    if (rocket != null)
                    {
                        rocket.ProgressBuild();
                    }

                    _actualHammerHits++;

                    _timeLeftToNextHammer = 0.65f;

                    if (_actualHammerHits >= _hammerHitsToLoseSatiation)
                    {
                        _context.ModifySatiation();
                        _actualHammerHits = 0;
                    }
                }
                else
                {
                    _timeLeftToNextHammer -= deltaTime;
                }
            }
        }
    }
}
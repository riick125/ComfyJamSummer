using ComfyJamSummer.Entities;
using ComfyJamSummer.Enums;
using ComfyJamSummer.Helpers;
using ComfyJamSummer.Manager;
using ComfyJamSummer.Prefab;
using Nez;
using System;
using System.Linq;
using static Nez.Sprites.SpriteAnimator;

namespace ComfyJamSummer.AI.Enemies
{
    public class CrabBuildState : BaseAIState<Crab>
    {
        float _timeLeftToNextHammer;

        int _hammerHitsToLoseSatiation, _actualHammerHits;

        readonly int _originalHammerHits = 8;

        float _animCd, _timeLeftToNextAnimation;
        readonly float _hammerCd = 0.65f;

        public CrabBuildState(Prefabs prefabs, GameManager manager) : base(prefabs, manager)
        {
            _hammerHitsToLoseSatiation = _originalHammerHits;
        }

        public override void Begin()
        {
            base.Begin();

            _timeLeftToNextHammer = _hammerCd;

            ResetAnim();
        }

        public override void End()
        {
            if (_context.Animator != null)
            {
                _context.Animator.Speed = 1f;
            }
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

            if (AnimHelper.CurrentAnim(_context.Animator, CrabAnim.Build) && _context.Animator.AnimationState == State.Completed)
            {
                _context.Animator.Speed = 1f;
                AnimHelper.Play(_context.Animator, CrabAnim.Idle);
            }

            if (_timeLeftToNextAnimation > 0)
                _timeLeftToNextAnimation -= Time.DeltaTime;
            else
            {
                if (!AnimHelper.CurrentAnim(_context.Animator, CrabAnim.Build))
                {
                    ResetAnim();

                    AnimHelper.Play(_context.Animator, CrabAnim.Build, LoopMode.ClampForever);
                }
            }

            var rocket = UtilHelper.GetEntity<Rocket>();

            if (rocket == null)
            {
                return;
            }

            if (_context.IsHungry || rocket.BuildPhases.All(x => x.IsDone))
            {
                _machine.ChangeState<CrabIdleState>();
            }
            else if (_context.LostPatience)
            {
                _machine.ChangeState<CrabPissedOffState>();
            }
            else
            {
                if (_timeLeftToNextHammer <= 0)
                {
                    var isSatiated = _context.ActualSatiation > (_context.MaxSatiation * 0.6f);

                    if (rocket != null)
                    {
                        rocket.ProgressBuild(isSatiated);
                    }

                    _actualHammerHits++;

                    _timeLeftToNextHammer = isSatiated ? _hammerCd * 0.82f : _hammerCd;

                    if (_actualHammerHits >= _hammerHitsToLoseSatiation)
                    {
                        _context.ModifySatiation();
                        _actualHammerHits = 0;

                        _hammerHitsToLoseSatiation = (_originalHammerHits + (int)(Nez.Random.NextInt(2) * (Nez.Random.Chance(50) ? -1 : 1)));
                    }
                }
                else
                {
                    _timeLeftToNextHammer -= deltaTime;
                }
            }
        }

        void ResetAnim()
        {
            _animCd = Nez.Random.Chance(50) ? Nez.Random.Range(0.04f, 0.4f) : Nez.Random.Range(0.55f, 1.25f);

            _timeLeftToNextAnimation = _animCd;

            if (_context.Animator != null)
            {
                _context.Animator.Speed = (float)Math.Round(Nez.Random.Range(1f, 1.35f), 2);
            }
        }
    }
}
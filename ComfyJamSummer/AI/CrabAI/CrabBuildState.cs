using ComfyJamSummer.Entities;
using ComfyJamSummer.Enums;
using ComfyJamSummer.Helpers;
using ComfyJamSummer.Manager;
using ComfyJamSummer.Prefab;
using Nez;
using Nez.Systems;
using System.Collections;

namespace ComfyJamSummer.AI.Enemies
{
    public class CrabBuildState : BaseAIState<Crab>
    {
        float _timeLeftToNextHammer;
        readonly float _hammerCdMin = 3.9f, _hammerCdMax = 5.5f;

        float _doubleSequenceChance = 0.25f;
        float _intervalBetweenSequence = 0.12f, _timeLeftToNextSequence;

        CoroutineManager _coroutineManager;

        public CrabBuildState(Prefabs prefabs, GameManager manager) : base(prefabs, manager)
        {
            _coroutineManager = new CoroutineManager();

            NextHammer();
        }

        public override void Begin()
        {
            base.Begin();

            AnimHelper.Play(_context.Animator, CrabAnim.Idle);
        }

        public override void Update(float deltaTime)
        {
            if (!Validate())
            {
                return;
            }

            if (_timeLeftToNextHammer <= 0)
            {                
                Core.StartCoroutine(HitHammer());

                NextHammer();
            }
            else
            {
                _timeLeftToNextHammer -= deltaTime;
            }
        }

        void NextHammer()
        {
            _timeLeftToNextHammer = Nez.Random.Range(_hammerCdMin, _hammerCdMax);
        }

        IEnumerator HitHammer()
        {
            var combo = Nez.Random.Chance(100) ? 2 : 1;
            //var combo = Nez.Random.Chance(_doubleSequenceChance) ? 2 : 1;

            for (int i = 0; i < combo; i++)
            {
                AnimHelper.Play(_context.Animator, CrabAnim.Build);

                if (i < combo - 1)
                {
                    yield return Coroutine.WaitForSeconds(0.5f);

                    yield return Coroutine.WaitForSeconds(_intervalBetweenSequence);
                }
            }

            yield return null;
        }
    }
}
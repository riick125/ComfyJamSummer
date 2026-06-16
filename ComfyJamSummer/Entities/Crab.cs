using ComfyJamSummer.Entities.Configs;
using ComfyJamSummer.Entities.Creatures;
using ComfyJamSummer.Enums;
using ComfyJamSummer.Helpers;
using Nez;

namespace ComfyJamSummer.Entities
{
    public class Crab : Creature
    {
        public CrabAnim ActualState { get; set; }

        float _satiation = 50, _satiationLossPerWave, _maxSatiation = 100;
        float _hungryValue = 25;

        public bool IsHungry { get { return _satiation <= _hungryValue; } }

        public Crab CloneCrab(CreatureConfig config)
        {
            var clone = base.CloneCreature(config) as Crab;

            clone.ActualState = CrabAnim.Talk;

            AnimHelper.Play(clone.Animator, CrabAnim.Idle);

            return clone;
        }

        public void PassWave()
        {
            _satiation -= _satiationLossPerWave;
            _satiation = Mathf.Clamp(_satiation, 0, _maxSatiation);
        }

        public override void Update()
        {
            base.Update();

            switch (ActualState)
            {
                case CrabAnim.Walk:
                    break;
                case CrabAnim.Talk:
                    break;
                case CrabAnim.Hungry:
                    break;
                case CrabAnim.Build:

                    break;
                case CrabAnim.Pissed:
                    break;
            }
        }
    }
}
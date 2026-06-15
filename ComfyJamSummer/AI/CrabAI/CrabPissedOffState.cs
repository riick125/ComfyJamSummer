using ComfyJamSummer.Entities;
using ComfyJamSummer.Enums;
using ComfyJamSummer.Helpers;
using ComfyJamSummer.Manager;
using ComfyJamSummer.Prefab;

namespace ComfyJamSummer.AI.Enemies
{
    public class CrabPissedOffState : BaseAIState<Crab>
    {
        public CrabPissedOffState(Prefabs prefabs, GameManager manager) : base(prefabs, manager)
        {
        }

        public override void Begin()
        {
            base.Begin();

            AnimHelper.Play(_context.Animator, CrabAnim.Pissed);
        }
    }
}
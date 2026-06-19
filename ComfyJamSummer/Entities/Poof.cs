using ComfyJamSummer.Entities.Base;
using ComfyJamSummer.Helpers;
using Microsoft.Xna.Framework;

namespace ComfyJamSummer.Entities
{
    public class Poof : Animated
    {
        public Poof ClonePoof(Vector2 pos)
        {
            var clone = base.CloneAnimated(pos) as Poof;

            AnimHelper.Play(clone.Animator, "Poof", Nez.Sprites.SpriteAnimator.LoopMode.ClampForever);

            clone.Animator.OnAnimationCompletedEvent += Animator_OnAnimationCompletedEvent;

            return clone;
        }

        private void Animator_OnAnimationCompletedEvent(string obj)
        {
            if (!IsDestroyed && Scene != null)
            {
                this.Destroy();
            }
        }
    }
}
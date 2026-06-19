using Nez.Sprites;
using System;
using static Nez.Sprites.SpriteAnimator;

namespace ComfyJamSummer.Helpers
{
    public static class AnimHelper
    {
        public static void Play(SpriteAnimator animator, Enum name, LoopMode? loopMode = null)
        {
            if (animator == null || name == null) return;

            var animation = name.ToString();

            if (!animator.Animations.ContainsKey(animation))
                return;

            if (animator.CurrentAnimationName != animation)
            {
                animator.Play(animation, loopMode);
            }
        }

        public static void Play(SpriteAnimator animator, string name, LoopMode? loopMode = null)
        {
            if (animator == null || name == null) return;

            if (!animator.Animations.ContainsKey(name))
                return;

            if (animator.CurrentAnimationName != name)
            {
                animator.Play(name, loopMode);
            }
        }

        public static bool CurrentAnim(SpriteAnimator animator, Enum name)
        {
            if (animator == null || name == null) return false;

            var animation = name.ToString();

            if (!animator.Animations.ContainsKey(animation))
                return false;

            return animator.CurrentAnimationName == animation;
        }
    }
}

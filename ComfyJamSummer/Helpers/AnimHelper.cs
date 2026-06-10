using Nez.Sprites;
using System;

namespace ComfyJamSummer.Helpers
{
    public static class AnimHelper
    {
        public static void Play(SpriteAnimator animator, Enum name)
        {
            if (animator == null || name == null) return;

            var animation = name.ToString();

            if (!animator.Animations.ContainsKey(animation))
                return;

            if (animator.CurrentAnimationName != animation)
            {
                animator.Play(animation);
            }
        }
    }
}

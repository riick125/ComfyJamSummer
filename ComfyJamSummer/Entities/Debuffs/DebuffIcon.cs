using System.Linq;
using ComfyJamSummer.Entities.Base;
using ComfyJamSummer.Enums;
using Microsoft.Xna.Framework;

namespace ComfyJamSummer.Entities.Debuffs
{
    public class DebuffIcon : Animated
    {
        public DebuffType Type { get; set; }

        public float TimeLeftToDisappear { get; set; }
        public float Duration { get; private set; }

        public Vector2 Offset { get; set; }

        public DebuffIcon Clonar(float duration, Vector2 offset)
        {
            var clone = base.CloneAnimated(default) as DebuffIcon;
            clone.Type = Type;
            clone.TimeLeftToDisappear = duration;
            clone.Duration = duration;
            clone.Offset = offset;

            clone.Animator.SetColor(Color.White);

            var uniqueAnimation = clone.Animator.Animations.FirstOrDefault();

            clone.Animator.Play(uniqueAnimation.Key);

            return clone;
        }
    }
}

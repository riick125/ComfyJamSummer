using ComfyJamSummer.Entities.Base;
using ComfyJamSummer.Helpers;
using Microsoft.Xna.Framework;
using Nez;

namespace ComfyJamSummer.Entities
{
    public class Poof : Animated
    {
        float _soundDelay;

        bool _played;

        public Poof ClonePoof(Vector2 pos)
        {
            var clone = base.CloneAnimated(pos) as Poof;

            clone._soundDelay = 0.025f;

            AnimHelper.Play(clone.Animator, "Poof", Nez.Sprites.SpriteAnimator.LoopMode.ClampForever);

            clone.Animator.Speed = 1.25f;

            return clone;
        }

        public override void Update()
        {
            base.Update();

            if (_soundDelay <= 0 && !_played)
            {
                Prefabs?.PlaySoundRandomPitch(Enums.SoundFxName.Poof, 0.1f);
                _played = true;
            }
            else
            {
                _soundDelay -= Time.DeltaTime;
            }

            if (Animator.AnimationState == Nez.Sprites.SpriteAnimator.State.Completed & !this.IsDestroyed)
            {
                this.Destroy();
            }
        }
    }
}
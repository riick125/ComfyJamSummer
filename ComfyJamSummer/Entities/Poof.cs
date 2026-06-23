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

        public override void Update()
        {
            base.Update();

            if (_soundDelay <= 0 && !_played)
            {
                Prefabs?.PlaySoundRandomPitch(Enums.SoundFxName.Collect_1, 0.1f);
                _played = true;
            }
            else
            {
                _soundDelay -= Time.DeltaTime;
            }
        }
    }
}
using ComfyJamSummer.Components.Gameplay;
using ComfyJamSummer.Entities.Base;
using ComfyJamSummer.Enums;
using ComfyJamSummer.Helpers;
using ComfyJamSummer.Manager;
using ComfyJamSummer.Prefab;
using Nez;
using Nez.Sprites;
using System;
using System.Linq;
using static ComfyJamSummer.Entities.Base.Animated;

namespace ComfyJamSummer.Components.General
{
    public class SoundAnimation : BaseComponent, IUpdatable
    {
        Animated _entity;

        SpriteAnimator _animator;

        SoundAnimator _actualSoundAnimator, _previousSoundAnimator;

        SoundPerFrame _actualFrame, _previousFrame;

        public SoundAnimation(GameManager manager, Prefabs prefabs) : base(manager, prefabs)
        {
        }

        public override void OnAddedToEntity()
        {
            base.OnAddedToEntity();

            _entity = this.Entity as Animated;

            _animator = _entity.Animator;
        }

        public void Update()
        {
            if (!Validate(_entity))
                return;

            if (_animator == null)
                return;

            var animators = _entity.SoundAnimators;

            if (animators == null)
            {
                return;
            }

            var currentAnim = animators.FirstOrDefault(x => x.AnimationName.Equals(_animator.CurrentAnimationName, System.StringComparison.InvariantCultureIgnoreCase));

            if (currentAnim != null)
            {
                if (_previousSoundAnimator != null && _previousSoundAnimator != _actualSoundAnimator)
                {
                    _previousSoundAnimator.SoundFrames.ForEach(x =>
                    {
                        x.AlreadyPlayed = false;
                    });

                    _previousSoundAnimator = _actualSoundAnimator;
                }

                _actualSoundAnimator = currentAnim;

                for (int i = 0; i < _actualSoundAnimator.SoundFrames.Count; i++)
                {
                    if (_actualSoundAnimator.SoundFrames[i].ActualFrame != _animator.CurrentFrame)
                    {
                        _actualSoundAnimator.SoundFrames[i].AlreadyPlayed = false;
                    }
                }

                var currentFrame = _actualSoundAnimator.SoundFrames.FirstOrDefault(x => x.ActualFrame == _animator.CurrentFrame);

                if (currentFrame != null)
                {
                    if (!currentFrame.AlreadyPlayed)
                    {
                        var sfx = SoundFxName.Walk_1;

                        if (Enum.TryParse<SoundFxName>(currentFrame.SoundName, out sfx))
                        {
                            if (SoundHelper.HaveVariations(sfx))
                            {
                                SoundHelper.PlayRandomSound(sfx, maxPitchValue: currentFrame.PitchMaxValue);
                            }
                            else
                            {
                                if (currentFrame.AllowPitchChange)
                                {
                                    _prefabs.PlaySoundRandomPitch(sfx, currentFrame.PitchMaxValue);
                                }
                                else
                                {
                                    _prefabs.PlaySound(sfx);
                                }
                            }
                        }

                        currentFrame.AlreadyPlayed = true;
                    }
                }
            }
        }
    }
}
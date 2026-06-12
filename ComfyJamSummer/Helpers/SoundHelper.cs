using ComfyJamSummer.Entities.Base;
using ComfyJamSummer.Enums;
using ComfyJamSummer.Prefab;
using ComfyJamSummer.Scenes;
using Nez;
using System;
using System.Collections.Generic;
using System.Linq;
using static ComfyJamSummer.Entities.Base.Animated;

namespace ComfyJamSummer.Helpers
{
    public static class SoundHelper
    {
        private static List<SoundFxName> _list = new List<SoundFxName>() { SoundFxName.Collect_1, SoundFxName.Collect_2, SoundFxName.Collect_3 };

        private static List<SoundFxName> _walkSoundList = new List<SoundFxName>() { SoundFxName.Walk_1, SoundFxName.Walk_2, SoundFxName.Walk_3, SoundFxName.Walk_4 };

        static SoundFxName[] _tempTransferList;

        public static void CreateSoundFrames(Animated entity, List<SoundPerFrame> sounds, int[] framesThatShouldPlaySound, string animName, SoundFxName soundName, bool allowPitchChange = false)
        {
            if (!ValidateSoundFrames(entity, sounds, framesThatShouldPlaySound))
            {
                return;
            }

            var sprites = entity.Animator.Animations.FirstOrDefault(x => x.Key == animName).Value;

            if (sprites != null)
            {
                for (int i = 0; i < sprites.Sprites.Count(); i++)
                {
                    var sprite = sprites.Sprites[i];

                    var soundPerFrame = new SoundPerFrame()
                    {
                        ActualFrame = i,
                        SoundName = soundName.ToString(),
                        AnimationName = animName,
                        AllowPitchChange = allowPitchChange
                    };

                    if (framesThatShouldPlaySound.Contains(i))
                    {
                        soundPerFrame.ShouldPlay = true;
                    }

                    sounds.Add(soundPerFrame);
                }
            }
        }

        private static bool ValidateSoundFrames(Animated entity, List<SoundPerFrame> sounds, int[] framesThatShouldPlaySound)
        {
            if (entity == null) return false;

            if (entity.Animator == null) return false;

            if (!entity.Animator.Animations.Any()) return false;

            if (framesThatShouldPlaySound == null) return false;

            if (!framesThatShouldPlaySound.Any()) return false;

            return true;
        }

        public static void PlayRandomSound(SoundFxName listName, float volReducePercent = 0.6f)
        {
            var prefabs = UtilHelper.Prefabs();

            if (!Validate(prefabs))
            {
                return;
            }

            if (Game1.SoundManager == null)
            {
                return;
            }

            if (_tempTransferList == null)
            {
                _tempTransferList = new SoundFxName[10];
            }

            var listSound = new List<SoundFxName>();

            switch (listName)
            {
                case SoundFxName.Bash:
                    break;

                case SoundFxName.Collect_1:
                case SoundFxName.Collect_2:
                case SoundFxName.Collect_3:
                    _list.CopyTo(_tempTransferList);
                    break;

                case SoundFxName.Walk_1:
                case SoundFxName.Walk_2:
                case SoundFxName.Walk_3:
                case SoundFxName.Walk_4:
                    _walkSoundList.CopyTo(_tempTransferList);
                    break;
            }

            listSound = _tempTransferList.Distinct().ToList();

            var volume = 1f;

            if (Core.Scene != null && Core.Scene is InGameScene gameScene)
            {
                var quantity = 0;

                for (int i = 0; i < listSound.Count; i++)
                {
                    var name = listSound[i];

                    quantity += Game1.SoundManager.GetPlayingSoundsQuantity(name.ToString());
                }

                volume = quantity > 1 ? volReducePercent : 0;
            }

            listSound.Shuffle();

            var selected = Nez.Random.Chance(50) ? listSound.FirstOrDefault() : listSound[Nez.Random.Range(0, listSound.Count)];

            prefabs.PlaySoundRandomPitch(selected, 0.05f, volume);
        }

        private static bool Validate(Prefabs prefabs)
        {
            if (prefabs == null)
            {
                return false;
            }

            return true;
        }
    }
}
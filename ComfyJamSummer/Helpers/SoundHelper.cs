using ComfyJamSummer.Enums;
using ComfyJamSummer.Prefab;
using ComfyJamSummer.Scenes;
using Nez;
using System.Collections.Generic;
using System.Linq;

namespace ComfyJamSummer.Helpers
{
    public static class SoundHelper
    {
        private static List<SoundFxName> _list = new List<SoundFxName>() { SoundFxName.Collect_1, SoundFxName.Collect_2, SoundFxName.Collect_3 };

        private static List<SoundFxName> _walkSoundList = new List<SoundFxName>() { SoundFxName.Walk_1, SoundFxName.Walk_2, SoundFxName.Walk_3, SoundFxName.Walk_4 };

        static SoundFxName[] _tempTransferList;

        public static bool HaveVariations(SoundFxName listName)
        {
            switch (listName)
            {
                case SoundFxName.Bash:
                    break;

                case SoundFxName.Collect_1:
                case SoundFxName.Collect_2:
                case SoundFxName.Collect_3:
                    return true;

                case SoundFxName.Walk_1:
                case SoundFxName.Walk_2:
                case SoundFxName.Walk_3:
                case SoundFxName.Walk_4:
                    return true;
            }

            return false;
        }

        public static void PlayRandomSound(SoundFxName listName, float volReducePercent = 0.6f, float maxPitchValue = 0.05f)
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

            var listSound = new List<SoundFxName>();

            switch (listName)
            {
                case SoundFxName.Bash:
                    break;

                case SoundFxName.Collect_1:
                case SoundFxName.Collect_2:
                case SoundFxName.Collect_3:
                    _tempTransferList = new SoundFxName[_list.Count];
                    _list.CopyTo(_tempTransferList);
                    break;

                case SoundFxName.Walk_1:
                case SoundFxName.Walk_2:
                case SoundFxName.Walk_3:
                case SoundFxName.Walk_4:
                    _tempTransferList = new SoundFxName[_walkSoundList.Count];
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

            prefabs.PlaySoundRandomPitch(selected, maxPitchValue, volume);
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
using System;
using System.Collections.Generic;
using System.Linq;
using ComfyJamSummer.Enums;

namespace ComfyJamSummer.Save
{
    [Serializable]
    public class SaveData : ICloneable
    {
        public DateTime LastSaveDate { get; set; }

        public Dictionary<string, int> BindedKeys { get; set; }

        public Dictionary<string, int> BindedButtons { get; set; }

        public GameResolution ChosenResolution { get; set; }

        public GameLanguage ChosenLanguage { get; set; }
        public List<AchievementEnum> UnlockedAchievements { get; set; }

        public bool ScanlineEffectState { get; set; }

        public bool IsFullScreen { get; set; }

        public uint Fps { get; set; }

        public bool IsVSyncEnabled { get; set; }

        public float MusicActualVolumePercentage { get; set; }

        public float SoundFxActualVolumePercentage { get; set; }

        public int ControllerSensitivity { get; set; }

        /// <summary>
        /// How many matches the player played.
        /// </summary>
        public int MatchesPlayed { get; set; }

        /// <summary>
        /// How many times the player won the game.
        /// </summary>
        public int Wins { get; set; }

        /// <summary>
        /// How many times the player died.
        /// </summary>
        public int Deaths { get; set; }


        public SaveData()
        {
            ControllerSensitivity = Constants.CONTROLLER_SENSITIVITY_DEFAULT_VALUE;

            ScanlineEffectState = true;

            ChosenLanguage = GameLanguage.English;

            ChosenResolution = GameResolution._1280x720;

            Fps = 60;

            IsVSyncEnabled = false;

            BindedKeys = new Dictionary<string, int>();

            BindedButtons = new Dictionary<string, int>();

            MusicActualVolumePercentage = 1f;
            SoundFxActualVolumePercentage = 1f;

#if DEBUG
            Fps = 120;
            MusicActualVolumePercentage = 0.5f;
            SoundFxActualVolumePercentage = 0.5f;
#endif
        }

        public object Clone()
        {
            return this.MemberwiseClone() as SaveData;
        }

        public void AddAchievement(AchievementEnum name)
        {
            if (UnlockedAchievements == null)
            {
                return;
            }

            if (UnlockedAchievements.Any(x => x == name))
            {
                return;
            }

            UnlockedAchievements.Add(name);

            UnlockedAchievements = UnlockedAchievements.Distinct().ToList();
        }
    }
}
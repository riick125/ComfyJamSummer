using System.Collections.Generic;
using System.Linq;
using ComfyJamSummer.Achievements;
using ComfyJamSummer.Enums;
using ComfyJamSummer.Helpers;
using ComfyJamSummer.Manager;
using ComfyJamSummer.Results;
using ComfyJamSummer.Save;
using Nez;

namespace ComfyJamSummer.Components.Extensions
{
    public class AchievementManager : CustomSceneComponent
    {
        private List<Achievement> _achievements;

        public List<Achievement> Achievements { get { return _achievements; } }

        private GameManager gameManager;

        public AchievementManager()
        {
        }

        public override void OnEnabled()
        {
            base.OnEnabled();

            if (gameManager == null)
            {
                gameManager = UtilHelper.GameManager();
            }

            ProcessInitialAchievements(Game1.SaveData);

            ProcessAllAchievementsByWinOrDeath();

            SaveHelper.SaveGame(Game1.SaveData);
        }

        public void ProcessInitialAchievements(SaveData saveData)
        {
            if (saveData == null)
            {
                return;
            }
        }

        public void ProcessAllAchievementsByWinOrDeath()
        {
            var saveData = Game1.SaveData;

            if (saveData == null)
            {
                return;
            }
        }

        /// <summary>
        /// for those achievements that has a simple condition, like: first death, first time killing certain boss... 
        /// </summary>
        /// <param name="achievementName"></param>
        public void ProcessBaseCheckAchievement(AchievementEnum achievementName)
        {
            var achievementResult = Validate(achievementName);

            if (!achievementResult.IsValid)
            {
                return;
            }

            achievementResult.Achievement.Unlock();
        }

        public override void Update()
        {
            base.Update();

            if (Scene == null)
            {
                return;
            }
        }

        private AchievementResult Validate(AchievementEnum achievementName)
        {
            if (_achievements == null)
            {
                return new AchievementResult(null);
            }

            if (Game1.SaveData != null && Game1.SaveData.UnlockedAchievements.Any(x => x == achievementName))
            {
                return new AchievementResult(null);
            }

            var achievement = _achievements.FirstOrDefault(x => x.Name == achievementName);

            if (achievement == null)
            {
                return new AchievementResult(null);
            }

            if (achievement.IsUnlocked)
            {
                return new AchievementResult(null);
            }

            return new AchievementResult(achievement);
        }
    }
}
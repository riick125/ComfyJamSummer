using ComfyJamSummer.Enums;

namespace ComfyJamSummer.Achievements
{
    public class Achievement
    {
        public AchievementEnum Name { get; set; }

        public bool IsUnlocked { get; private set; }

        public Achievement(AchievementEnum name)
        {
            this.Name = name;
        }

        public void Unlock()
        {
            var achieved = Game1.SteamScript?.UnlockAchievement(this.Name);

            if (achieved.HasValue)
            {
                IsUnlocked = achieved.Value;
            }
        }
    }
}
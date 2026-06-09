using ComfyJamSummer.Achievements;

namespace ComfyJamSummer.Results
{
    public class AchievementResult
    {
        public Achievement Achievement { get; set; }

        public bool IsValid { get; private set; }

        public AchievementResult(Achievement achievement)
        {
            this.Achievement = achievement;

            IsValid = achievement != null;
        }
    }
}
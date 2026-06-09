using ComfyJamSummer.Save;

namespace ComfyJamSummer.Helpers
{
    public class SaveHelper
    {
        public static SaveData GetLocalSaveData()
        {
            if (Game1.SaveGameComponent != null)
            {
                Game1.SaveData = Game1.SaveGameComponent.LoadLocalSave();

                return Game1.SaveData;
            }

            return null;
        }

        public static bool SaveGame(SaveData saveData)
        {
            if (Game1.SaveGameComponent != null)
            {
                return Game1.SaveGameComponent.Save(saveData);
            }

            return false;
        }
    }
}

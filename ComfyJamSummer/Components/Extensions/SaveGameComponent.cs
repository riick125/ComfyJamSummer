using System.IO;
using System.Text.Json;
using ComfyJamSummer.Entities.Creatures;
using ComfyJamSummer.Helpers;
using ComfyJamSummer.Save;

namespace ComfyJamSummer.Components.Extensions
{
    public class SaveGameComponent
    {
        public bool Save(SaveData gameData)
        {
            try
            {
                var path = Constants.SAVE_PATH;

                var directory = Path.GetDirectoryName(path);

                if (!Directory.Exists(directory))
                {
                    Directory.CreateDirectory(directory);
                }

                var serializedText = JsonSerializer.Serialize(gameData);
                File.WriteAllText(path, serializedText);

                Game1.SaveData = gameData;

                return true;
            }
            catch (System.Exception)
            {
                return false;
            }
        }

        public SaveData LoadLocalSave()
        {
            var path = Constants.SAVE_PATH;

            if (!File.Exists(path))
            {
                var newSave = new SaveData();
                Save(newSave);
                return newSave;
            }

            var fileText = File.ReadAllText(path);

            try
            {
                Game1.SaveData = JsonSerializer.Deserialize<SaveData>(fileText);
                return Game1.SaveData;
            }
            catch
            {
            }

            var fallbackSave = new SaveData();

            Save(fallbackSave);

            Game1.SaveData = fallbackSave;

            return fallbackSave;
        }

        public SaveData LoadCloudSaveAsync()
        {
            try
            {
                if (Game1.SteamScript != null)
                {
                    return Game1.SteamScript.GetSaveGameOnCloud();
                }

                return LoadLocalSave();
            }
            catch (System.Exception ex)
            {
                return null;
            }
        }

        public bool HasSavedGame()
        {
            return File.Exists(Constants.SAVE_PATH);
        }

        public void SaveAfterPlayerDeath(Creature player)
        {
            if (player == null)
            {
                return;
            }

            var saveData = SaveHelper.GetLocalSaveData();

            if (saveData == null)
            {
                return;
            }

            SavePlayerImportantStuff(saveData, player, false);

            SaveHelper.SaveGame(saveData);
        }

        public void SavePlayerImportantStuff(SaveData saveData, Creature player, bool isAWin)
        {
            if (isAWin)
            {
                saveData.Wins++;

                //if (!saveData.AccomplishedDifficulties.Contains(saveData.LastDifficultyChosen))
                //{
                //    saveData.AccomplishedDifficulties.Add(saveData.LastDifficultyChosen);
                //}

                //saveData.AccomplishedDifficulties = saveData.AccomplishedDifficulties.Distinct().ToList();
            }
            else
            {
                saveData.Deaths++;
            }
        }
    }
}
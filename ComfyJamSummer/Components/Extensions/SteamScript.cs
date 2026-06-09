using System;
using System.IO;
using System.Text;
using System.Text.Json;
using ComfyJamSummer.Enums;
using ComfyJamSummer.Helpers;
using ComfyJamSummer.Save;
using ComfyJamSummer.UI;
using Nez;
using Steamworks;

namespace ComfyJamSummer.Components.Extensions
{
    public class SteamScript
    {
        protected Callback<GameOverlayActivated_t> m_GameOverlayActivated;

        protected CallResult<RemoteStorageFileReadAsyncComplete_t> _fileReadResult;

        protected CallResult<RemoteStorageFileWriteAsyncComplete_t> _fileWriteResult;

        protected bool _syncronizedCloudSave;
        public bool SyncronizedCloudSave { get { return _syncronizedCloudSave; } }

        public SteamScript()
        {
            if (DestroyIfNull())
            {
                return;
            }

            Start();
        }

        public bool SaveGameToCloud()
        {
            if (DestroyIfNull())
            {
                return false;
            }

            try
            {
                //if (Core.Scene != null)
                //{
                //    Entity savingGameUI = Core.Scene.FindEntity(UINames.SAVING_UI);

                //    if (savingGameUI == null)
                //    {
                //        if (Game1.CustomScene == null)
                //        {
                //            savingGameUI = Core.Scene.CreateEntity(UINames.SAVING_UI);
                //        }
                //        else
                //        {
                //            savingGameUI = Game1.CustomScene.CreateEntityCustom(UINames.SAVING_UI);
                //        }

                //        savingGameUI.AddComponent(new SavingGameUI(UtilHelper.Prefabs()));
                //    }
                //}

                var fileName = Constants.SAVE_PATH;

                Game1.SaveData.LastSaveDate = DateTime.Now.ToUniversalTime();
                var jsonData = JsonSerializer.Serialize(Game1.SaveData);

                var data = Encoding.UTF8.GetBytes(jsonData);

                var handle = SteamRemoteStorage.FileWriteAsync(fileName, data, (uint)data.Length);

                _fileWriteResult.Set(handle);

                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro ao salvar o arquivo na Steam Cloud: {ex.Message}");
                return false;
            }
        }

        public SaveData GetSaveGameOnCloud()
        {
            if (DestroyIfNull())
            {
                return null;
            }

            var fileName = Constants.SAVE_PATH;

            if (SteamRemoteStorage.FileExists(fileName))
            {
                var fileSize = SteamRemoteStorage.GetFileSize(fileName);

                if (fileSize > 0)
                {
                    var buffer = new byte[fileSize];

                    var handle = SteamRemoteStorage.FileReadAsync(fileName, 0, (uint)fileSize);

                    _fileReadResult.Set(handle);
                }
            }
            else
            {
                if (!File.Exists(fileName))
                {
                    SaveHelper.SaveGame(new SaveData());
                }
                else
                {
                    var deserializedData = File.ReadAllText(fileName);

                    try
                    {
                        Game1.SaveData = JsonSerializer.Deserialize<SaveData>(deserializedData);
                    }
                    catch
                    {
                    }

                    if (Game1.SaveData == null)
                    {
                        Game1.SaveData = new SaveData();
                        SaveHelper.SaveGame(Game1.SaveData);
                    }
                }

                return Game1.SaveData;
            }

            return Game1.SaveData;
        }


        private void OnFileWriteCompleted(RemoteStorageFileWriteAsyncComplete_t pCallback, bool bIOFailure)
        {
            if (bIOFailure || pCallback.m_eResult != EResult.k_EResultOK)
            {
                return;
            }

            if (Core.Scene != null)
            {
                DestroySavingGameUI();
            }
        }

        private void DestroySavingGameUI(bool force = false)
        {
            var savingGameUI = Core.Scene.FindEntity("SavingGameUI");

            if (savingGameUI != null)
            {
                if (force)
                {
                    savingGameUI.Destroy();
                }
                else
                {
                    var component = savingGameUI.GetComponent<SavingGameUI>();

                    if (component != null)
                    {
                        component.Finish();
                    }
                }
            }
        }

        private void OnGameOverlayActivated(GameOverlayActivated_t pCallback)
        {
            if (pCallback.m_bActive != 0)
            {
                Debug.Log("Steam Overlay has been activated");
            }
            else
            {
                Debug.Log("Steam Overlay has been closed");
            }
        }

        private void OnFileReadCompleted(RemoteStorageFileReadAsyncComplete_t pCallback, bool bIOFailure)
        {
            if (bIOFailure || pCallback.m_eResult != EResult.k_EResultOK)
            {
                return;
            }

            SaveData steamCloudSaveData = null;

            byte[] buffer = new byte[pCallback.m_cubRead];

            var read = SteamRemoteStorage.FileReadAsyncComplete(pCallback.m_hFileReadAsync, buffer, pCallback.m_cubRead);

            if (read)
            {
                var localFileName = Constants.SAVE_PATH;

                var fileContent = Encoding.UTF8.GetString(buffer);

                steamCloudSaveData = JsonSerializer.Deserialize<SaveData>(fileContent);

                if (File.Exists(localFileName))
                {
                    var deserializedData = File.ReadAllText(localFileName);

                    var localSaveData = JsonSerializer.Deserialize<SaveData>(deserializedData);

                    if (localSaveData == null)
                    {
                        SaveHelper.SaveGame(steamCloudSaveData);

                        return;
                    }

                    if (steamCloudSaveData?.LastSaveDate > localSaveData?.LastSaveDate)
                    {
                        SaveHelper.SaveGame(steamCloudSaveData);
                    }
                    else if (steamCloudSaveData?.LastSaveDate < localSaveData?.LastSaveDate)
                    {
                        SaveHelper.SaveGame(localSaveData);
                    }
                    else
                    {
                        SaveHelper.SaveGame(steamCloudSaveData);
                    }
                }
                else
                {
                    SaveHelper.SaveGame(steamCloudSaveData);
                }

                _syncronizedCloudSave = true;
            }
        }

        void Start()
        {
            if (Game1.SteamManager.Initialized)
            {
                string name = SteamFriends.GetPersonaName();

                m_GameOverlayActivated = Callback<GameOverlayActivated_t>.Create(OnGameOverlayActivated);

                _fileReadResult = CallResult<RemoteStorageFileReadAsyncComplete_t>.Create(OnFileReadCompleted);

                _fileWriteResult = CallResult<RemoteStorageFileWriteAsyncComplete_t>.Create(OnFileWriteCompleted);

                SteamUserStats.RequestCurrentStats();
                //SteamUserStats.ResetAllStats(true);
                //SteamUserStats.StoreStats();

                //bool result = SteamRemoteStorage.FileDelete(Constants.SAVE_PATH);
            }
        }

        public bool UnlockAchievement(AchievementEnum achievement)
        {
            var isAchievementUnlocked = false;
            var achievementID = achievement.ToString();

            if (SteamUserStats.GetAchievement(achievementID, out isAchievementUnlocked))
            {
                if (!isAchievementUnlocked)
                {
                    var success = SteamUserStats.SetAchievement(achievementID);
                    if (success)
                    {
                        if (Game1.SaveData != null)
                        {
                            Game1.SaveData.AddAchievement(achievement);
                        }

                        SteamUserStats.StoreStats();

                        return true;
                    }
                }
                else
                {
                    if (Game1.SaveData != null)
                    {
                        Game1.SaveData.AddAchievement(achievement);
                    }

                    return true;
                }
            }

            return false;
        }

        bool DestroyIfNull()
        {
            if (Game1.SteamManager == null)
            {
                return true;
            }

            return false;
        }
    }
}
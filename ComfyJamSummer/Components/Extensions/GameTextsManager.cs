using System;
using System.Collections.Generic;
using System.Linq;
using ComfyJamSummer.Data.Jsons;
using ComfyJamSummer.Enums;
using ComfyJamSummer.Helpers;
using ComfyJamSummer.JsonsData.Xnb;
using Nez;

namespace ComfyJamSummer.Components.Extensions
{
    public class GameTextsManager : SceneComponent
    {
        private GameLanguage _language = GameLanguage.English;

        public override void OnEnabled()
        {
            base.OnEnabled();
        }

        public GameTextJsonXnbData<GameTextJsonData> GetXnbData(GameTextArchiveName name)
        {
            GameTextJsonXnbData<GameTextJsonData> xnbData = null;

            var prefabs = UtilHelper.Prefabs();

            if (prefabs != null)
            {
                xnbData = prefabs.GameTextJsonData.FirstOrDefault(x => x.ArchiveName == name);
            }

            return xnbData;
        }

        public List<GameText> GetAllValuesByNameAndLanguage(GameTextFilter filter)
        {
            var xnbData = GetXnbData(filter.ArchiveName);

            UpdateLanguage();

            var result = new List<GameText>();

            var prefabs = UtilHelper.Prefabs();

            if (prefabs != null)
            {
                var text = xnbData.Values.FirstOrDefault(x => x.Language == _language &&
                (string.IsNullOrEmpty(filter.Name) || CompareString(x.Name, filter.Name)));

                var clones = new List<GameText>();

                text.Texts.ForEach(x => { clones.Add(x.Clone() as GameText); });

                result.AddRange(clones);
            }

            return result;
        }

        public List<GameText> GetAllByType(GameTextJsonXnbData<GameTextJsonData> xnbData, string type)
        {
            UpdateLanguage();

            var result = new List<GameText>();

            if (xnbData != null)
            {
                var filtered = xnbData.Values.FirstOrDefault(x => x.Language == _language && CompareString(x.Type, type));

                if (filtered != null)
                {
                    filtered.Texts.ForEach(x => { result.Add(x.Clone() as GameText); });
                }
            }

            return result;
        }

        public List<GameText> GetListByNameType(GameTextJsonXnbData<GameTextJsonData> xnbData, string name, string type)
        {
            UpdateLanguage();

            var result = new List<GameText>();

            if (xnbData != null)
            {
                var filtered = xnbData.Values.Where(x => x.Language == _language && CompareString(x.Name, name) && CompareString(x.Type, type));

                if (filtered != null)
                {
                    foreach (var item in filtered)
                    {
                        item.Texts.ForEach(x => { result.Add(x.Clone() as GameText); });
                    }
                }
            }

            return result;
        }

        public List<GameText> GetAllByName(GameTextJsonXnbData<GameTextJsonData> xnbData, string name)
        {
            UpdateLanguage();

            var result = new List<GameText>();

            if (xnbData != null)
            {
                var filtered = xnbData.Values.FirstOrDefault(x => x.Language == _language && CompareString(x.Name, name));

                if (filtered != null)
                {
                    filtered.Texts.ForEach(x => { result.Add(x.Clone() as GameText); });
                }
            }

            return result;
        }

        public List<GameText> GetAll(GameTextJsonXnbData<GameTextJsonData> xnbData)
        {
            UpdateLanguage();

            var result = new List<GameText>();

            if (xnbData != null)
            {
                var filtered = xnbData.Values.FirstOrDefault(x => x.Language == _language);

                if (filtered != null)
                {
                    filtered.Texts.ForEach(x => { result.Add(x.Clone() as GameText); });
                }
            }

            return result;
        }

        public string GetGameTextByValueOfName(List<GameText> list, string name)
        {
            UpdateLanguage();

            var result = string.Empty;

            if (list != null)
            {
                var filtered = list.FirstOrDefault(x => CompareString(x.Name, name));

                if (filtered != null)
                {
                    return filtered.Value;
                }
            }

            return result;
        }

        private void UpdateLanguage()
        {
            if (Game1.SaveData != null)
            {
                _language = Game1.SaveData.ChosenLanguage;
            }
        }

        private bool CompareString(string obj1, string obj2)
        {
            if (string.IsNullOrEmpty(obj1) || string.IsNullOrEmpty(obj2))
            {
                return false;
            }

            return obj1.Equals(obj2, StringComparison.InvariantCultureIgnoreCase);
        }
    }
}
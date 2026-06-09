using System;
using System.Collections.Generic;
using ComfyJamSummer.Enums;

namespace ComfyJamSummer.Data.Jsons
{
    public class GameTextJsonData
    {
        public string Name { get; set; }

        public string Type { get; set; }

        public GameLanguage Language { get; set; }

        public List<GameText> Texts { get; set; }
    }

    public class GameText : ICloneable
    {
        public string Name { get; set; }

        public string Value { get; set; }

        public string Order { get; set; }

        public string Subject { get; set; }

        public object Clone()
        {
            return this.MemberwiseClone() as GameText;
        }
    }

    public class GameTextFilter
    {
        public GameTextArchiveName ArchiveName { get; set; }

        public string Name { get; set; }
        public string Type { get; set; }

        public GameLanguage Language { get; set; }

        public string NameText { get; set; }
        public string ValueText { get; set; }

    }
}
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Nez.BitmapFonts;
using Nez.UI;

namespace ComfyJamSummer.UI.CustomLabels
{
    public class MultiLabel
    {
        public object UserData { get; set; }
        public List<Label> Labels { get; set; }

        public MultiLabel(BitmapFont font, Color color, string text, int maxWidth, string prefix = null)
        {
            Labels = new List<Label>();

            var wrappedText = font.WrapText(text, maxWidth);

            var splittedText = wrappedText.Split("\n");

            if (splittedText != null && splittedText.Any())
            {
                foreach (var line in splittedText)
                {
                    Labels.Add(new Label($"{prefix} {line}", new LabelStyle(font, color)));
                }
            }
        }

        public MultiLabel(BitmapFont font, Color color, string text, string prefix = null)
        {
            Labels = new List<Label>();

            var splittedText = text.Split("\n");

            if (splittedText != null && splittedText.Any())
            {
                foreach (var line in splittedText)
                {
                    Labels.Add(new Label($"{prefix} {line}", new LabelStyle(font, color)));
                }
            }
        }
    }
}
using Microsoft.Xna.Framework;
using Nez.UI;

namespace ComfyJamSummer.UI.CustomElements
{
    public class SimpleElement
    {
        public Element Instance { get; set; }

        public Vector2 OriginalSize { get; }

        public SimpleElement(Element element)
        {
            Instance = element;
            OriginalSize = new Vector2(element.PreferredWidth, element.PreferredHeight);
        }
    }
}
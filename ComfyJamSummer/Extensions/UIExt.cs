using Microsoft.Xna.Framework;
using Nez.UI;

namespace ComfyJamSummer.Extensions
{
    public static class UIExt
    {
        public static Vector2 GetElementPosition(this Cell cell)
        {
            if (cell == null)
            {
                return Vector2.Zero;
            }

            var table = cell.GetTable();

            if (table == null)
            {
                return Vector2.Zero;
            }

            var element = cell.GetElement<Element>();

            var width = element.Width();
            var height = element.Height();

            var halfWidth = width / 2;
            var halfHeight = height / 2;

            if (element.GetOriginX() <= 0)
            {
                element.SetOriginX(halfWidth);
                element.SetOriginY(halfHeight);
            }

            var x = element.GetX();
            var y = element.GetY();

            var origin = new Vector2(halfWidth, halfHeight);

            return new Vector2(table.GetX() + (x + origin.X), table.GetY() + (y + origin.Y));
        }

        public static float Width(this Element elem)
        {
            return elem.GetWidth() > 0 ? elem.GetWidth() : elem.PreferredWidth;
        }

        public static float Height(this Element elem)
        {
            return elem.GetHeight() > 0 ? elem.GetHeight() : elem.PreferredHeight;
        }
    }
}
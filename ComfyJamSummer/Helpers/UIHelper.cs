using System;
using System.Collections.Generic;
using System.Linq;
using ComfyJamSummer.Enums;
using ComfyJamSummer.Helpers;
using ComfyJamSummer.UI.CustomElements;
using ComfyJamSummer.UI.CustomImages;
using ComfyJamSummer.UI.Data;
using ComfyJamSummer.UI.Enums;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Nez;
using Nez.BitmapFonts;
using Nez.Textures;
using Nez.UI;
using static ComfyJamSummer.UI.CustomImages.ShakableImageButton;
using Image = Nez.UI.Image;
using Label = Nez.UI.Label;
using Screen = Nez.Screen;

namespace ComfyJamSummer.Extensions
{
    public static class UIHelper
    {
        private static int _extraCollisionSpaceForCursor = 8;

        public static void KeepRescaledImageOnSamePosition(CustomImage img, float scale)
        {
            var updatedScale = new Vector2(scale, scale);

            var newWidth = img.OriginalWidth * updatedScale.X;
            var newHeight = img.OriginalHeight * updatedScale.Y;

            var offsetX = (img.OriginalWidth - newWidth) * 0.5f;
            var offsetY = (img.OriginalHeight - newHeight) * 0.5f;

            img.SetPosition(img.OriginalPosition.X + offsetX, img.OriginalPosition.Y + offsetY);
        }

        public static void CentralizeElementInScreen(this Element element)
        {
            if (element == null)
            {
                return;
            }

            var width = element.GetWidth() > 0 ? element.GetWidth() : element.PreferredWidth;
            var height = element.GetHeight() > 0 ? element.GetHeight() : element.PreferredHeight;

            element.SetOrigin(width / 2, height / 2);

            element.SetPosition((Screen.Width / 2) - element.GetOriginX(), (Screen.Height / 2) - element.GetOriginY());
        }

        public static Vector2 CentralizeElementPosXInScreen(this Element element, float posY = 0)
        {
            if (element == null)
            {
                return Vector2.Zero;
            }

            var width = element.GetWidth() > 0 ? element.GetWidth() : element.PreferredWidth;
            var height = element.GetHeight() > 0 ? element.GetHeight() : element.PreferredHeight;

            element.SetOrigin(width / 2, height / 2);

            var positionY = posY == 0 ? element.GetY() : posY;

            var position = new Vector2((Screen.Width / 2) - element.GetOriginX(), positionY - element.GetOriginY());

            element.SetPosition(position.X, position.Y);

            return position;
        }

        public static void CentralizeElementPosYInScreen(this Element element, float posX = 0)
        {
            if (element == null)
            {
                return;
            }

            var width = element.GetWidth() > 0 ? element.GetWidth() : element.PreferredWidth;
            var height = element.GetHeight() > 0 ? element.GetHeight() : element.PreferredHeight;

            element.SetOrigin(width / 2, height / 2);

            var positionX = posX == 0 ? element.GetX() : posX;

            element.SetPosition(positionX - element.GetOriginX(), (Screen.Height / 2) - element.GetOriginY());
        }


        public static float GetScaleForUIElements()
        {
            var scale = (float)Math.Round(UtilHelper.GetGameScale());

            scale = Mathf.Clamp(scale, 1, 4);

            return scale;
        }


        public static void RescaleUIElementSize(Element element)
        {
            if (element == null)
            {
                return;
            }

            var scale = UtilHelper.GetGameScaleForUI();

            var maxScale = scale;

            scale = Mathf.Clamp(scale, 1, maxScale);

            var newWidth = element.PreferredWidth * scale;
            var newHeight = element.PreferredHeight * scale;

            element.SetSize(newWidth, newHeight);
        }

        public static bool CursorCollidedWithButton(Vector2 cursorPosition, Button targetElement)
        {
            if (targetElement == null)
            {
                return false;
            }
            var width = targetElement.GetWidth();

            var height = targetElement.GetHeight();

            if (width == 0)
            {
                width = targetElement.PreferredWidth;
            }

            if (height == 0)
            {
                height = targetElement.PreferredHeight;
            }

            return (cursorPosition.X >= (targetElement.GetX()) && cursorPosition.X <= (targetElement.GetX() + width)) &&
                (cursorPosition.Y >= (targetElement.GetY()) && cursorPosition.Y <= (targetElement.GetY() + height));
        }

        public static bool CursorCollidedWithImg(Vector2 cursorPosition, Image targetElement)
        {
            if (targetElement == null)
            {
                return false;
            }

            return (cursorPosition.X >= (targetElement.GetX()) && cursorPosition.X <= (targetElement.GetX() + targetElement.GetWidth())) &&
                (cursorPosition.Y >= (targetElement.GetY()) && cursorPosition.Y <= (targetElement.GetY() + targetElement.GetHeight()));
        }

        public static bool ClickedInImage(Vector2 cursorPosition, Image targetElement)
        {
            if (targetElement == null)
            {
                return false;
            }

            return CursorCollidedWithImg(cursorPosition, targetElement) && Input.LeftMouseButtonPressed;
        }


        public static bool CursorCollidedWithLabel(Vector2 cursorPosition, Label lblTarget)
        {
            if (lblTarget == null)
            {
                return false;
            }

            return (cursorPosition.X >= (lblTarget.GetX() - _extraCollisionSpaceForCursor) && cursorPosition.X <= (lblTarget.GetX() + lblTarget.PreferredWidth + _extraCollisionSpaceForCursor)) &&
                        (cursorPosition.Y >= (lblTarget.GetY() - _extraCollisionSpaceForCursor) && cursorPosition.Y <= (lblTarget.GetY() + lblTarget.PreferredHeight + _extraCollisionSpaceForCursor));
        }

        public static bool CursorImageCollidedWithRectangle(Image img, Rectangle rect)
        {
            if (img == null)
            {
                return false;
            }

            var x = img.GetX() + img.PreferredWidth / 2;

            return (x >= rect.X && x <= (rect.X + rect.Width)) &&
                (img.GetY() >= rect.Y && img.GetY() <= (rect.Y + rect.Height));
        }

        public static void RemoveFromContainer(Container container, ElementUserData elementUserData)
        {
            if (container == null)
            {
                return;
            }

            var children = container.GetChildren();

            for (int i = children.Count - 1; i >= 0; i--)
            {
                var child = children[i];

                if (child.UserData != null && child.UserData.GetType() == typeof(ElementUserData))
                {
                    var userData = child.UserData as ElementUserData;

                    if (userData != null)
                    {
                        if (userData.Id == elementUserData.Id && userData.ElementClassType == elementUserData.ElementClassType)
                        {
                            children.RemoveAt(i);
                            child.Remove();
                        }
                    }
                }
            }
        }

        public static void RemoveFromContainer(Group container, Element element)
        {
            if (container == null || element == null)
            {
                return;
            }

            var children = container.GetChildren();

            for (int i = children.Count - 1; i >= 0; i--)
            {
                var child = children[i];
                if (element == child)
                {
                    children.RemoveAt(i);
                    child.Remove();
                }
            }
        }

        public static void RemoveEverythingFromContainer(Container container)
        {
            if (container == null)
            {
                return;
            }

            var children = container.GetChildren();

            for (int i = children.Count - 1; i >= 0; i--)
            {
                var child = children[i];

                children.RemoveAt(i);
                child.Remove();
            }
        }

        public static TextButtonStyle CreateTextButtonStyle(BitmapFont font)
        {
            var txtButtonStyle = new TextButtonStyle();
            txtButtonStyle.PressedOffsetX = 2f;
            txtButtonStyle.PressedOffsetY = 2f;
            txtButtonStyle.Font = font;
            txtButtonStyle.OverFontColor = Color.DarkGray;
            txtButtonStyle.FontColor = Constants.WHITE_COLOR;

            return txtButtonStyle;
        }

        public static float GetTitleUIPosY()
        {
            return Screen.Height * 0.14f;
        }

        public static float GetBtnBackUIPosY()
        {
            return Screen.Height * 0.94f;
        }

        public static ImageButtonStyle CreateImageButtonStyle(UISprite textureName, float upAlpha = 0.15f, float overAlpha = 0.35f)
        {
            var prefabs = UtilHelper.Prefabs();

            if (prefabs == null)
            {
                return new ImageButtonStyle();
            }

            var texture = prefabs.GetUITexture(textureName);

            return CreateDefaultImageButtonStyle(texture, upAlpha, overAlpha);
        }

        public static ImageButtonStyle CreateDefaultAlphaImageButtonStyle(Texture2D texture, float upAlpha = 1f, float overAlpha = 1f)
        {
            var prefabs = UtilHelper.Prefabs();

            if (prefabs == null)
            {
                return new ImageButtonStyle();
            }

            return CreateDefaultImageButtonStyle(texture, upAlpha, overAlpha);
        }

        private static ImageButtonStyle CreateDefaultImageButtonStyle(Texture2D texture, float upAlpha = 0.15f, float overAlpha = 0.35f)
        {
            var sprite = new Sprite(texture);

            var drawableUp = new SpriteDrawable(sprite);
            drawableUp.Alpha = upAlpha;

            var drawableDown = new SpriteDrawable(sprite);

            var drawableOver = new SpriteDrawable(sprite);
            drawableOver.Alpha = overAlpha;

            return new ImageButtonStyle()
            {
                Up = drawableUp,
                Down = drawableDown,
                Over = drawableOver,
                PressedOffsetX = 1f,
                PressedOffsetY = 1f,
            };
        }

        public static void ProcessShaking(Element element, float shakeFrequency = 1.5f, float shakeAmplitude = 0.045f)
        {
            if (element == null)
            {
                return;
            }

            if (shakeFrequency <= 0)
            {
                return;
            }

            var time = (Time.TotalTime) * shakeFrequency;
            var offsetX = shakeAmplitude * (float)Math.Sin(time);
            var offsetY = shakeAmplitude * (float)Math.Cos(time);

            var shakeOffset = new Vector2(offsetX, offsetY);

            element.SetPosition(element.GetX() + shakeOffset.X, element.GetY() + shakeOffset.Y);
        }

        public static void ProcessFloating(ShakableImageButton element, float shakeAmplitude = 0.045f)
        {
            if (element == null)
            {
                return;
            }

            if (element.StartFloatingDelay > 0)
            {
                element.StartFloatingDelay -= Time.DeltaTime;

                return;
            }

            var shakeOffset = GetShakeOffset(element.FloatFrequency, shakeAmplitude);

            element.SetPosition(element.GetX() + shakeOffset.X, element.GetY() + shakeOffset.Y);
        }

        public static void ProcessFloating(AttachedLabel element, float shakeAmplitude = 0.045f)
        {
            if (element == null)
            {
                return;
            }

            if (element.StartFloatingDelay > 0)
            {
                element.StartFloatingDelay -= Time.DeltaTime;

                return;
            }

            var shakeOffset = GetShakeOffset(element.FloatFrequency, shakeAmplitude);

            element.SetPosition(element.GetX() + shakeOffset.X, element.GetY() + shakeOffset.Y);
        }

        private static Vector2 GetShakeOffset(float floatFrequency, float shakeAmplitude)
        {
            var time = (Time.TotalTime) * floatFrequency;
            var offsetX = shakeAmplitude * (float)Math.Sin(time);
            var offsetY = shakeAmplitude * (float)Math.Cos(time);

            return new Vector2(offsetX, offsetY);
        }

        public static void ReAddElementInGroup(Element elementToRemove, Group group, bool value)
        {
            if (group == null || elementToRemove == null)
            {
                return;
            }

            if (elementToRemove != null)
            {
                var children = group.GetChildren();

                for (int i = children.Count - 1; i >= 0; i--)
                {
                    var child = children[i];

                    if (child == elementToRemove)
                    {
                        children.RemoveAt(i);
                        child.Remove();
                    }
                }

                if (value)
                {
                    group.AddElement(elementToRemove);
                }
            }
        }

        public static UICanvas GetCanvas(string entityName)
        {
            var statsUI = Core.Scene.FindEntity(entityName);

            if (statsUI != null)
            {
                return statsUI.GetComponent<UICanvas>();
            }

            return null;
        }

        public static void ConnectButtonNeighborsTable(Table table, Button btnTitle, Button btnBack)
        {
            var cells = table.GetCells();

            for (int i = 0; i < cells.Count; i++)
            {
                var cell = cells[i];
                Button btnUpNeighbor = null;
                Button btnRightNeighbor = null;
                Button btnDownNeighbor = null;
                Button btnLeftNeighbor = null;

                var button = cell.GetElement<Button>();

                var horizontalGroup = cell.GetElement<HorizontalGroup>();

                if (horizontalGroup != null)
                {
                    var childrenHorizontal = horizontalGroup.GetChildren();

                    if (childrenHorizontal == null)
                    {
                        continue;
                    }

                    for (int j = 0; j < childrenHorizontal.Count; j++)
                    {
                        var btnChild = childrenHorizontal[j] as Button;

                        if (btnChild == null)
                        {
                            continue;
                        }

                        var indexLeftNeighbor = j - 1;
                        var indexRightNeighbor = j + 1;

                        while (indexLeftNeighbor >= 0 && btnChild.GamepadLeftElement == null)
                        {
                            var leftNeighborElement = childrenHorizontal[indexLeftNeighbor];

                            var buttonLeftNeighbor = leftNeighborElement as Button;

                            btnChild.ChangeGamePadFocusable(buttonLeftNeighbor, GenericDirection.Left);

                            indexLeftNeighbor--;
                        }

                        while (indexRightNeighbor <= childrenHorizontal.Count - 1 && btnChild.GamepadRightElement == null)
                        {
                            var rightNeighborElement = childrenHorizontal[indexRightNeighbor];

                            var buttonRightNeighbor = rightNeighborElement as Button;

                            btnChild.ChangeGamePadFocusable(buttonRightNeighbor, GenericDirection.Right);

                            indexRightNeighbor++;
                        }

                        btnDownNeighbor = (cell.GetRow() >= table.GetRows() - 1) ? btnBack : GetNeighborButtonOnTable(cells, cell, cell.GetColumn(), cell.GetRow() + 1);

                        if (btnTitle != null)
                        {
                            btnUpNeighbor = cell.GetRow() == 0 ? btnTitle : GetNeighborButtonOnTable(cells, cell, cell.GetColumn(), cell.GetRow() - 1);

                            if (cell.GetRow() == 0)
                            {
                                btnTitle.ChangeGamePadFocusable(btnChild, GenericDirection.Bottom);
                            }
                        }
                        else
                        {
                            btnUpNeighbor = cell.GetRow() == 0 ? null : GetNeighborButtonOnTable(cells, cell, cell.GetColumn(), cell.GetRow() - 1);
                        }

                        btnChild.ChangeGamePadFocusable(btnUpNeighbor, GenericDirection.Top);

                        btnChild.ChangeGamePadFocusable(btnDownNeighbor, GenericDirection.Bottom);

                        if (btnDownNeighbor != null)
                        {
                            btnDownNeighbor.ChangeGamePadFocusable(btnChild, GenericDirection.Top);
                        }

                        if (btnUpNeighbor != null)
                        {
                            btnUpNeighbor.ChangeGamePadFocusable(btnChild, GenericDirection.Bottom);
                        }

                        if (btnBack != null && cell.GetRow() >= table.GetRows() - 1)
                        {
                            btnBack.ChangeGamePadFocusable(btnChild, GenericDirection.Top, true);
                        }
                    }
                }
                else
                {
                    if (button == null)
                    {
                        continue;
                    }

                    btnUpNeighbor = GetNeighborButtonOnTable(cells, cell, cell.GetColumn(), cell.GetRow() - 1);

                    btnRightNeighbor = FindHorizontalNeighbor(cells, cell);

                    btnLeftNeighbor = FindHorizontalNeighbor(cells, cell, false);

                    btnDownNeighbor = (cell.GetRow() >= table.GetRows()) ? btnBack : GetNeighborButtonOnTable(cells, cell, cell.GetColumn(), cell.GetRow() + 1);

                    button.ChangeGamePadFocusable(btnUpNeighbor, GenericDirection.Top);
                    button.ChangeGamePadFocusable(btnDownNeighbor, GenericDirection.Bottom);
                    button.ChangeGamePadFocusable(btnRightNeighbor, GenericDirection.Right);
                    button.ChangeGamePadFocusable(btnLeftNeighbor, GenericDirection.Left);

                    if (btnTitle != null && cell.GetRow() == 0)
                    {
                        btnTitle.ChangeGamePadFocusable(button, GenericDirection.Bottom);

                        button.ChangeGamePadFocusable(btnTitle, GenericDirection.Top);
                    }

                    if (btnBack != null && cell.GetRow() >= table.GetRows() - 1)
                    {
                        btnBack.ChangeGamePadFocusable(button, GenericDirection.Top, true);
                    }
                }
            }
        }

        private static Button FindHorizontalNeighbor(List<Cell> cells, Cell cell, bool increment = true)
        {
            Button possibleNeighbor = null;

            var found = false;

            var index = 1;

            while (index < cells.Count && !found)
            {
                possibleNeighbor = GetNeighborButtonOnTable(cells, cell, increment ? cell.GetColumn() + index : cell.GetColumn() - index, cell.GetRow());

                if (possibleNeighbor != null)
                {
                    found = true;
                }

                index++;
            }

            return possibleNeighbor;
        }

        public static void ChangeGamePadFocusable(this Button button, IGamepadFocusable newFocusable, GenericDirection direction, bool ignoreNullCondition = false)
        {
            switch (direction)
            {
                case GenericDirection.Top:
                    if (button.GamepadUpElement == null || ignoreNullCondition)
                    {
                        button.GamepadUpElement = newFocusable;

                        button.ShouldUseExplicitFocusableControl = true;
                    }
                    break;

                case GenericDirection.Right:
                    if (button.GamepadRightElement == null || ignoreNullCondition)
                    {
                        button.GamepadRightElement = newFocusable;

                        button.ShouldUseExplicitFocusableControl = true;
                    }
                    break;

                case GenericDirection.Left:
                    if (button.GamepadLeftElement == null || ignoreNullCondition)
                    {
                        button.GamepadLeftElement = newFocusable;

                        button.ShouldUseExplicitFocusableControl = true;
                    }
                    break;

                case GenericDirection.Bottom:
                    if (button.GamepadDownElement == null || ignoreNullCondition)
                    {
                        button.GamepadDownElement = newFocusable;

                        button.ShouldUseExplicitFocusableControl = true;
                    }
                    break;
            }
        }

        private static Button GetNeighborButtonOnTable(List<Cell> cells, Cell cell, int column, int row)
        {
            var neighbor = cells.FirstOrDefault(x => x.GetColumn() == column && x.GetRow() == row);

            if (neighbor != null)
            {
                return neighbor.GetElement<Button>();
            }

            return null;
        }

        public static Button SetFocusOnFirstButton(Table table, Stage stage)
        {
            if (stage == null)
            {
                return null;
            }

            if (table == null)
            {
                return null;
            }

            var children = table.GetChildren();

            if (children == null)
            {
                return null;
            }

            if (!children.Any())
            {
                return null;
            }

            var firstElement = children.OrderBy(x => x.GetY()).FirstOrDefault(x => x.GetType().IsSubclassOf(typeof(Button)));

            if (firstElement != null)
            {
                var firstButton = firstElement as Button;

                stage.SetGamepadFocusElement(firstButton);

                return firstButton;
            }

            return null;
        }

        public static void CreateHorizontalVerticalNeighbors(int i, List<Button> list, bool isVertical = false)
        {
            Button previousPopup = null, nextPopup = null;

            var previousIndex = i - 1;
            var nextIndex = i + 1;

            if (previousIndex >= 0)
            {
                previousPopup = list[previousIndex];
            }

            if (nextIndex < list.Count)
            {
                nextPopup = list[nextIndex];
            }

            var popup = list[i];

            popup.ShouldUseExplicitFocusableControl = true;

            if (previousPopup != null)
            {
                if (isVertical)
                {
                    previousPopup.GamepadDownElement = popup;
                }
                else
                {
                    previousPopup.GamepadRightElement = popup;
                }
            }

            if (nextPopup != null)
            {
                if (isVertical)
                {
                    nextPopup.GamepadUpElement = popup;
                }
                else
                {
                    nextPopup.GamepadLeftElement = popup;
                }
            }

            if (isVertical)
            {
                popup.GamepadUpElement = previousPopup;
                popup.GamepadDownElement = nextPopup;
            }
            else
            {
                popup.GamepadLeftElement = previousPopup;
                popup.GamepadRightElement = nextPopup;
            }
        }

        public static List<Element> GetAllChildren(Stage stage)
        {
            var list = new List<Element>();

            void Collect(Element e)
            {
                list.Add(e);

                if (e is Group g)
                {
                    foreach (var child in g.GetChildren())
                        Collect(child);
                }
            }

            var allElements = stage.GetElements();

            foreach (var root in allElements)
                Collect(root);

            return list;
        }

        public static bool RemoveDamageTakenElementFromContainer(Container container, uint id, List<EnemyHealthBarImage> list = null, bool isBoss = false)
        {
            if (container == null)
            {
                return false;
            }

            var children = container.GetChildren();
            var removed = false;
            for (int i = children.Count - 1; i >= 0; i--)
            {
                var child = children[i];

                if (child.UserData != null)
                {
                    if (child.UserData.GetType() != typeof(EnemyUIData))
                    {
                        continue;
                    }

                    var enemyData = child.UserData as EnemyUIData;

                    if (enemyData != null && enemyData.EnemyId == id)
                    {
                        if (child.GetType() == typeof(EnemyHealthBarImage))
                        {
                            var healthBar = child as EnemyHealthBarImage;

                            if (healthBar != null && healthBar.Name == HealthBarTypeEnum.DamageTaken)
                            {
                                children.RemoveAt(i);
                                child.Remove();

                                if (list != null)
                                {
                                    list.Remove(healthBar);
                                }

                                removed = true;
                            }
                        }
                    }
                }
            }

            return removed;
        }
    }
}
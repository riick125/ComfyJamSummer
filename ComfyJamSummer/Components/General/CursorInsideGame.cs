using ComfyJamSummer.Helpers;
using ComfyJamSummer.Scenes;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Nez;

namespace ComfyJamSummer.Components.General
{
    public class CursorInsideGame : SceneComponent
    {
        int _windowWidth, _windowHeight;

        public CursorInsideGame(int windowWidth, int windowHeight)
        {
            _windowWidth = windowWidth;
            _windowHeight = windowHeight;
        }

        public override void Update()
        {
            base.Update();

#if DEBUG
            return;
#endif

            var gameManager = UtilHelper.GameManager();

            if (gameManager != null && Core.Scene != null && Core.Scene is InGameScene)
            {
                var lockCursor = false;

                var actualScene = Core.Scene;

                if (actualScene is InGameScene)
                {
                    lockCursor = !gameManager.IsGamePaused;
                }
                else
                {
                    lockCursor = true;
                }

                if (lockCursor)
                {
                    MouseState mouseState = Mouse.GetState();
                    Point relativeCursorPos = new Point(mouseState.X, mouseState.Y);
                    Point relativeCursorPosCache = relativeCursorPos;

                    if (relativeCursorPos.X < 0)
                        relativeCursorPos.X = 1;
                    else if (relativeCursorPos.X > _windowWidth)
                        relativeCursorPos.X = _windowWidth - 1;

                    if (relativeCursorPos.Y < 0)
                        relativeCursorPos.Y = 1;
                    else if (relativeCursorPos.Y > _windowHeight)
                        relativeCursorPos.Y = _windowHeight - 1;

                    if (relativeCursorPos != relativeCursorPosCache)
                        Mouse.SetPosition(relativeCursorPos.X, relativeCursorPos.Y);
                }
            }
        }

        public void UpdateWindowBounds(int width, int height)
        {
            if (width <= 0 || height <= 0)
            {
                return;
            }

            _windowWidth = width;
            _windowHeight = height;
        }
    }
}
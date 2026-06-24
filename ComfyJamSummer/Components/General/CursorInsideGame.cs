using ComfyJamSummer.Helpers;
using ComfyJamSummer.Manager;
using ComfyJamSummer.Scenes;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Nez;

namespace ComfyJamSummer.Components.General
{
    public class CursorInsideGame : SceneComponent
    {
        int _windowWidth, _windowHeight;

        int _safeDistanceX, _safeDistanceY;

        GameManager _manager;

        public CursorInsideGame(int windowWidth, int windowHeight)
        {
            _windowWidth = windowWidth;
            _windowHeight = windowHeight;

            SetSafeDistance();
        }

        public override void OnEnabled()
        {
            base.OnEnabled();

            _manager = UtilHelper.GameManager();
        }

        public override void Update()
        {
            base.Update();

            if(!Core.Instance.IsActive)
            {
                return;
            }

            if (_manager != null && _manager.IsGamePaused)
            {
                Core.Instance.IsMouseVisible = true;
                return;
            }

            Core.Instance.IsMouseVisible = false;

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

                    if (relativeCursorPos.X < _safeDistanceX)
                        relativeCursorPos.X = 1;
                    else if (relativeCursorPos.X > _windowWidth - _safeDistanceX)
                        relativeCursorPos.X = _windowWidth - _safeDistanceX;

                    if (relativeCursorPos.Y < _safeDistanceY)
                        relativeCursorPos.Y = 1;
                    else if (relativeCursorPos.Y > _windowHeight - _safeDistanceY)
                        relativeCursorPos.Y = _windowHeight - _safeDistanceY;

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

            SetSafeDistance();
        }

        void SetSafeDistance()
        {
            _safeDistanceX = (int)(_windowWidth * 0.005f);
            _safeDistanceY = (int)(_windowHeight * 0.005f);
        }
    }
}
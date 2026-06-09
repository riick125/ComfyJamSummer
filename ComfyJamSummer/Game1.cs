using System;
using System.Diagnostics;
using System.IO;
using ComfyJamSummer.Components.Extensions;
using ComfyJamSummer.Components.General;
using ComfyJamSummer.Enums;
using ComfyJamSummer.Manager;
using ComfyJamSummer.Save;
using ComfyJamSummer.Scenes.Base;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Nez;

namespace ComfyJamSummer
{
    public class Game1 : Core
    {
        public const int ScreenSpaceRenderLayer = 999;

        public static CustomScene CustomScene;
        public static SteamManager SteamManager;
        public static SteamScript SteamScript;
        public static CursorInsideGame CursorInsideGame;
        public static AchievementManager AchievementManager;

        public static GameManager GameManager;
        public static SoundManager SoundManager;
        public static GameTextsManager TextManager;

        public static Material FlashMaterial;

        public static SaveData SaveData;

        public static SaveGameComponent SaveGameComponent;

        public static GameResolution ChosenResolution;

        public static float GameZoom = 2;

        public static float GameMaxZoom = 3;

        private uint _fps = 60;
        private Stopwatch stopwatch;
        private static double targetFrameTime;

        public Game1(GameResolution chosenResolution, int width = 1280, int height = 720, uint fps = 60, bool isFullScreen = false) : base(width, height, isFullScreen)
        {
            try
            {
                CreateDefaultProperties(chosenResolution, width, height, fps, isFullScreen);
            }
            catch (Exception ex)
            {
#if DEBUG
                throw ex;
#endif
            }
        }

        protected override void Initialize()
        {
            base.Initialize();
            try
            {
                //Game1.SteamManager = new SteamManager();
                stopwatch = new Stopwatch();
                stopwatch.Start();

                SetTargetFPS(_fps);

                LoadEffect();

                //DebugRenderEnabled = true;

                Core.Scene = new BaseScene();
            }
            catch (Exception ex)
            {
                var name = $"erro-log_{DateTime.Now}.txt";

                File.WriteAllText(name, ex.ToString());

#if DEBUG
                throw ex;
#endif
            }
        }

        protected override void Update(GameTime gameTime)
        {
            if (!IsActive)
            {
                return;
            }

            CursorInsideGame?.UpdateWindowBounds(Window.ClientBounds.Width, Window.ClientBounds.Height);

#if DEBUG
            if (Input.IsKeyPressed(Keys.Escape))
            {
                Exit();
            }
#endif

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);

            if (Game1.SaveData != null && !Game1.SaveData.IsVSyncEnabled)
            {
                LimitFPS();
            }
        }

        public static void SetTargetFPS(double fps)
        {
            targetFrameTime = 1000.0 / fps;
        }

        void CreateDefaultProperties(GameResolution chosenResolution, int width, int height, uint fps, bool isFullScreen)
        {
            AppDomain.CurrentDomain.UnhandledException += (sender, args) =>
            {
                var exception = (Exception)args.ExceptionObject;
                LogAndNotify(exception);
            };

            _fps = fps;
            Window.IsBorderless = false;
            PauseOnFocusLost = false;
            ChosenResolution = chosenResolution;

            IsMouseVisible = true;
            ExitOnEscapeKeypress = false;
            Batcher.UseFnaHalfPixelMatrix = false;
            Core.DefaultSamplerState = SamplerState.PointClamp;

            Screen.SynchronizeWithVerticalRetrace = false;
            IsFixedTimeStep = true;
        }

        void LimitFPS()
        {
            double elapsedMilliseconds = stopwatch.Elapsed.TotalMilliseconds;

            if (elapsedMilliseconds < targetFrameTime)
            {
                int sleepTime = (int)(targetFrameTime - elapsedMilliseconds);
                if (sleepTime > 0)
                {
                    System.Threading.Thread.Sleep(sleepTime);
                }
            }

            stopwatch.Restart();
        }

        void LoadEffect()
        {
#if WINDOWS
            FlashMaterial = new Material()
            {
                BlendState = BlendState.NonPremultiplied,
                Effect = new SpriteBlinkEffect()
                {
                    BlinkColor = Color.White,
                }
            };
#endif
        }

        void LogAndNotify(Exception exception)
        {
            try
            {
                string logPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "crashlog.txt");
                string logEntry = $"[{DateTime.Now:yyyy-MM-dd HH:mm:ss}] EXCEPTION:\n{exception}\n\n";

                File.AppendAllText(logPath, logEntry);
            }
            catch (Exception ex)
            {
#if DEBUG
                throw ex;
#endif
            }
        }
    }
}
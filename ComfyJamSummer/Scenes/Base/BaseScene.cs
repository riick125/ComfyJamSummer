using ComfyJamSummer.Components.General;
using ComfyJamSummer.Configs;
using ComfyJamSummer.Helpers;
using ComfyJamSummer.Manager;
using ComfyJamSummer.Prefab;
using ComfyJamSummer.UI;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Nez;

namespace ComfyJamSummer.Scenes.Base
{
    public class BaseScene : Scene, IFinalRenderDelegate
    {
        ScreenSpaceRenderer _screenSpaceRenderer;
        int width = 1280, height = 720;

        public BaseScene()
        {
            CreateWithDefaultRenderer();
        }

        public override void Initialize()
        {
            base.Initialize();

            ClearColor = Color.Black;

            if (Game1.SaveData == null)
            {
                SaveHelper.GetLocalSaveData();

                SetDesignResolution(width, height, SceneResolutionPolicy.ShowAllPixelPerfect);

                Screen.SetSize(width, height);
            }
            else
            {
                UtilHelper.SetResolution(this);
            }

            Screen.ApplyChanges();

            Game1.CursorInsideGame = AddSceneComponent(new CursorInsideGame(width, height));

            var prefabs = AddSceneComponent(new Prefabs());
            prefabs.FastLoad();

            var gameManager = AddSceneComponent(new GameManager());

            AddSceneComponent(new CustomFont(new CustomFontConfig()));

            CreateEntity(UINames.DEBUG).AddComponent(new DebugUI(gameManager, prefabs));
        }

        private Scene _scene;

        public void OnAddedToScene(Scene scene) => _scene = scene;

        public void OnSceneBackBufferSizeChanged(int newWidth, int newHeight) => _screenSpaceRenderer.OnSceneBackBufferSizeChanged(newWidth, newHeight);

        public void HandleFinalRender(RenderTarget2D finalRenderTarget, Color letterboxColor, RenderTarget2D source, Rectangle finalRenderDestinationRect, SamplerState samplerState)
        {
            Core.GraphicsDevice.SetRenderTarget(null);
            Core.GraphicsDevice.Clear(letterboxColor);
            Graphics.Instance.Batcher.Begin(BlendState.Opaque, samplerState, DepthStencilState.None, RasterizerState.CullNone, null);
            Graphics.Instance.Batcher.Draw(source, finalRenderDestinationRect, Color.White);
            Graphics.Instance.Batcher.End();

            _screenSpaceRenderer.Render(_scene);
        }
    }
}

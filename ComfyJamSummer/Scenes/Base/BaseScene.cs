using ComfyJamSummer.Components.Extensions;
using ComfyJamSummer.Components.General;
using ComfyJamSummer.Configs;
using ComfyJamSummer.Helpers;
using ComfyJamSummer.Manager;
using ComfyJamSummer.Prefab;
using ComfyJamSummer.Save;
using ComfyJamSummer.UI;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Nez;

namespace ComfyJamSummer.Scenes.Base
{
    public class BaseScene : Scene, IFinalRenderDelegate
    {
        protected GameManager _gameManager;

        protected Prefabs _prefabs;

        ScreenSpaceRenderer _screenSpaceRenderer;

        public BaseScene()
        {
            CreateWithDefaultRenderer();

            _screenSpaceRenderer = new ScreenSpaceRenderer(100, Game1.ScreenSpaceRenderLayer);
            _screenSpaceRenderer.ShouldDebugRender = false;
            FinalRenderDelegate = this;

            AddRenderer(new RenderLayerExcludeRenderer(0, Game1.ScreenSpaceRenderLayer));
        }

        public override void Initialize()
        {
            base.Initialize();

            ClearColor = Color.Black;

            var width = (int)(Constants.GAME_WIDTH * Constants.SCREEN_SIZE_MULTIPLIER);
            var height = (int)(Constants.GAME_HEIGHT * Constants.SCREEN_SIZE_MULTIPLIER);

            if (Game1.SaveData == null)
            {
                SaveHelper.SaveGame(new SaveData());

                SetDesignResolution(width, height, SceneResolutionPolicy.ShowAllPixelPerfect);

                Screen.SetSize(width, height);
            }
            else
            {
                UtilHelper.SetResolution(this);
            }

            //Game1.CursorInsideGame = AddSceneComponent(new CursorInsideGame(width, height));

            _prefabs = AddSceneComponent(new Prefabs());
            _prefabs.FastLoad();

            _gameManager = AddSceneComponent(new GameManager());

            AddSceneComponent(new CustomFont(new CustomFontConfig()));

            Game1.SoundManager = AddSceneComponent(new SoundManager());

#if DEBUG
            CreateEntity(UINames.DEBUG).AddComponent(new DebugUI(_gameManager, _prefabs));
#endif
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

using ComfyJamSummer.Components.Extensions;
using ComfyJamSummer.Components.Gameplay;
using ComfyJamSummer.Components.Visuals;
using ComfyJamSummer.CustomPostProcessors;
using ComfyJamSummer.Entities;
using ComfyJamSummer.Helpers;
using ComfyJamSummer.Scenes.Base;
using ComfyJamSummer.UI;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Nez;

namespace ComfyJamSummer.Scenes
{
    public class InGameScene : CustomScene
    {
        Island _island;

        Player _player;

        private SaturationPostProcessor _saturationPostProcessor;

        public SaturationPostProcessor SaturationPostProcessor { get => _saturationPostProcessor; set => _saturationPostProcessor = value; }

        public override void Initialize()
        {
            base.Initialize();

            _saturationPostProcessor = new SaturationPostProcessor(2, Constants.SATURATION_FACTOR_NORMAL);
            AddPostProcessor(_saturationPostProcessor);

            AddSceneComponent(new BesideTextRegistry());
        }

        public override void Begin()
        {
            base.Begin();

            _island = _prefabs.Island.CloneIsland(100, Vector2.Zero);

            if (_island != null)
            {
                AddEntity(_island);

                Camera.SetPosition(_island.CenterPosition());

                Camera.SetZoom(Game1.GameZoom);

                _island.AddComponent(new IslandBuilder(_gameManager, _prefabs));

                AddSceneComponent(new DepthComponent());

                CreateEntity(UINames.PLAYER).AddComponent(new PlayerUI(_gameManager, _prefabs));

                CreateEntity(UINames.CRAB).AddComponent(new CrabUI(_gameManager, _prefabs));

                CreateEntity(UINames.WAVE).AddComponent(new WaveUI(_gameManager, _prefabs));

                CreateEntity(UINames.PAUSE).AddComponent(new PauseUI(_gameManager, _prefabs));

                Camera.AddComponent(new CameraShake());
            }
        }

        public override void Update()
        {
            base.Update();

            if (_player == null)
            {
                _player = UtilHelper.Player();
            }

            if (_player != null && !_player.IsAlive)
            {
                if (Input.IsKeyPressed(Keys.R))
                {
                    Core.StartSceneTransition<FadeTransition>(new FadeTransition(() => new InGameScene()));
                }
            }
        }
    }
}
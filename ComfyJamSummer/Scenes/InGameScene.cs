using ComfyJamSummer.Components.Extensions;
using ComfyJamSummer.Components.Gameplay;
using ComfyJamSummer.Components.Visuals;
using ComfyJamSummer.CustomPostProcessors;
using ComfyJamSummer.Entities;
using ComfyJamSummer.Scenes.Base;
using ComfyJamSummer.UI;
using Microsoft.Xna.Framework;
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

            _island = _prefabs.Island.CloneIsland(15, Vector2.Zero);

            if (_island != null)
            {
                AddEntity(_island);

                Camera.SetPosition(_island.CenterPosition());

                Camera.SetZoom(Game1.GameZoom);

                _island.AddComponent(new IslandBuilder(_gameManager, _prefabs));

                AddSceneComponent(new DepthComponent());

                CreateEntity(UINames.PLAYER).AddComponent(new PlayerUI(_gameManager, _prefabs));
            }
        }
    }
}
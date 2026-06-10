using ComfyJamSummer.Components.Extensions;
using ComfyJamSummer.Components.Gameplay;
using ComfyJamSummer.CustomPostProcessors;
using ComfyJamSummer.Entities;
using ComfyJamSummer.Helpers;
using ComfyJamSummer.Scenes.Base;
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

            _island = _prefabs.Island.CloneIsland(15, Screen.Center);

            if (_island != null)
            {
                AddEntity(_island);

                Camera.SetPosition(_island.CenterPosition());

                Camera.SetZoom(Game1.GameZoom);

                _player = AddEntity(_prefabs.GetPlayer(_island.CenterPosition()));

                Camera.AddComponent(new FollowCamera(_player, Camera));

                CreateEntity("Battle").AddComponent(new BattleComponent(_island, _gameManager, _prefabs));
            }
        }
    }
}
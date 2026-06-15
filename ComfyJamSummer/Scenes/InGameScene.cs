using ComfyJamSummer.Components.Extensions;
using ComfyJamSummer.Components.Gameplay;
using ComfyJamSummer.Components.Visuals;
using ComfyJamSummer.CustomPostProcessors;
using ComfyJamSummer.Entities;
using ComfyJamSummer.Entities.Base;
using ComfyJamSummer.Helpers;
using ComfyJamSummer.Scenes.Base;
using Microsoft.Xna.Framework;
using Nez;

namespace ComfyJamSummer.Scenes
{
    public class InGameScene : CustomScene
    {
        Island _island;

        Rocket _rocket;

        Animated _stone;

        Crab _crab;

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
                var stonePosition = Vector2.Zero;

                AddEntity(_island);

                Camera.SetPosition(_island.CenterPosition());

                Camera.SetZoom(Game1.GameZoom);

                _rocket = AddEntity(_prefabs.Rocket.CloneRocket(_island.Id, _island.CenterPosition()));

                stonePosition = _rocket.Position - new Vector2(_rocket.SpriteWidth * 1.25f, 0);

                _player = AddEntity(_prefabs.GetPlayer(_island.Id,
                    _rocket.Position + new Vector2(_rocket.SpriteWidth * 1.25f, 0)));

                _stone = AddEntity(_prefabs.GetStone(stonePosition));

                _crab = AddEntity(_prefabs.GetCrab(_island.Id, stonePosition - new Vector2(0, _stone.SpriteHeight / 2)));

                Camera.AddComponent(new FollowCamera(_player, Camera));

                CreateEntity(EntityNames.BATTLE).AddComponent(new BattleComponent(_island, _gameManager, _prefabs));

                CreateEntity(EntityNames.COLLECTIBLE_SPAWNER).AddComponent(new CollectibleSpawner(_gameManager, _prefabs));

                AddSceneComponent(new DepthComponent());
            }
        }
    }
}
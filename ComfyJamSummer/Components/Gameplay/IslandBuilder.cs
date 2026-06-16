using ComfyJamSummer.Components.Visuals;
using ComfyJamSummer.Entities;
using ComfyJamSummer.Entities.Base;
using ComfyJamSummer.Enums;
using ComfyJamSummer.Extensions;
using ComfyJamSummer.Helpers;
using ComfyJamSummer.Manager;
using ComfyJamSummer.Prefab;
using Microsoft.Xna.Framework;
using Nez;

namespace ComfyJamSummer.Components.Gameplay
{
    public class IslandBuilder : BaseComponent
    {
        Island _island;

        Rocket _rocket;

        Animated _stone;

        Crab _crab;

        Player _player;

        public IslandBuilder(GameManager manager, Prefabs prefabs) : base(manager, prefabs)
        {
        }

        public override void OnAddedToEntity()
        {
            base.OnAddedToEntity();

            if (!Validate(this.Entity))
            {
                this.RemoveComponent();

                return;
            }

            _island = this.Entity as Island;

            if (_island == null)
            {
                this.RemoveComponent();

                return;
            }

            Init();
        }

        void Init()
        {
            CreateRocketAndPlayer();

            CreateCrab();

            CreateBreadBag();

            CreateColliders();

            CreateFinalComponents();
        }

        void CreateColliders()
        {
            var renderer = _island.Renderer;

            var tmxMap = renderer?.TiledMap;

            var collisionGroup = tmxMap?.GetObjectGroup(TiledLayerNames.WALLS);

            if (collisionGroup != null)
            {
                foreach (var obj in collisionGroup.Objects)
                {
                    var pos = _island.Position + new Vector2(obj.X, obj.Y);

                    var collider = _island.CreateCollider(physicsLayer: CollisionLayer.Map, pos.X, pos.Y, obj.Width, obj.Height);

                    Physics.AddCollider(collider);
                }
            }
        }

        void CreateRocketAndPlayer()
        {
            var horizontalSpacing = 0f;

            _rocket = _scene.AddEntity(_prefabs.Rocket.CloneRocket(_island.Id, _island.CenterPosition()));
            horizontalSpacing = _rocket.SpriteWidth * 1.25f;

            _player = _scene.AddEntity(_prefabs.GetPlayer(_island.Id,
                _rocket.Position + new Vector2(_rocket.SpriteWidth / 2, _rocket.SpriteHeight)));
        }

        void CreateCrab()
        {
            var stonePosition = Vector2.Zero;

            stonePosition = _rocket.Position - new Vector2(_rocket.SpriteWidth * 1.05f, 0);
            _stone = _scene.AddEntity(_prefabs.GetStone(stonePosition));
            _stone.Position += new Vector2(0, _stone.SpriteHeight * 0.9f);

            _crab = _scene.AddEntity(_prefabs.GetCrab(_island.Id, stonePosition - new Vector2(0, _stone.SpriteHeight / 8)));
        }

        void CreateBreadBag()
        {
            var pos = _island.GetCornerPosition("floor", Nez.Random.Chance(50) ? GenericDirectionPlus.TopLeft : GenericDirectionPlus.TopRight);

            var spacing = _rocket.SpriteWidth * 1.25f;

            pos = pos != default ? pos : _rocket.Position + new Vector2(spacing, _rocket.SpriteHeight / 8);

            _scene.AddEntity(_prefabs.BreadBag.CloneBread(_island.Id, pos));
        }

        void CreateFinalComponents()
        {
            _scene.CreateEntity(EntityNames.BATTLE).AddComponent(new BattleComponent(_island, _manager, _prefabs));

            _scene.CreateEntity(EntityNames.COLLECTIBLE_SPAWNER).AddComponent(new CollectibleSpawner(_manager, _prefabs));

            _scene.CreateEntity(EntityNames.WATER_BACKGROUND).AddComponent(new WaterBackground(_manager, _prefabs));

            if (_player != null)
            {
                _camera.AddComponent(new FollowCamera(_player, _camera));
            }
        }
    }
}
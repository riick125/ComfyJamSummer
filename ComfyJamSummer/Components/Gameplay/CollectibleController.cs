using ComfyJamSummer.Entities;
using ComfyJamSummer.Entities.Base;
using ComfyJamSummer.Entities.Collectibles;
using ComfyJamSummer.Helpers;
using ComfyJamSummer.Manager;
using ComfyJamSummer.Prefab;
using Microsoft.Xna.Framework;
using Nez;

namespace ComfyJamSummer.Components.Gameplay
{
    public class CollectibleController : BaseComponent, IUpdatable
    {
        private GameManager _gameManager;

        private float _crawlSpeedPercentage = 0.035f;
        private float _crawlFastSpeedPercentage = 2.2f;

        Collectible _collectible;

        private Player _player;

        public CollectibleController(GameManager manager, Prefabs prefabs) : base(manager, prefabs)
        {
        }

        public override void OnAddedToEntity()
        {
            base.OnAddedToEntity();

            _collectible = this.Entity as Collectible;

            _player = UtilHelper.Player();
        }

        public void Update()
        {
            if (!Validate(this.Entity))
            {
                return;
            }

            if (_collectible.FollowingCatcher)
            {
                var player = UtilHelper.Player();

                if (player == null)
                {
                    return;
                }

                if (player.BodyCollider == null || _collectible.CatchAreaCollider == null)
                {
                    return;
                }

                var direction = player.Position - _collectible.Position;
                direction.Normalize();

                if (DirectionHelper.Validate(direction))
                {
                    _collectible.Position += direction * _collectible.Speed * Time.DeltaTime;

                    if (player.BodyCollider.Overlaps(_collectible.CatchAreaCollider))
                    {
                        var shouldDestroy = false;

                        switch (_collectible.Type)
                        {
                            case Enums.CollectibleType.Fried_Chicken:
                                var chicken = _collectible as FriedChicken;

                                if (chicken != null)
                                {
                                    _player.FriedChicken = chicken;
                                }
                                break;

                            case Enums.CollectibleType.Sandwich:
                                var sandwich = _collectible as Sandwich;

                                if (sandwich != null)
                                {
                                    _player.Sandwich = sandwich;
                                }
                                break;

                            case Enums.CollectibleType.Sliced_Bread:
                                var bread = _collectible as SlicedBread;

                                if (bread != null && _player?.FriedChicken != null)
                                {
                                    if (!_player.FriedChicken.IsDestroyed)
                                        _player?.FriedChicken?.Destroy();

                                    var island = UtilHelper.GetEntity<Island>();

                                    if (island != null)
                                    {
                                        shouldDestroy = true;

                                        var fallDestination = _player.Position + new Vector2(_player.SpriteWidth * Nez.Random.MinusOneToOne(), _player.SpriteHeight * 1.35f);

                                        var brandNewDeliciousSandwich = _scene.AddEntity(_prefabs.GetCollectible(_prefabs.InteractableConfig
                                            .CloneNpc(island.Id, _collectible.Position), Enums.CollectibleType.Sandwich, fallDestination));

                                        var crab = UtilHelper.Crab();

                                        if (crab != null)
                                        {
                                            crab.LookAtSandwich(brandNewDeliciousSandwich.Id);
                                        }
                                    }
                                }
                                break;
                        }

                        if (!shouldDestroy)
                        {
                            _collectible.FollowingCatcher = false;
                            _collectible.IsCollected = true;

                            var colliders = _collectible.GetComponents<Collider>();

                            foreach (var item in colliders)
                            {
                                item.RemoveComponent();
                            }
                        }
                        else
                        {
                            _collectible.Destroy();
                        }

                        if (_collectible.Shadow != null && !_collectible.IsDestroyed)
                        {
                            _collectible.Shadow.Destroy();
                        }

                        this.RemoveComponent();
                    }
                }
            }
        }
    }
}
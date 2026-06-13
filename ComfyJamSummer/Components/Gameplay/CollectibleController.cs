using ComfyJamSummer.Entities.Base;
using ComfyJamSummer.Entities.Creatures;
using ComfyJamSummer.Enums;
using ComfyJamSummer.Helpers;
using ComfyJamSummer.Manager;
using ComfyJamSummer.Prefab;
using Microsoft.Xna.Framework;
using Nez;
using System.Linq;

namespace ComfyJamSummer.Components.Gameplay
{
    public class CollectibleController : BaseComponent, IUpdatable
    {
        private Collider[] _nearColliders;

        private GameManager _gameManager;

        private float _crawlSpeedPercentage = 0.035f;
        private float _crawlFastSpeedPercentage = 2.2f;

        private Creature _selectedCreature;

        public CollectibleController(GameManager manager, Prefabs prefabs) : base(manager, prefabs)
        {
        }

        public override void OnAddedToEntity()
        {
            base.OnAddedToEntity();

            _nearColliders = new Collider[8];
        }

        public void Update()
        {
            if (_gameManager == null)
            {
                return;
            }

            if (_gameManager.CantDoAnyAction)
            {
                return;
            }

            if (this.Entity == null)
            {
                return;
            }

            if (this.Entity.IsDestroyed)
                return;

            var collectible = this.Entity as Collectible;

            if (collectible == null)
            {
                return;
            }

            if (collectible.FollowingCatcher)
            {
                this.RemoveComponent();

                if (!this.Entity.IsDestroyed && this.Entity.Scene != null)
                {
                    this.Entity.Destroy();
                }
            }

            collectible.InteractText?.SetEnabled(false);

            var aliveCreatures = Core.Scene.EntitiesOfType<Creature>().Where(x => x.IsAlive).ToList();
            var indexCreature = 0;

            while (indexCreature < aliveCreatures.Count && _selectedCreature == null && !_gameManager.CantDoAnyAction)
            {
                var creature = aliveCreatures[indexCreature];

                if (!creature.IsAlive)
                {
                    indexCreature++;
                    continue;
                }

                if (!collectible.FollowingCatcher)
                {
                    collectible.TimeLeftToBeCollected -= Time.DeltaTime;

                    var collider = collectible.ShowPopupItemInfoAreaCollider != null ? collectible.ShowPopupItemInfoAreaCollider : collectible.CatchAreaCollider;

                    if (collectible.CanBeCollected)
                    {
                        if (collectible.BounceComponent == null)
                        {
                            var direction = creature.Position - collectible.Position;
                            direction.Normalize();

                            var vel = Vector2.Zero;

                            //if (!_actualRoom.IsCombatHappening)
                            //{
                            //    vel += direction * (creature.Speed * _crawlFastSpeedPercentage) * Time.DeltaTime;
                            //}
                            //else
                            //{
                            //    vel += direction * (collectible.Speed * _crawlSpeedPercentage) * Time.DeltaTime;
                            //}

                            collectible.Position += vel;


                            if (collectible.Shadow != null)
                            {
                                collectible.Shadow.SetPosition(collectible.Position + new Vector2(0, 1));
                            }
                        }

                        if (creature.BodyCollider != null && collider != null && creature.BodyCollider.Overlaps(collider))
                        {
                            collectible.FollowingCatcher = true;

                            _selectedCreature = collectible.FollowingCatcher ? creature : null;
                        }
                    }
                }

                if (!collectible.FollowingCatcher)
                {
                    indexCreature++;
                }
            }

            if (collectible.FollowingCatcher && _selectedCreature != null)
            {
                var direction = _selectedCreature.Position - collectible.Position;
                direction.Normalize();

                var vel = Vector2.Zero;

                vel += direction * collectible.Speed * Time.DeltaTime;

                collectible.Position += vel;

                var radius = 8;

                Physics.OverlapCircleAll(collectible.Position, radius, _nearColliders);

                if (_nearColliders.Any(x => x != null && x.Entity != null && x.Entity.GetType() == typeof(Creature)))
                {
                    if (_prefabs == null)
                    {
                        return;
                    }

                    SoundHelper.PlayRandomSound(SoundFxName.Collect_1);

                    //ItemPopupUI.Emitter?.Emit(Enums.Events.UIEventEnums.CollectItem, new Events.UIEventData() { Item = item, ActualRoom = _actualRoom });                     
                }
            }
        }
    }
}
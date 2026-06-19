using ComfyJamSummer.AI.Enemies;
using ComfyJamSummer.Entities;
using ComfyJamSummer.Entities.Objects;
using ComfyJamSummer.Enums;
using ComfyJamSummer.Helpers;
using ComfyJamSummer.Manager;
using ComfyJamSummer.Prefab;
using Microsoft.Xna.Framework;
using Nez;
using Nez.AI.FSM;
using System.Linq;

namespace ComfyJamSummer.Components.Gameplay
{
    public class EnemyController : BaseComponent, IUpdatable
    {
        Enemy _enemy;

        StateMachine<Enemy> _machine;
        public EnemyController(GameManager manager, Prefabs prefabs) : base(manager, prefabs)
        {
        }

        public StateMachine<Enemy> Machine => _machine;

        public override void OnAddedToEntity()
        {
            base.OnAddedToEntity();

            _enemy = this.Entity as Enemy;

            if (_enemy != null)
            {
                _machine = new StateMachine<Enemy>(_enemy, new EnemyIdleState(_prefabs, _manager));

                _machine.AddState(new EnemyMoveState(_prefabs, _manager));
                _machine.AddState(new EnemyPatrolState(_prefabs, _manager));
                _machine.AddState(new EnemyAttackState(_prefabs, _manager));
            }
        }

        public void Update()
        {
            if (!Validate(this.Entity))
                return;

            var deltaTime = Time.DeltaTime;

            if (_enemy.Island != null && !_enemy.Island.IsDestroyed)
            {
                _enemy.Position = _enemy.ClampPosition(_enemy.Position);
            }

            if (!_enemy.IsAlive)
            {
                if (_enemy.WillSpawnChicken && !_enemy.AlreadySpawnedChicken)
                {
                    var shouldSpawn = true;

                    var rocket = UtilHelper.GetEntity<Rocket>();

                    if (rocket != null && rocket.BuildPhases.All(x=> x.IsDone))
                    {
                        shouldSpawn = false;
                    }

                    var player = UtilHelper.Player();

                    if (player != null && shouldSpawn)
                    {
                        var breadBag = UtilHelper.GetEntity<BreadBag>();

                        if (breadBag != null)
                        {
                            breadBag.IsInteracting = false;
                        }

                        var collectibleSpawner = UtilHelper.GetComponent<CollectibleSpawner>();

                        collectibleSpawner?.Spawn(CollectibleType.Fried_Chicken, 1, _enemy.Position, player.Position);
                    }

                    _enemy.AlreadySpawnedChicken = true;
                }

                _enemy.SpiralDisappear();
            }
            else
            {
                if (_enemy.TimeLeftToNextAtk > 0)
                {
                    _enemy.TimeLeftToNextAtk -= deltaTime;
                }

                if (_enemy.TimeLeftToNextPatrol > 0)
                {
                    _enemy.TimeLeftToNextPatrol -= deltaTime;
                }
            }

            _machine.Update(Time.DeltaTime);
        }
    }
}
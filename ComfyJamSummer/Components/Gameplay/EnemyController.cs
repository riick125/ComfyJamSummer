using ComfyJamSummer.AI.Enemies;
using ComfyJamSummer.Entities;
using ComfyJamSummer.Enums;
using ComfyJamSummer.Helpers;
using ComfyJamSummer.Manager;
using ComfyJamSummer.Prefab;
using Microsoft.Xna.Framework;
using Nez;
using Nez.AI.FSM;

namespace ComfyJamSummer.Components.Gameplay
{
    public class EnemyController : BaseComponent, IUpdatable
    {
        Enemy _enemy;

        StateMachine<Enemy> _machine;

        bool _spawnedCollectible;

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
                _machine = new StateMachine<Enemy>(_enemy, new EnemyMoveState(_prefabs, _manager));

                _machine.AddState(new EnemyIdleState(_prefabs, _manager));
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
                if (!_spawnedCollectible)
                {
                    var player = UtilHelper.Player();

                    if (player != null)
                    {
                        var collectibleSpawner = UtilHelper.GetComponent<CollectibleSpawner>();

                        var qty = 1; //Nez.Random.Range(4, 7)

                        collectibleSpawner?.Spawn(CollectibleType.Fried_Chicken, qty, _enemy.Position, player.Position);
                    }

                    _spawnedCollectible = true;
                }

                _enemy.RotationDegrees += _enemy.DyingRotationSpeed * deltaTime;

                var scale = _enemy.Scale.X;

                scale -= _enemy.LosingScaleSpeed * deltaTime;

                scale = Mathf.Clamp01(scale);

                _enemy.SetScale(scale);

                if (scale < 0.4f)
                {
                    _enemy.Alpha -= _enemy.LosingColorSpeed * deltaTime;

                    _enemy.Alpha = Mathf.Clamp01(_enemy.Alpha);

                    _enemy.GetAnyRenderer().SetColor(Color.White * _enemy.Alpha);

                    if (scale <= 0)
                    {
                        _enemy.Destroy();
                    }
                }
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
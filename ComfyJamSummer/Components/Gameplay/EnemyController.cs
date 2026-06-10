using ComfyJamSummer.AI.Enemies;
using ComfyJamSummer.Entities;
using ComfyJamSummer.Manager;
using ComfyJamSummer.Prefab;
using Nez;
using Nez.AI.FSM;

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
                _machine = new StateMachine<Enemy>(_enemy, new EnemyMoveState(_prefabs, _manager));

                _machine.AddState(new EnemyIdleState(_prefabs, _manager));
                _machine.AddState(new EnemyAttackState(_prefabs, _manager));
            }
        }

        public void Update()
        {
            if (!Validate(this.Entity))
                return;

            _machine.Update(Time.DeltaTime);
        }
    }
}
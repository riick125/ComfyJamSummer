using ComfyJamSummer.AI.Enemies;
using ComfyJamSummer.Entities;
using ComfyJamSummer.Manager;
using ComfyJamSummer.Prefab;
using Nez;
using Nez.AI.FSM;

namespace ComfyJamSummer.Components.Gameplay
{
    public class CrabController : BaseComponent, IUpdatable
    {
        Crab _crab;

        StateMachine<Crab> _machine;

        public CrabController(GameManager manager, Prefabs prefabs) : base(manager, prefabs)
        {
        }

        public StateMachine<Crab> Machine => _machine;

        public override void OnAddedToEntity()
        {
            base.OnAddedToEntity();

            _crab = this.Entity as Crab;

            if (_crab != null)
            {
                _machine = new StateMachine<Crab>(_crab, new CrabIdleState(_prefabs, _manager));

                _machine.AddState(new CrabBuildState(_prefabs, _manager));
                _machine.AddState(new CrabPissedOffState(_prefabs, _manager));
            }
        }

        public void Update()
        {
            if (!Validate(this.Entity))
            {
                return;
            }

            _machine.Update(Time.DeltaTime);
        }
    }
}
using ComfyJamSummer.Components.Gameplay;
using ComfyJamSummer.Entities.Configs;
using ComfyJamSummer.Entities.Creatures;
using ComfyJamSummer.Enums;
using ComfyJamSummer.Helpers;
using Nez.AI.FSM;

namespace ComfyJamSummer.Entities
{
    public class Enemy : Creature
    {
        public StateMachine<Enemy> Machine => EnemyController?.Machine;

        public EnemyController EnemyController => this.GetComponent<EnemyController>();

        public Player Player => UtilHelper.GetEntity<Player>(EntityNames.PLAYER);

        public EnemyType Type { get; set; }

        public float IdleTimeState { get; set; }

        public float MoveTimeState { get; set; }

        public float PatrolTimeState { get; set; }

        public float AtkTimeState { get; set; }

        public float RadiusAlertArea { get; set; }

        public Enemy CloneEnemy(EnemyConfig config)
        {
            var clone = base.CloneCreature(config) as Enemy;

            clone.IdleTimeState = config.IdleTimeState;
            clone.MoveTimeState = config.MoveTimeState;
            clone.PatrolTimeState = config.PatrolTimeState;
            clone.AtkTimeState = config.AtkTimeState;
            clone.RadiusAlertArea = config.RadiusAlertArea;

            clone.AddComponent(new EnemyController(UtilHelper.GameManager(), UtilHelper.Prefabs()));

            return clone;
        }

        public override void Stalk(Creature target, float radiusAreaAlert = 120)
        {
            if (target != Player)
            {
                return;
            }

            base.Stalk(target, radiusAreaAlert);
        }
    }
}
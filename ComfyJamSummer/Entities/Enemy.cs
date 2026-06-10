using ComfyJamSummer.Components.Gameplay;
using ComfyJamSummer.Entities.Configs;
using ComfyJamSummer.Entities.Creatures;
using ComfyJamSummer.Enums;
using ComfyJamSummer.Helpers;
using Microsoft.Xna.Framework;
using Nez;
using Nez.AI.FSM;
using System;

namespace ComfyJamSummer.Entities
{
    public class Enemy : Creature
    {
        public StateMachine<Enemy> Machine => EnemyController?.Machine;

        public EnemyController EnemyController => this.GetComponent<EnemyController>();

        public Player Player => UtilHelper.GetEntity<Player>();

        public EnemyType Type { get; set; }

        public float IdleTimeState { get; set; }

        public float MoveTimeState { get; set; }

        public float PatrolTimeState { get; set; }

        public float AtkTimeState { get; set; }

        public float BulletSpeed { get; set; }

        public float MaxAngleSpread { get; set; }

        float _maxPosY;

        public Enemy CloneEnemy(EnemyConfig config)
        {
            var clone = base.CloneCreature(config) as Enemy;

            clone.Type = Type;
            clone.IdleTimeState = config.IdleTimeState;
            clone.MoveTimeState = config.MoveTimeState;
            clone.PatrolTimeState = config.PatrolTimeState;
            clone.AtkTimeState = config.AtkTimeState;
            clone.MaxAngleSpread = config.MaxAngleSpread;
            clone.BulletSpeed = config.BulletSpeed;

            clone.AddComponent(new EnemyController(UtilHelper.GameManager(), UtilHelper.Prefabs()));

            clone.AddComponent(new CircleCollider(10) { Tag = CreatureCollider.Body.ToString() });

            return clone;
        }

        public override Vector2 Stalk(Creature target)
        {
            if (target != Player)
            {
                return this.Position;
            }

            var pos = base.Stalk(target);

            this.Position = pos;

            return this.Position;
        }

        public void Attack(Creature target)
        {
            if (!Validate())
            {
                return;
            }

            if (TimeLeftToNextAtk > 0)
            {
                return;
            }

            switch (Type)
            {
                case EnemyType.Birb:
                    BulletHelper.Create(this, target);
                    break;
            }

            TimeLeftToNextAtk = AtkSpeed;
        }
    }
}
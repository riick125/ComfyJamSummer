using ComfyJamSummer.Components.Gameplay;
using ComfyJamSummer.Components.Visuals;
using ComfyJamSummer.Configs;
using ComfyJamSummer.Entities.Configs;
using ComfyJamSummer.Entities.Creatures;
using ComfyJamSummer.Enums;
using ComfyJamSummer.Helpers;
using Microsoft.Xna.Framework;
using Nez;
using Nez.AI.FSM;

namespace ComfyJamSummer.Entities
{
    public class Enemy : Creature
    {
        public bool WillSpawnChicken { get; set; }

        public bool AlreadySpawnedChicken { get; set; }

        public StateMachine<Enemy> Machine => EnemyController?.Machine;

        public EnemyController EnemyController => this.GetComponent<EnemyController>();

        public Player Player => UtilHelper.Player();

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

            clone.AddComponent(new CircleCollider(11) { Tag = CreatureCollider.Body.ToString() });

            clone.AddComponent(new FakeShadowComponent(8, clone.SpriteHeight * 3));

            AnimHelper.Play(clone.Animator, BirbAnim.Idle);

            if (clone.Animator != null)
            {
                clone.Animator.Speed = 1.25f;
            }

            var battleComponent = UtilHelper.GetComponent<BattleComponent>();

            if (battleComponent != null)
            {
                if (battleComponent.ActualWave != null)
                {
                    battleComponent.ActualWave.BuffEnemy(clone);
                }
            }

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

            if (AnimHelper.CurrentAnim(Animator, BirbAnim.Atk) && Animator.CurrentFrame >= 13)
            {
                switch (Type)
                {
                    case EnemyType.Birb:
                        Prefabs?.PlaySoundRandomPitch(SoundFxName.Enemy_Shot, 0.065f);
                        BulletHelper.Create(this, target);
                        break;
                }

                TimeLeftToNextAtk = AtkSpeed;
            }
        }


        public virtual void Buff(BuffConfig config)
        {
            if (config != null)
            {
                MaxHP *= config.HpModifier;
                Damage *= config.DamageModifier;
                Speed *= config.SpeedModifier;
                BulletSpeed *= config.BulletSpeedModifier;
                AtkSpeed *= config.AtkSpeedModifier;
                ActualHP = MaxHP;
            }
        }
    }
}
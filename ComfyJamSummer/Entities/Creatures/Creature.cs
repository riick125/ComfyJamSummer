using ComfyJamSummer.Components.Visuals;
using ComfyJamSummer.Configs;
using ComfyJamSummer.Entities.Base;
using ComfyJamSummer.Entities.Configs;
using ComfyJamSummer.Entities.Debuffs;
using ComfyJamSummer.Enums;
using ComfyJamSummer.Helpers;
using Microsoft.Xna.Framework;
using Nez;
using System.Collections.Generic;

namespace ComfyJamSummer.Entities.Creatures
{
    public class Creature : Actor
    {
        public CreatureStates ActualState { get; set; }

        public string CreatureName { get; set; }

        public float ActualHP { get; set; }

        public float MaxHP { get; set; }

        public bool IsAlive { get { return ActualHP > 0; } }

        public float PreviousHP { get; set; }

        public float Damage { get; set; }

        public float TimeLeftToNextAtk { get; set; }

        public float AtkSpeed { get; set; }

        public bool IsAttacking { get; set; }

        public List<Debuff> DebuffsToGive { get; set; }

        Collider[] _collidersForAlertArea;

        public Creature CloneCreature(CreatureConfig config)
        {
            var clone = base.CloneAnimated(config.Position) as Creature;
            clone.ActualHP = config.HP;
            clone.MaxHP = config.HP;
            clone.Damage = config.Damage;
            clone.Speed = config.Speed;
            clone.AtkSpeed = config.AtkSpeed;
            clone.DebuffsToGive = new List<Debuff>();
            clone.AddComponent(new SimpleFlash(Game1.FlashMaterial, clone.Animator == null ? clone.Renderer : clone.Animator));

            return clone;
        }

        public enum CreatureStates
        {
            Idle,
            Walking,
            Attacking
        }

        public virtual void Buff(BuffConfig config)
        {
            if (config != null)
            {
                MaxHP *= config.HpModifier;
                Damage *= config.DamageModifier;
                Speed *= config.SpeedModifier;
                AtkSpeed *= config.AtkSpeedModifier;
            }
        }

        public void Idle()
        {
            AnimHelper.Play(Animator, CreatureAnim.Idle);
        }

        public void Patrol()
        {
            AnimHelper.Play(Animator, CreatureAnim.Move);
        }

        public virtual void Stalk(Creature target, float radiusAreaAlert = 120f)
        {
            if (target == null)
                return;

            if (target.BodyCollider == null)
                return;

            if (target.Id == this.Id || !target.IsAlive)
                return;

            if (_collidersForAlertArea == null)
            {
                _collidersForAlertArea = new Collider[5];
            }

            var targetCollider = Physics.OverlapCircle(this.Position, radiusAreaAlert, target.BodyCollider.PhysicsLayer);

            if (targetCollider != null && targetCollider.Entity?.Id == target.Id)
            {
                return;
            }

            var direction = target.Position - this.Position;
            direction.Normalize();

            var vel = Vector2.Zero;

            vel += direction * Speed * Time.DeltaTime;

            if (DirectionHelper.ValidateVelocity(direction, vel))
            {
                AnimHelper.Play(Animator, CreatureAnim.Move);

                this.Position += vel;
            }
        }

        public void Attack()
        {
            AnimHelper.Play(Animator, CreatureAnim.Atk);
        }

        public bool TakeDamage(float dmg)
        {
            var hurted = false;

            return hurted;
        }
    }
}
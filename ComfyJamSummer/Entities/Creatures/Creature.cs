using ComfyJamSummer.Components.Gameplay;
using ComfyJamSummer.Components.General;
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

        float _dyingRotationSpeed = 1300f, _losingScaleSpeed = 2.7f, _losingColorSpeed = 4f;

        public enum CreatureStates
        {
            Idle,
            Walking,
            Attacking
        }

        public Creature CloneCreature(CreatureConfig config)
        {
            var clone = base.CloneAnimated(config.Position) as Creature;
            clone._dyingRotationSpeed = _dyingRotationSpeed * Nez.Random.Range(0.85f, 1.25f);
            clone._losingScaleSpeed = _losingScaleSpeed * Nez.Random.Range(0.95f, 1.5f);
            clone._losingColorSpeed = _losingColorSpeed;
            clone.ActualHP = config.HP;
            clone.MaxHP = config.HP;
            clone.Damage = config.Damage;
            clone.Speed = config.Speed * Nez.Random.Range(0.91f, 1.04f);
            clone.AtkSpeed = config.AtkSpeed * Nez.Random.Range(0.95f, 1.15f);
            clone.DebuffsToGive = new List<Debuff>();
            clone.AddComponent(new SimpleFlash(Game1.FlashMaterial, clone.Animator == null ? clone.Renderer : clone.Animator));

            return clone;
        }

        public override void Update()
        {
            base.Update();

            if (!Validate())
            {
                return;
            }

            var deltaTime = Time.DeltaTime;

            if (!IsAlive)
            {

                this.RotationDegrees += _dyingRotationSpeed * deltaTime;

                var scale = this.Scale.X;

                scale -= _losingScaleSpeed * deltaTime;

                scale = Mathf.Clamp01(scale);

                this.SetScale(scale);

                if (scale < 0.4f)
                {
                    Alpha -= _losingColorSpeed * deltaTime;

                    Alpha = Mathf.Clamp01(Alpha);

                    this.GetAnyRenderer().SetColor(Color.White * Alpha);

                    if (scale <= 0)
                    {
                        this.Destroy();
                    }
                }
            }
            else
            {
                if (TimeLeftToNextAtk > 0)
                {
                    TimeLeftToNextAtk -= deltaTime;
                }
            }
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

        public virtual Vector2 Stalk(Creature target)
        {
            if (target == null)
                return this.Position;

            if (target.BodyCollider == null)
                return this.Position;

            if (target.Id == this.Id || !target.IsAlive)
                return this.Position;

            var direction = target.Position - this.Position;
            direction.Normalize();

            var vel = Vector2.Zero;

            vel += direction * Speed * Time.DeltaTime;

            if (DirectionHelper.ValidateVelocity(direction, vel))
            {
                AnimHelper.Play(Animator, CreatureAnim.Move);

                var pos = this.Position;
                pos += vel;

                pos += vel;

                return pos;
            }

            return this.Position;
        }

        public bool TakeDamage(float dmg)
        {
            if (!Validate() || !IsAlive)
            {
                return false;
            }

            if (dmg < 1)
            {
                dmg = 1;
            }

            dmg = float.Round(dmg);

            ActualHP -= dmg;
            ActualHP = Mathf.Clamp(ActualHP, 0, MaxHP);

            var textOffset = new Vector2(SpriteWidth * (Nez.Random.Chance(50) ? 1 : -1), -SpriteHeight / 4);

            var config = new BesideTextConfig(this, $"-{dmg}", offset: textOffset, color: Color.Red);
            TextHelper.CreateGoingUpBesideText(config);

            AnimHelper.Play(Animator, CreatureAnim.Dying);

            CrazyScaleComponent?.SqueezeByDirection(ComfyJamSummer.Components.General.CrazyScaleComponent.SqueezeDirection.Horizontal);

            SimpleFlash?.Flash(0.25f);

            if (!IsAlive)
            {
                RemoveComponent<CrazyScaleComponent>();
            }

            return true;
        }
    }
}
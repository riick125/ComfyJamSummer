using ComfyJamSummer.Components.Visuals;
using ComfyJamSummer.Entities.Base;
using ComfyJamSummer.Entities.Configs;
using ComfyJamSummer.Entities.Debuffs;
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

        public Creature CloneCreature(Config config)
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

        public bool TakeDamage(float dmg)
        {
            var hurted = false;

            return hurted;
        }
    }
}
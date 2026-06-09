using ComfyJamSummer.Entities.Base;
using ComfyJamSummer.Entities.Creatures;
using ComfyJamSummer.Enums;

namespace ComfyJamSummer.Entities.Debuffs
{
    public class Debuff : Animated
    {
        protected bool _playedSound;

        public DebuffType Type { get; set; }

        public float Duration { get; set; }

        public float TimeLeftToEnd { get; set; }

        public float ElapsedTime { get; set; }

        public float DamagePercentage { get; set; }

        public float DelayToDamage { get; set; }

        private float _minDmg = 1;

        public Debuff Clonar(DebuffType type, float duration, float damagePercentage = 0f, float delayToDamage = 1f)
        {
            var clone = CloneAnimated(default) as Debuff;
            clone.Type = type;
            clone.DelayToDamage = delayToDamage;
            clone.Duration = duration;
            clone.TimeLeftToEnd = duration;
            clone.DamagePercentage = damagePercentage;

            return clone;
        }

        public virtual void ProcessDamagePerSecond(Creature creature, bool isBoss)
        {
            if (DamagePercentage <= 0)
            {
                return;
            }

            var prefabs = Prefabs;

            if (prefabs != null && !_playedSound)
            {
                _playedSound = true;
            }

            var damage = creature.MaxHP * (isBoss ? DamagePercentage / 4 : DamagePercentage);

            if (damage < _minDmg)
            {
                damage = _minDmg;
            }

            if (ElapsedTime >= DelayToDamage)
            {
                creature.TakeDamage(damage);

                ElapsedTime = 0f;
            }
        }
    }
}
using ComfyJamSummer.Entities.Configs;
using ComfyJamSummer.Entities.Creatures;

namespace ComfyJamSummer.Entities
{
    public class Crab : Creature
    {
        public Crab CloneCrab(CreatureConfig config)
        {
            var clone = base.CloneCreature(config) as Crab;

            return clone;
        }
    }
}
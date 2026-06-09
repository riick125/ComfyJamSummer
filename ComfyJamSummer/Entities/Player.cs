using ComfyJamSummer.Components.Gameplay;
using ComfyJamSummer.Entities.Configs;
using ComfyJamSummer.Entities.Creatures;
using ComfyJamSummer.Helpers;

namespace ComfyJamSummer.Entities
{
    public class Player : Creature
    {
        public Player ClonePlayer(PlayerConfig config)
        {
            var clone = base.CloneCreature(config) as Player;

            clone.AddComponent(new PlayerController(UtilHelper.GameManager(), UtilHelper.Prefabs()));

            return clone;
        }
    }
}
using ComfyJamSummer.Components.Gameplay;
using ComfyJamSummer.Entities.Configs;
using ComfyJamSummer.Entities.Creatures;
using ComfyJamSummer.Helpers;

namespace ComfyJamSummer.Entities
{
    public class Player : Creature
    {
        public Gun Gun { get; set; }

        public Player ClonePlayer(PlayerConfig config)
        {
            var clone = base.CloneCreature(config) as Player;
            clone.Name = EntityNames.PLAYER;

            clone.AddComponent(new PlayerController(UtilHelper.GameManager(), UtilHelper.Prefabs()));

            return clone;
        }

        public override void OnAddedToScene()
        {
            base.OnAddedToScene();

            if (Gun != null && Gun.Scene == null)
            {
                this.Scene.AddEntity(Gun);
            }
        }
    }
}
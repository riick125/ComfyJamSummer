using ComfyJamSummer.Components.Gameplay;
using ComfyJamSummer.Components.Visuals;
using ComfyJamSummer.Entities.Configs;
using ComfyJamSummer.Entities.Creatures;
using ComfyJamSummer.Enums;
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

            AnimHelper.Play(clone.Animator, CreatureAnim.Idle);

            clone.AddComponent(new FakeShadowComponent(8, clone.SpriteHeight / 3.4f));

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
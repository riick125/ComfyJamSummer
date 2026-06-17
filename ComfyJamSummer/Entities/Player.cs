using ComfyJamSummer.Components.Gameplay;
using ComfyJamSummer.Components.Visuals;
using ComfyJamSummer.Entities.Base;
using ComfyJamSummer.Entities.Collectibles;
using ComfyJamSummer.Entities.Configs;
using ComfyJamSummer.Entities.Creatures;
using ComfyJamSummer.Enums;
using ComfyJamSummer.Helpers;
using Microsoft.Xna.Framework;
using Nez;

namespace ComfyJamSummer.Entities
{
    public class Player : Creature
    {
        public Gun Gun { get; set; }

        public FriedChicken FriedChicken { get; set; }

        public Sandwich Sandwich { get; set; }

        public int DevouredSandwiches { get; set; }

        public Vector2 OffsetCollectible { get; set; }

        float _collectibleFollowSpeed;

        public Player ClonePlayer(PlayerConfig config)
        {
            var clone = base.CloneCreature(config) as Player;
            clone.Name = EntityNames.PLAYER;

            clone.AddComponent(new PlayerController(UtilHelper.GameManager(), UtilHelper.Prefabs()));

            AnimHelper.Play(clone.Animator, CreatureAnim.Idle);

            clone.AddComponent(new FakeShadowComponent(8, clone.SpriteHeight / 3.4f));

            clone.OffsetCollectible = new Vector2(clone.SpriteWidth / 1.5f, -clone.SpriteHeight / 4);

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

        public override void Update()
        {
            base.Update();

            if (!Validate())
            {
                return;
            }

            DragCollectible(FriedChicken);

            DragCollectible(Sandwich);
        }

        void DragCollectible(Collectible collectible)
        {
            var deltaTime = Time.DeltaTime;

            if (collectible != null && collectible.IsCollected)
            {
                _collectibleFollowSpeed = Speed * 0.04f;

                collectible.Position = Vector2.Lerp(collectible.Position, this.Position + OffsetCollectible, _collectibleFollowSpeed * deltaTime);
            }
        }
    }
}
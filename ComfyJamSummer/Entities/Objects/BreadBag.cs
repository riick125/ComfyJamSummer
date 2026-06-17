using ComfyJamSummer.Components.Gameplay;
using ComfyJamSummer.Entities.Base;
using ComfyJamSummer.Entities.Configs;
using ComfyJamSummer.Enums;
using ComfyJamSummer.Extensions;
using ComfyJamSummer.Helpers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Nez;

namespace ComfyJamSummer.Entities.Objects
{
    public class BreadBag : InteractableObject
    {
        Player _player;

        string _canInteractText = "Press [E] to make sandwich", _cannotInteractText = "You need a fried chicken to make a sandwich!!";

        public BreadBag CloneBread(InteractableConfig config)
        {
            var clone = base.CloneInteractable(config) as BreadBag;

            clone.DepthHeight = clone.SpriteHeight / 2;

            clone.CreateCollider(CollisionLayer.Map, clone.SpriteWidth * 0.9f, clone.SpriteHeight / 6, offset: new Vector2(0, clone.SpriteHeight / 3.2f));

            return clone;
        }

        public override void Update()
        {
            base.Update();

            if (!ValidateBag())
            {
                return;
            }

            if (CanPressInteractButton && Input.IsKeyPressed(Keys.E) && _player?.FriedChicken != null && !IsInteracting)
            {
                StartInteractingWithPlayer();

                var spawner = UtilHelper.GetComponent<CollectibleSpawner>();

                var posBread = _player.Position + new Vector2(_player.SpriteWidth * Nez.Random.MinusOneToOne(), _player.SpriteHeight * 1.35f);

                spawner?.Spawn(CollectibleType.Sliced_Bread, 1, this.Position, posBread);
            }

        }

        public void ChangeText()
        {
            if (!ValidateBag())
            {
                return;
            }

            var txt = InteractText;

            var auxText = txt.Font.WrapText(_player.FriedChicken != null ? _canInteractText : _cannotInteractText, Constants.TEXT_WIDTH_LIMIT);

            var measureString = txt.Font.MeasureString(auxText);

            txt.Offset = new Vector2(-measureString.X / 4, (SpriteHeight / 2) + measureString.Y / 2);

            InteractText.TextComponent.SetText(auxText);
        }

        bool ValidateBag()
        {
            if (!Validate())
            {
                return false;
            }

            _player = UtilHelper.Player();

            if (_player == null)
            {
                return false;
            }

            if (InteractText == null)
            {
                return false;
            }

            if (InteractText.TextComponent == null)
            {
                return false;
            }

            return true;
        }
    }
}
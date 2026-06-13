using ComfyJamSummer.Helpers;
using ComfyJamSummer.Manager;
using Microsoft.Xna.Framework;
using Nez;
using System.Linq;

namespace ComfyJamSummer.Entities.Base
{
    public class InteractableObject : Animated
    {
        public Island Island { get; set; }

        protected uint _islandId;

        public bool CanPressInteractButton { get; private set; }

        public bool IsInteracting { get; set; }

        public BesideText InteractText;

        protected float _txtAlpha = 0f;

        public CircleCollider TalkAreaCollider
        {
            get
            {
                return GetComponent<CircleCollider>();
            }
        }

        public Vector2 FallDestination { get; set; }
        public Vector2 FallDirection { get; set; }

        protected float _talkAreaOffsetX, _talkAreaOffsetY;

        protected bool _disappeared;

        public InteractableObject CloneInteractable(uint islandId, Vector2 pos, float talkAreaOffsetX, float talkAreaOffsetY, string interactText = "Press [E] to interact", float talkAreaRadius = 9f)
        {
            var clone = base.CloneAnimated(pos) as InteractableObject;

            clone._islandId = islandId;

            clone.InteractText = TextHelper.CreateFollowBesideText(new BesideTextConfig(clone, interactText, new Vector2(SpriteWidth / 2.25f, SpriteHeight / 2), true, 0));

            clone.InteractText?.SetEnabled(false);

            var txtComponent = clone.InteractText.TextComponent;

            if (txtComponent != null)
            {
                txtComponent.SetColor(Constants.SPRITE_COLOR * 0);
            }

            var localoffset = new Vector2(talkAreaOffsetX <= 0 ? 0 : -talkAreaOffsetX, talkAreaOffsetY <= 0 ? 0 : talkAreaOffsetY / 2);

            clone.AddComponent(new CircleCollider(talkAreaRadius) { LocalOffset = localoffset, IsTrigger = true });

            return clone;
        }

        public override void OnAddedToScene()
        {
            base.OnAddedToScene();

            if (InteractText != null && InteractText.Scene == null)
            {
                Scene.AddEntity(InteractText);
            }

            Island = Scene.EntitiesOfType<Island>().FirstOrDefault(x => x.Id == _islandId);
        }

        public override void OnRemovedFromScene()
        {
            base.OnRemovedFromScene();

            if (!InteractText.IsDestroyed && InteractText.Scene != null)
            {
                InteractText.Destroy();
            }
        }

        public override void Update()
        {
            base.Update();

            if (this.IsDestroyed)
            {
                return;
            }

            var gameManager = GameManager;

            if (gameManager == null)
            {
                return;
            }

            var player = UtilHelper.Player();

            if (player == null)
            {
                return;
            }

            if (gameManager.CantDoAnyAction)
            {
                ToggleEnableDisable(false);
                return;
            }

            CanPressInteractButton = CheckInteractAreaCollision(player, gameManager);
        }

        public virtual bool StartInteractingWithPlayer()
        {
            if (IsInteracting)
            {
                return false;
            }

            IsInteracting = true;

            if (InteractText != null)
            {
                var txtComponent = InteractText.TextComponent;

                if (txtComponent != null)
                {
                    txtComponent.SetColor(Constants.SPRITE_COLOR * 0);
                    _txtAlpha = 0f;
                }
            }

            return true;
        }

        private void ToggleEnableDisable(bool value)
        {
            if (InteractText != null)
            {
                InteractText.SetEnabled(value);
            }
        }

        private bool CheckInteractAreaCollision(Player player, GameManager gameManager)
        {
            if (IsInteracting)
            {
                return false;
            }

            var result = false;

            if (gameManager.CantDoAnyAction)
            {
                return false;
            }

            if (InteractText == null)
            {
                return false;
            }

            if (player == null)
            {
                return false;
            }

            if (player.BodyCollider == null || TalkAreaCollider == null)
            {
                return false;
            }

            if (player != null && player.BodyCollider != null)
            {
                var collided = player.BodyCollider.Overlaps(TalkAreaCollider);
                result = collided;

                if (collided)
                {
                    _txtAlpha += 5 * Time.DeltaTime;
                }
                else
                {
                    _txtAlpha -= 5 * Time.DeltaTime;
                }
            }
            else
            {
                _txtAlpha -= 5 * Time.DeltaTime;
            }

            _txtAlpha = Mathf.Clamp(_txtAlpha, 0, 1f);

            var txtComponent = InteractText.TextComponent;

            if (txtComponent != null)
            {
                txtComponent.SetColor(Constants.SPRITE_COLOR * _txtAlpha);
            }

            return result;
        }
    }
}
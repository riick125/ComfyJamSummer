using ComfyJamSummer.Entities.Configs;
using ComfyJamSummer.Entities.Objects;
using ComfyJamSummer.Helpers;
using ComfyJamSummer.Manager;
using Microsoft.Xna.Framework;
using Nez;
using System.Linq;

namespace ComfyJamSummer.Entities.Base
{
    public class InteractableObject : Animated
    {
        public InteractableConfig BaseConfig { get; set; }

        public bool IsKillable { get; set; }

        public float ActualHP { get; set; }

        public float MaxHP { get; set; }

        public bool IsAlive
        {
            get
            {
                if (!IsKillable)
                {
                    return true;
                }
                else
                {
                    return ActualHP > 0;
                }
            }
        }

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

        public InteractableObject CloneInteractable(InteractableConfig config)
        {
            var clone = base.CloneAnimated(config.Position) as InteractableObject;

            clone._islandId = config.IslandId;

            var talkAreaOffsetX = config.TalkAreaOffsetX;
            var talkAreaOffsetY = config.TalkAreaOffsetY;
            var talkAreaRadius = config.TalkAreaRadius;
            var interactText = config.InteractText;

            if (BaseConfig != null)
            {
                talkAreaOffsetX = BaseConfig.TalkAreaOffsetX != 0 && BaseConfig.TalkAreaOffsetX != talkAreaOffsetX ? BaseConfig.TalkAreaOffsetX : talkAreaOffsetX;

                talkAreaOffsetY = BaseConfig.TalkAreaOffsetY != 0 && BaseConfig.TalkAreaOffsetY != talkAreaOffsetY ? BaseConfig.TalkAreaOffsetY : talkAreaOffsetY;

                talkAreaRadius = BaseConfig.TalkAreaRadius != 0 && BaseConfig.TalkAreaRadius != talkAreaRadius ? BaseConfig.TalkAreaRadius : talkAreaRadius;

                interactText = !string.IsNullOrEmpty(BaseConfig.InteractText) && BaseConfig.InteractText != interactText ? BaseConfig.InteractText : interactText;
            }

            clone.InteractText = TextHelper.CreateFollowBesideText(new BesideTextConfig(clone, interactText, new Vector2(SpriteWidth / 2.25f, SpriteHeight / 2), true, 0));

            clone.InteractText?.SetEnabled(false);

            var txtComponent = clone.InteractText.TextComponent;

            if (txtComponent != null)
            {
                txtComponent.SetColor(Constants.WHITE_COLOR * 0);
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

            ToggleEnableDisable(true);

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
                    txtComponent.SetColor(Constants.WHITE_COLOR * 0);
                    _txtAlpha = 0f;
                }
            }

            return true;
        }

        protected void MeasureInteractText(string txt)
        {
            if (InteractText == null || InteractText?.TextComponent == null)
            {
                return;
            }

            var auxText = InteractText.Font.WrapText(txt, Constants.TEXT_WIDTH_LIMIT);

            var measureString = InteractText.Font.MeasureString(auxText);

            InteractText.Offset = new Vector2(-measureString.X / 4, (SpriteHeight / 2) + measureString.Y / 2);

            InteractText.TextComponent.SetText(auxText);
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

            if (player.InteractAreaCollider == null || TalkAreaCollider == null)
            {
                return false;
            }

            if (player != null && player.InteractAreaCollider != null)
            {
                var collided = player.InteractAreaCollider.Overlaps(TalkAreaCollider);
                result = collided;

                if (collided)
                {
                    switch (this)
                    {
                        case BreadBag bag:
                            bag.ChangeText();
                            break;
                    }

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
                txtComponent.Transform.SetPosition(this.Position);

                txtComponent.SetColor(Color.White * _txtAlpha);
            }

            return result;
        }
    }
}
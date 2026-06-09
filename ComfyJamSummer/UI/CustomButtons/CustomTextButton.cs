using ComfyJamSummer.Components.Gameplay;
using ComfyJamSummer.Enums;
using ComfyJamSummer.Helpers;
using Microsoft.Xna.Framework;
using Nez.UI;

namespace ComfyJamSummer.UI.CustomButtons
{
    public class CustomTextButton : TextButton, IInputListener, IGamepadFocusable
    {
        public Vector2 OriginalPosition { get; private set; }

        static int _idGenerator;

        public readonly int Id;

        public SqueezeAnimation SqueezeAnimation { get; set; }

        public CustomTextButton(string text, TextButtonStyle style, bool shouldSqueeze = false) : base(text, style)
        {
            if (shouldSqueeze)
            {
                SqueezeAnimation = UtilHelper.CreateSqueezeAnimation(this, Nez.Random.Range(0.945f, 0.96f));
            }

            Id = _idGenerator;
            _idGenerator++;

            OnClicked += CustomTextButton_OnClicked;
        }

        private void CustomTextButton_OnClicked(Button obj)
        {
        }

        public override Element SetPosition(float x, float y)
        {
            if (OriginalPosition == default)
            {
                OriginalPosition = new Vector2(x, y);
            }

            this.SetOrigin(PreferredWidth / 2, PreferredHeight / 2);

            return base.SetPosition(x, y);
        }

        void IInputListener.OnMouseEnter()
        {
            SqueezeAnimation?.SqueezeByDirection(SqueezeDirection.Horizontal);

            _mouseOver = true;
        }

        void IInputListener.OnMouseExit()
        {
            _mouseOver = _mouseDown = false;
        }

        #region IGamepadFocusable
        void IGamepadFocusable.OnUnhandledDirectionPressed(Direction direction)
        { }

        void IGamepadFocusable.OnFocused()
        {
            // play sound

            OnFocused();

            SqueezeAnimation?.SqueezeByDirection(SqueezeDirection.Horizontal);

            // enable this for specific instant action
            //if (UserData != null && UserData.GetType().Equals(typeof(OptionsScreensEnum)))
            //{
            //    OnActionButtonReleased();
            //}
        }

        void IGamepadFocusable.OnUnfocused()
        {
            OnUnfocused();
        }

        void IGamepadFocusable.OnActionButtonPressed()
        {
            OnActionButtonPressed();
        }

        void IGamepadFocusable.OnActionButtonReleased()
        {
            OnActionButtonReleased();
        }
        #endregion
    }
}
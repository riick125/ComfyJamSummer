using System;
using System.Collections.Generic;
using System.Linq;
using ComfyJamSummer.Enums;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Nez;

namespace ComfyJamSummer.Components.Extensions
{
    public class InputManager : SceneComponent
    {
        public InputType ActualInputType { get; set; }
        public InputType PreviousInputType { get; set; }

        private readonly List<Buttons> _allButtons;

        private readonly List<Buttons> _availableButtons;

        private readonly List<Buttons> _invalidButtons;

        private Vector2 _lastMousePosition;

        public InputManager()
        {
            ActualInputType = InputType.Keyboard;

            _allButtons = Enum.GetValues<Buttons>().ToList();

            _availableButtons = _allButtons.Where(x => x == Buttons.A ||
            x == Buttons.B ||
            x == Buttons.Y ||
            x == Buttons.X ||
            x == Buttons.RightShoulder ||
            x == Buttons.LeftShoulder ||
            x == Buttons.RightTrigger ||
            x == Buttons.LeftTrigger ||
            x == Buttons.LeftStick ||
            x == Buttons.RightStick).ToList();

            _invalidButtons = _allButtons.Except(_availableButtons).ToList();
        }

        public override void Update()
        {
            base.Update();

            var keyboardState = Input.CurrentKeyboardState;
            var gamePad = Input.GamePads[0];
            var isUsingKeyboard = false;

            if (keyboardState.GetPressedKeys().Length > 0 || _lastMousePosition != Input.MousePosition)
            {
                _lastMousePosition = Input.MousePosition;
                isUsingKeyboard = true;

                if (ActualInputType != InputType.Keyboard)
                {
                    PreviousInputType = ActualInputType;
                    ActualInputType = InputType.Keyboard;
                }
            }

            if (!isUsingKeyboard)
            {
                var index = 0;
                var isUsingGamepad = false;

                if (gamePad.IsConnected())
                {
                    while (index < _allButtons.Count - 1 && !isUsingGamepad)
                    {
                        isUsingGamepad = gamePad.IsButtonDown(_allButtons[index]);

                        if (isUsingGamepad && ActualInputType != InputType.Gamepad)
                        {
                            PreviousInputType = ActualInputType;
                            ActualInputType = InputType.Gamepad;
                        }

                        index++;
                    }
                }
            }
        }

        public List<Buttons> GetButtonPressed()
        {
            var result = new List<Buttons>();

            var gamepad = Input.GamePads[0];

            if (gamepad.IsConnected())
            {
                var index = 0;

                while (index < _allButtons.Count - 1)
                {
                    var button = _allButtons[index];

                    if (gamepad.IsButtonPressed(button))
                    {
                        result.Add(button);

                        return result;
                    }

                    index++;
                }
            }

            return result;
        }

        public bool IsButtonInvalid(Buttons button)
        {
            return _invalidButtons.Contains(button);
        }
    }
}
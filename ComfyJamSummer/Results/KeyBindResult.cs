using System;

namespace ComfyJamSummer.Results
{
    public class KeyBindResult
    {
        public KeybindChangeStateEnum State { get; set; }

        public Enum? NewBindedKey { get; set; }

        public KeyBindResult(KeybindChangeStateEnum state, Enum newBindedKey = null)
        {
            State = state;
            NewBindedKey = newBindedKey;
        }
    }

    public enum KeybindChangeStateEnum
    {
        InvalidOperation,
        InvalidKey,
        KeyAlreadyExist,
        Success
    }
}
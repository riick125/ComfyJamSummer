using Keys = Microsoft.Xna.Framework.Input.Keys;

namespace ComfyJamSummer.Enums
{
    public enum GameKey
    {
        MOVE_UP = Keys.W,
        MOVE_LEFT = Keys.A,
        MOVE_DOWN = Keys.S,
        MOVE_RIGHT = Keys.D,
        INTERACT = Keys.E
    }

    public enum CustomKey
    {
        None = -999,
        LeftMouseButton = 1000,
        RightMouseButton = 1001,
        MiddleMouseButton = 1002
    }

    public enum InputType
    {
        //None,
        Keyboard,
        Gamepad
    }
}
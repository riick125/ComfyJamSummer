using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ComfyJamSummer.PoolObjects
{
    public class CustomSpritePoolConfig
    {
        public Texture2D Texture { get; set; }

        public Vector2 Position { get; set; }

        public Point Point { get; set; }

        public Color Color { get; set; } = Color.White;

        public float Rotation { get; set; } = 0f;

        public float Scale { get; set; } = 1f;

        public float Depth { get; set; } = 0;

        public float Alpha { get; set; } = 1f;
    }
}
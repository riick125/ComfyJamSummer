using ComfyJamSummer.PoolObjects;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ComfyJamSummer.Entities.Base
{
    public class CustomSprite
    {
        public Texture2D Texture { get; set; }

        public Vector2 Position { get; set; }

        public Point Point { get; set; }

        public Color Color { get; set; }

        public Rectangle Rectangle
        {
            get
            {
                if (Texture == null)
                {
                    return new Rectangle();
                }

                return new Rectangle((int)Position.X, (int)Position.Y, Texture.Width, Texture.Height);
            }
        }

        public Vector2 Origin { get; set; }

        public float Rotation { get; set; }

        public float Scale { get; set; }

        public float Depth { get; set; }

        public float Alpha { get; set; }

        public virtual void Initialize(CustomSpritePoolConfig config)
        {
            Texture = config.Texture;
            Position = config.Position;
            Point = config.Point;
            Color = config.Color;
            Origin = new Vector2(Texture.Width / 2, Texture.Height / 2);
            Rotation = config.Rotation;
            Scale = config.Scale;
            Depth = config.Depth;
            Alpha = config.Alpha;
        }

        public virtual void Initialize(CustomParticlePoolConfig config)
        {
            Texture = config.Texture;
            Position = config.Position;
            Point = config.Point;
            Color = config.Color;
            Origin = new Vector2(Texture.Width / 2, Texture.Height / 2);
            Rotation = config.Rotation;
            Scale = config.Scale;
            Depth = config.Depth;
            Alpha = config.Alpha;
        }
    }
}

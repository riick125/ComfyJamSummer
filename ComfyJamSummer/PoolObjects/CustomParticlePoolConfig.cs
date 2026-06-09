using Microsoft.Xna.Framework;

namespace ComfyJamSummer.PoolObjects
{
    public class CustomParticlePoolConfig : CustomSpritePoolConfig
    {
        public Vector2 Gravity { get; set; }

        public Vector2 StartSpeed { get; set; }

        public Vector2 EndSpeed { get; set; }

        public float StartScale { get; set; }

        public float EndScale { get; set; }

        public int ActualBounces { get; set; }

        public int TotalBounces { get; set; }

        public Vector2 PosYLimit { get; set; }

        public Vector2 Direction { get; set; }

        public float LifeTime { get; set; }

        public float MaxLifeTime { get; set; }
    }
}
using Microsoft.Xna.Framework;

namespace ComfyJamSummer.Entities.Configs
{
    public class PlayerConfig : CreatureConfig
    {
        public PlayerConfig ClonePlayer(Vector2 pos)
        {
            return base.Clone(pos) as PlayerConfig;
        }
    }
}
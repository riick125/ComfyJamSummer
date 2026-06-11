using Microsoft.Xna.Framework;

namespace ComfyJamSummer.Entities.Configs
{
    public class PlayerConfig : CreatureConfig
    {
        public PlayerConfig ClonePlayer(uint islandId, Vector2 pos)
        {
            return base.Clone(islandId, pos) as PlayerConfig;
        }
    }
}
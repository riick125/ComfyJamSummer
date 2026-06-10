using ComfyJamSummer.Enums;
using Microsoft.Xna.Framework;

namespace ComfyJamSummer.Entities.Configs
{
    public class EnemyConfig : CreatureConfig
    {
        public EnemyType Type { get; set; }

        public float IdleTimeState { get; set; }

        public float MoveTimeState { get; set; }

        public float PatrolTimeState { get; set; }

        public float AtkTimeState { get; set; }

        public float RadiusAlertArea { get; set; }

        public EnemyConfig CloneEnemy(Vector2 pos)
        {
            var clone = base.Clone(pos) as EnemyConfig;

            clone.IdleTimeState = IdleTimeState;
            clone.MoveTimeState = MoveTimeState;
            clone.PatrolTimeState = PatrolTimeState;
            clone.AtkTimeState = AtkTimeState;
            clone.RadiusAlertArea = RadiusAlertArea;

            return clone;
        }
    }
}
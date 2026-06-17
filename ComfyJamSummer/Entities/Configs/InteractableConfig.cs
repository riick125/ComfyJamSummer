using Microsoft.Xna.Framework;

namespace ComfyJamSummer.Entities.Configs
{
    public class InteractableConfig : CreatureConfig
    {
        public float TalkAreaOffsetX { get; set; }

        public float TalkAreaOffsetY { get; set; }

        public string InteractText { get; set; }

        public float TalkAreaRadius { get; set; }

        public bool IsKillable { get; set; }

        public InteractableConfig CloneNpc(uint islandId, Vector2 pos, float talkAreaOffsetX = 0f, float talkAreaOffsetY = 0f, string interactText = "Press[E] to interact", float talkAreaRadius = 12f)
        {
            var clone = base.Clone(islandId, pos) as InteractableConfig;

            clone.TalkAreaOffsetX = talkAreaOffsetX != 0 ? talkAreaOffsetX : TalkAreaOffsetX;
            clone.TalkAreaOffsetY = talkAreaOffsetY != 0 ? talkAreaOffsetY : TalkAreaOffsetY;
            clone.InteractText = !string.IsNullOrEmpty(interactText) ? interactText : InteractText;
            clone.TalkAreaRadius = talkAreaRadius != 0 ? talkAreaRadius : TalkAreaRadius;

            return clone;
        }
    }
}
using Nez;
using Nez.UI;

namespace ComfyJamSummer.UI.Animations.Base
{
    public class AnimatorUI : Container
    {
        public Entity Entity { get; set; }

        public SpriteAnimatorUI Animator { get { return Entity?.GetComponent<SpriteAnimatorUI>(); } }
    }
}
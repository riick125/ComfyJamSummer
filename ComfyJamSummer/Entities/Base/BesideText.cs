using ComfyJamSummer.Components.Extensions;
using Microsoft.Xna.Framework;
using Nez;
using Nez.BitmapFonts;

namespace ComfyJamSummer.Entities.Base
{
    public class BesideText : Animated
    {
        public uint EntityId { get; set; }

        public string Text { get; set; }

        public Vector2 Offset { get; set; }

        public Microsoft.Xna.Framework.Color Color { get; set; }

        public BitmapFont Font { get; set; }

        public float OriginalAlpha { get; set; }

        public float TimeLeftToDisappear { get; set; }

        public float Duration { get; set; }

        public bool IsGoingUp { get; set; }

        public bool ShouldWink { get; set; }

        public bool HasTypingTextEffect { get; set; }

        public bool IsPermanent { get; set; }

        public bool IsTalk { get; set; }


        protected TextComponent _textComponent;

        public TextComponent TextComponent
        {
            get
            {
                return GetComponent<TextComponent>();
            }
        }

        public override void OnAddedToScene()
        {
            base.OnAddedToScene();

            var registry = Scene.GetSceneComponent<BesideTextRegistry>();
            registry?.Register(this);
        }

        public override void OnRemovedFromScene()
        {
            base.OnRemovedFromScene();

            var registry = Scene.GetSceneComponent<BesideTextRegistry>();
            registry?.Unregister(this);
        }
    }
}
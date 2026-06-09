using System;
using Microsoft.Xna.Framework;
using Nez;

namespace ComfyJamSummer.Helpers
{
    public class BesideTextConfig : IDisposable
    {
        private bool _disposed;

        public BesideTextConfig(Entity target,
            string text,
            Vector2 offset,
            bool smallText = false,
            float duration = 0.9f,
            float alpha = 1f,
            bool shouldWink = false,
            bool typingTextEffect = false,
            int maxLineWidth = 0,
            Color color = default)
        {
            Target = target;
            Text = text;
            Offset = offset;
            SmallText = smallText;
            Duration = duration;
            IsPermanent = duration == 0f;
            Alpha = alpha;
            ShouldWink = shouldWink;
            HasTypingTextEffect = typingTextEffect;
            MaxLineWidth = maxLineWidth;

            Color = color != default ? color : Constants.SPRITE_COLOR;
        }

        public Entity Target { get; set; }

        public string Text { get; set; }

        public Vector2 Offset { get; set; }

        public bool SmallText { get; set; }

        public float Duration { get; set; }

        public float Alpha { get; set; }

        public bool ShouldWink { get; set; }

        public bool HasTypingTextEffect { get; set; }

        public bool IsPermanent { get; set; }

        public bool IsTalk { get; set; }

        public int MaxLineWidth { get; set; }

        public Color Color { get; set; }

        public void Dispose()
        {
            Dispose(true);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                _disposed = true;
            }
        }
    }
}
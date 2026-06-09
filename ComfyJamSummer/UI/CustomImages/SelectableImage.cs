using ComfyJamSummer.Prefab;
using ComfyJamSummer.UI.CustomImages;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ComfyJamSummer.UI.CustomImages
{
    public class SelectableImage : ShakableImage
    {
        protected float _transparentAlpha = 0.45f;

        public SelectableImage(Prefabs prefabs, Texture2D texture, float shakeIntensity = 10f, bool isMuted = false) : base(prefabs, texture, shakeIntensity)
        {
        }

        public virtual void SetSelected(bool selected)
        {
            if ((!IsSelected && selected) && !IsMuted)
            {
                // sound
            }

            IsSelected = selected;

            var color = IsSelected ? Color.White : Color.White * _transparentAlpha;

            this.SetColor(color);
        }

        public bool IsSelected { get; protected set; }
    }
}
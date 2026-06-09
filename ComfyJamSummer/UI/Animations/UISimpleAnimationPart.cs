using System;
using System.Collections.Generic;
using ComfyJamSummer.UI.CustomElements;
using Microsoft.Xna.Framework;
using Nez;
using Nez.UI;

namespace ComfyJamSummer.UI.Animations
{
    public class UISimpleAnimationPart
    {
        public List<SimpleElement> Elements { get; set; }
        private readonly Vector2 _elementOriginalSize;

        public Enum Name { get; set; }

        public float Duration { get; set; }

        public float StartDelay { get; set; }

        public float TimeLeftToEnd { get; set; }

        public bool Finished { get { return TimeLeftToEnd <= 0; } }

        public Vector2 Destination { get; set; }

        public float InitialScale { get; set; }

        private float _increaseScaleRate, _decreaseScaleRate;
        private readonly float _scaleRateMaximumValue = 8f, _scaleRateMinimumValue = 2f;

        public float FinalScale { get; set; }


        public UISimpleAnimationPart(Enum name, float duration, float startDelay = 0f, Vector2 destination = default)
        {
            Name = name;
            Duration = duration;
            TimeLeftToEnd = duration;
            Destination = destination;
            StartDelay = startDelay;

            _increaseScaleRate = _scaleRateMinimumValue;
            _decreaseScaleRate = _scaleRateMinimumValue;

            InitialScale = 0f;
            FinalScale = 1f;
        }

        public UISimpleAnimationPart(List<SimpleElement> elements, Enum name, float duration, float startDelay = 0f, Vector2 destination = default)
        {
            Name = name;
            Duration = duration;
            TimeLeftToEnd = duration;
            Destination = destination;
            StartDelay = startDelay;

            _increaseScaleRate = _scaleRateMinimumValue;
            _decreaseScaleRate = _scaleRateMinimumValue;

            InitialScale = 0f;
            FinalScale = 1f;

            Elements = elements;
        }

        public void SetInitialScale(float value)
        {
            foreach (var elem in Elements)
            {
                UpdateScale(elem, value);
            }
        }

        public void SetPosition(Vector2 pos)
        {
            foreach (var elem in Elements)
            {
                elem.Instance.SetPosition(pos.X - elem.Instance.PreferredWidth / 2, pos.Y - elem.Instance.PreferredHeight / 2);
            }
        }

        public void SetIncreaseScaleRate(float value)
        {
            _increaseScaleRate = value;

            _increaseScaleRate = Mathf.Clamp(_increaseScaleRate, _scaleRateMinimumValue, _scaleRateMaximumValue);
        }

        public void SetDecreaseScaleRate(float value)
        {
            _decreaseScaleRate = value;

            _decreaseScaleRate = Mathf.Clamp(_decreaseScaleRate, _scaleRateMinimumValue, _scaleRateMaximumValue);
        }

        public bool ChangeScale(bool increase)
        {
            var deltaTime = Time.AltDeltaTime;

            var scale = 0f;

            foreach (var elem in this.Elements)
            {
                switch (elem.Instance)
                {
                    case Image img:
                        scale = elem.Instance.GetScaleX();
                        break;

                    case Label lbl:
                        scale = lbl.GetStyle().FontScaleX;
                        break;
                }

                if (increase)
                {
                    scale += _increaseScaleRate * deltaTime;
                }
                else
                {
                    scale -= _decreaseScaleRate * deltaTime;
                }

                UpdateScale(elem, scale);
            }

            return scale >= 1f;
        }

        public bool ChangeScale(SimpleElement elem, bool increase)
        {
            var deltaTime = Time.AltDeltaTime;

            var scale = 0f;

            switch (elem.Instance)
            {
                case Image img:
                    scale = elem.Instance.GetScaleX();
                    break;

                case Dialog dialog:
                    scale = elem.Instance.GetScaleX();
                    break;

                case Label lbl:
                    scale = lbl.GetStyle().FontScaleX;
                    break;
            }

            if (increase)
            {
                scale += _increaseScaleRate * deltaTime;
            }
            else
            {
                scale -= _decreaseScaleRate * deltaTime;
            }

            UpdateScale(elem, scale);

            return scale >= 1f;
        }

        private void UpdateScale(SimpleElement elem, float scale)
        {
            scale = Mathf.Clamp01(scale);

            switch (elem.Instance)
            {
                case Label lbl:
                    lbl.SetFontScale(scale, scale);
                    break;

                default:
                    var newSize = elem.OriginalSize * scale;

                    newSize.X = (float)Math.Round(newSize.X, 2);
                    newSize.Y = (float)Math.Round(newSize.Y, 2);

                    elem.Instance.SetSize(newSize.X, newSize.Y);

                    elem.Instance.SetScaleX(scale);
                    break;
            }
        }
    }
}
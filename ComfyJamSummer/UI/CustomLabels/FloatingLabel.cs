using Nez;
using Nez.UI;

namespace ComfyJamSummer.UI.CustomLabels
{
    public class FloatingLabel : Label
    {
        float _minPointsLifeTime = 0.25f, _maxPointsLifeTime = 0.75f;

        float _lifeTime;

        float _elapsedTime;

        public bool IsDone => _elapsedTime >= _lifeTime;

        float _alphaLossValue = 0.25f, _alpha = 1f;

        float _speed, _minSpeed = 50, _maxSpeed = 60;

        public FloatingLabel(string text, LabelStyle style) : base(text, style)
        {
            _lifeTime = Nez.Random.Range(_minPointsLifeTime, _maxPointsLifeTime);

            _speed = Nez.Random.Range(_minSpeed, _maxSpeed);
        }

        public void Process()
        {
            var deltaTime = Time.DeltaTime;

            _elapsedTime += deltaTime;

            var speed = _speed * deltaTime;

            SetPosition(GetX(), GetY() - speed);

            if (_elapsedTime >= _lifeTime / 2)
            {
                var style = GetStyle();

                if (style == null)
                {
                    return;
                }

                _alpha -= _alphaLossValue * deltaTime;

                Mathf.Clamp01(_alpha);

                style.FontColor *= _alpha;

                SetStyle(style); 
            }
        }
    }
}
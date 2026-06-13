using Nez;

namespace ComfyJamSummer.Entities.General
{
    public class CustomAccelerator
    {
        float _force, _forceMaxValue;
        float _accel, _accelValue;
        float _forceOriginalValue;

        float _duration;
        readonly float _maxDuration;
        readonly bool _isInfinite;

        public float Accel => _accel;

        public float Force => _force;

        public CustomAccelerator(float force, float duration = 0f)
        {
            if (force <= 0)
            {
                return;
            }

            _force = force;
            _forceOriginalValue = force;
            _duration = duration;
            _maxDuration = _duration;

            _isInfinite = duration <= 0;

            Reset();
        }

        public bool Process()
        {
            if (!_isInfinite && _duration <= 0)
            {
                return false;
            }

            var deltaTime = Time.DeltaTime;

            _duration -= deltaTime;
            _duration = Mathf.Clamp(_duration, 0, 30);

            _accel += _accelValue * deltaTime;
            _accel = Mathf.Clamp(_accel, _accelValue, _force);

            return true;
        }

        public void SetForce(float value)
        {
            if (value < (_forceOriginalValue * 0.05f))
            {
                return;
            }

            _force = value;
            _forceOriginalValue = _force;

            Reset();
        }

        public void Reset()
        {
            _force = _forceOriginalValue;
            _accel = _force * 0.4f;
            _accelValue = _force * 0.5f;
            _forceMaxValue = _force;
        }
    }
}
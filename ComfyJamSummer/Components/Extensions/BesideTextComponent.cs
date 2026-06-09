using System;
using System.Collections.Generic;
using System.Linq;
using ComfyJamSummer.Entities.Base;
using ComfyJamSummer.Entities.Creatures;
using ComfyJamSummer.Enums;
using ComfyJamSummer.Helpers;
using ComfyJamSummer.Manager;
using ComfyJamSummer.Prefab;
using Microsoft.Xna.Framework;
using Nez;

namespace ComfyJamSummer.Components.Extensions
{
    public class BesideTextComponent : Component, IUpdatable
    {
        private Prefabs _prefabs;

        private GameManager manager;

        private Entity _target;
        private Vector2 _localOffset;
        private float _duration = 0.5f;
        private float _vel = 30;

        private float _alpha;
        private float _alphaMaxValue;

        private bool _isFadingOut = true;

        private float _typingRate = 0.035f;
        private float _timeLeftToNextChar;
        private int _actualChar;
        private string _originalText;

        private bool _isPermanent;

        private List<SoundFxName> _typingSounds;
        private SoundPrefab _typingSoundPrefab;

        private bool _canPlaySound = true;

        private BesideText text;
        private TextComponent component;

        public BesideTextComponent(Entity target, Vector2 localOffset, float duration = 0.5f, float alphaMaxValue = 1)
        {
            _prefabs = UtilHelper.Prefabs();

            _target = target;
            _localOffset = localOffset;
            _duration = _isPermanent ? int.MaxValue : duration;
            _alphaMaxValue = alphaMaxValue;
            _alpha = _alphaMaxValue;

            _typingSounds = Enum.GetValues<SoundFxName>().Where(x => x.ToString().Contains("typing", StringComparison.OrdinalIgnoreCase)).ToList();

            //_typingSoundPrefab = _prefabs.GetSoundFxPrefab(SoundFxsEnum.Typing_1);
        }

        public override void OnAddedToEntity()
        {
            base.OnAddedToEntity();

            manager = UtilHelper.GameManager();

            text = this.Entity as BesideText;

            component = text.GetComponent<TextComponent>();

            if (component != null && component.Text.Length <= 40)
            {
                _duration *= 0.5f;
            }

            if (component != null && text.HasTypingTextEffect)
            {
                _originalText = component.Text;

                component.Text = string.Empty;

                text.Text = string.Empty;

                if (_originalText.Length >= 30)
                {
                    _typingRate *= 0.45f;
                }

                _timeLeftToNextChar = _typingRate;
            }

            if (_target != null)
            {
                Entity.SetPosition(new Vector2(_target.Position.X, _target.Position.Y) + _localOffset);
            }
        }

        public void Update()
        {
            if (Core.Scene == null)
            {
                return;
            }

            if (Entity == null)
            {
                return;
            }

            if (Entity.IsDestroyed)
            {
                return;
            }

            var deltaTime = Time.DeltaTime;

            var text = (BesideText)Entity;

            if (manager == null)
            {
                return;
            }

            if (manager != null && manager.IsGamePaused)
            {
                return;
            }

            if (component == null)
            {
                return;
            }

            if (!text.IsGoingUp)
            {
                switch (_target)
                {
                    case Creature creature:
                        if (creature != null && !creature.IsAlive)
                        {
                            text.Destroy();
                            this.RemoveComponent();
                            return;
                        }
                        break;
                }
            }

            if (!text.IsPermanent)
                _duration -= deltaTime;

            if (text.IsPermanent || _duration > 0)
            {
                float shakeFrequency = 3f;
                float shakeAmplitude = 0.5f;

                float time = Time.TotalTime * shakeFrequency;
                float offsetX = shakeAmplitude * (float)Math.Sin(time);
                float offsetY = shakeAmplitude * (float)Math.Cos(time);

                var shakeOffset = new Vector2(offsetX, offsetY);

                if (!text.IsPermanent)
                {
                    if (!text.ShouldWink)
                    {
                        if (_duration < 0.15f)
                        {
                            _alpha -= 5 * deltaTime;
                        }
                    }
                    else
                    {
                        if (_isFadingOut)
                        {
                            _alpha -= 2 * deltaTime;

                            if (_alpha <= 0)
                            {
                                _isFadingOut = false;
                            }
                        }
                        else
                        {
                            _alpha += 2 * deltaTime;

                            if (_alpha >= _alphaMaxValue)
                            {
                                _isFadingOut = true;
                            }
                        }
                    }
                }

                if (text.HasTypingTextEffect)
                {
                    if (_timeLeftToNextChar <= 0)
                    {
                        if (_actualChar < _originalText.Length)
                        {
                            if (_typingSoundPrefab != null)
                            {
                                var newVolume = _typingSoundPrefab.OriginalVolume;

                                newVolume *= (Nez.Random.Range(0.8f, 1f));

                                _typingSounds.Shuffle();
                                var rdmSound = _typingSounds[Nez.Random.RNG.Next(0, _typingSounds.Count)];
                                if (_prefabs != null && _canPlaySound)
                                    _prefabs.PlaySoundRandomPitch(rdmSound, 0.1f, newVolume);

                                _canPlaySound = !_canPlaySound;
                            }

                            component.Text += _originalText.Substring(_actualChar, 1);

                            _timeLeftToNextChar = _typingRate;

                            _actualChar++;
                        }
                    }
                    else
                    {
                        _timeLeftToNextChar -= deltaTime;
                    }
                }

                if (!text.IsPermanent)
                    component.Color = text.Color * _alpha;

                if (text.IsGoingUp)
                {
                    var vel = new Vector2(text.Position.X, text.Position.Y - _vel * Time.DeltaTime) + shakeOffset;
                    text.SetPosition(vel);
                }
                else
                {
                    if (_target != null)
                    {
                        var vel = 0.88f;

                        if (_target is Creature creature)
                        {
                            vel = creature.Speed * 0.0012f;
                        }

                        Entity.Position = Vector2.Lerp(Entity.Position + shakeOffset, _target.Position + _localOffset, vel);
                    }
                }
            }
            else
            {
                text.Destroy();
            }
        }
    }
}

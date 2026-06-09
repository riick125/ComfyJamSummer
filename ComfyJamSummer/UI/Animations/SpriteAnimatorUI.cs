using System;
using System.Collections.Generic;
using ComfyJamSummer.UI.Animations.Base;
using Microsoft.Xna.Framework;
using Nez;
using Nez.Sprites;
using Nez.Textures;
using Nez.UI;

namespace ComfyJamSummer.UI.Animations
{
    public class SpriteAnimatorUI : Component, IUpdatable
    {
        public enum LoopMode
        {
            Loop,
            Once,
            ClampForever,
            PingPong,
            PingPongOnce
        }

        public enum State
        {
            None,
            Running,
            Paused,
            Completed
        }

        /// <summary>
        /// fired when an animation completes, includes the animation name;
        /// </summary>
        public event Action<string> OnAnimationCompletedEvent;

        /// <summary>
        /// animation playback speed
        /// </summary>
        public float Speed = 1;

        /// <summary>
        /// the current state of the animation
        /// </summary>
        public State AnimationState { get; private set; } = State.None;

        /// <summary>
        /// the current animation
        /// </summary>
        public SpriteUIAnimation CurrentAnimation { get; private set; }

        public Image CurrentImage { get; private set; }

        /// <summary>
        /// the name of the current animation
        /// </summary>
        public string CurrentAnimationName { get; private set; }

        /// <summary>
        /// index of the current frame in sprite array of the current animation
        /// </summary>
        public int CurrentFrame { get; set; }

        /// <summary>
        /// checks to see if the CurrentAnimation is running
        /// </summary>
        public bool IsRunning => AnimationState == State.Running;

        /// <summary>
        /// Provides access to list of available animations
        /// </summary>
        public Dictionary<string, SpriteUIAnimation> Animations { get { return _animations; } }

        readonly Dictionary<string, SpriteUIAnimation> _animations = new Dictionary<string, SpriteUIAnimation>();

        float _elapsedTime;
        LoopMode _loopMode;

        SpriteRenderer _renderer;
        Sprite _sprite;

        public SpriteAnimatorUI()
        { }

        public SpriteAnimatorUI(Sprite sprite)
        {
            _renderer = new SpriteRenderer();

            _renderer.SetSprite(sprite);

            _sprite = sprite;
        }

        public virtual void Update()
        {
            if (AnimationState != State.Running || CurrentAnimation == null)
                return;

            var animation = CurrentAnimation;
            var secondsPerFrame = 1 / (animation.FrameRate * Speed);
            var iterationDuration = secondsPerFrame * animation.Sprites.Length;
            var pingPongIterationDuration = animation.Sprites.Length < 3 ? iterationDuration : secondsPerFrame * (animation.Sprites.Length * 2 - 2);

            _elapsedTime += Time.DeltaTime;
            var time = Math.Abs(_elapsedTime);

            // Once and PingPongOnce reset back to Time = 0 once they complete
            if (_loopMode == LoopMode.Once && time > iterationDuration ||
                _loopMode == LoopMode.PingPongOnce && time > pingPongIterationDuration)
            {
                AnimationState = State.Completed;
                _elapsedTime = 0;
                CurrentFrame = 0;
                //Sprite = animation.Sprites[0];
                //CurrentImage = animation.Images[0];
                OnAnimationCompletedEvent?.Invoke(CurrentAnimationName);
                return;
            }

            if (_loopMode == LoopMode.ClampForever && time > iterationDuration)
            {
                AnimationState = State.Completed;
                CurrentFrame = animation.Sprites.Length - 1;
                //Sprite = animation.Sprites[CurrentFrame];
                //CurrentImage = animation.Images[CurrentFrame];
                OnAnimationCompletedEvent?.Invoke(CurrentAnimationName);
                return;
            }

            // figure out which frame we are on
            int i = Mathf.FloorToInt(time / secondsPerFrame);
            int n = animation.Sprites.Length;
            if (n > 2 && (_loopMode == LoopMode.PingPong || _loopMode == LoopMode.PingPongOnce))
            {
                // create a pingpong frame
                int maxIndex = n - 1;
                CurrentFrame = maxIndex - Math.Abs(maxIndex - i % (maxIndex * 2));
            }
            else
                // create a looping frame
                CurrentFrame = i % n;

            //Sprite = animation.Sprites[CurrentFrame];
            CurrentImage = animation.Images[CurrentFrame];

            foreach (var img in animation.Images)
            {
                img.SetVisible(false);
            }

            animation.Images[CurrentFrame].SetVisible(true);
        }

        /// <summary>
        /// adds all the animations from the SpriteAtlas
        /// </summary>
        public SpriteAnimatorUI AddAnimationsFromAtlas(SpriteUIAtlas atlas)
        {
            for (var i = 0; i < atlas.AnimationNames.Length; i++)
            {
                _animations.Add(atlas.AnimationNames[i], atlas.SpriteAnimations[i]);
            }

            return this;
        }

        /// <summary>
        /// Adds a SpriteAnimation
        /// </summary>
        public SpriteAnimatorUI AddAnimation(string name, SpriteUIAnimation animation)
        {
            _animations[name] = animation;
            return this;
        }

        public SpriteAnimatorUI AddAnimation(string name, Sprite[] sprites, float fps = 10) => AddAnimation(name, fps, sprites);

        public SpriteAnimatorUI AddAnimation(string name, float fps, params Sprite[] sprites)
        {
            AddAnimation(name, new SpriteUIAnimation(sprites, fps));
            return this;
        }

        #region Playback

        /// <summary>
        /// plays the animation with the given name. If no loopMode is specified it is defaults to Loop
        /// </summary>
        public void Play(string name, LoopMode? loopMode = null)
        {
            CurrentAnimation = _animations[name];
            CurrentAnimationName = name;
            CurrentFrame = 0;
            AnimationState = State.Running;

            //Sprite = CurrentAnimation.Sprites[0];
            _elapsedTime = 0;
            _loopMode = loopMode ?? LoopMode.Loop;
        }

        /// <summary>
        /// checks to see if the animation is playing (i.e. the animation is active. it may still be in the paused state)
        /// </summary>
        public bool IsAnimationActive(string name) => CurrentAnimation != null && CurrentAnimationName.Equals(name);

        /// <summary>
        /// pauses the animator
        /// </summary>
        public void Pause() => AnimationState = State.Paused;

        /// <summary>
        /// unpauses the animator
        /// </summary>
        public void UnPause() => AnimationState = State.Running;

        /// <summary>
        /// stops the current animation and nulls it out
        /// </summary>
        public void Stop()
        {
            CurrentAnimation = null;
            CurrentAnimationName = null;
            CurrentFrame = 0;
            AnimationState = State.None;
        }

        public void SetPosition(Vector2 pos)
        {
            if (CurrentAnimation == null)
            {
                return;
            }

            foreach (var img in CurrentAnimation.Images)
            {
                img.SetPosition(pos.X, pos.Y);
            }
        }

        public void AddAllOnContainer(Container container)
        {
            if (CurrentAnimation == null || container == null)
            {
                return;
            }

            foreach (var img in CurrentAnimation.Images)
            {
                container.AddElement(img);
            }
        }

        #endregion
    }
}

using ComfyJamSummer.Components.General;
using ComfyJamSummer.Components.Visuals;
using ComfyJamSummer.Helpers;
using ComfyJamSummer.Manager;
using ComfyJamSummer.Prefab;
using Microsoft.Xna.Framework;
using Nez;
using Nez.Sprites;
using System.Collections.Generic;
using System.Linq;

namespace ComfyJamSummer.Entities.Base
{
    public class Animated : Entity
    {
        public Shadow Shadow { get; set; }

        public SpriteRenderer Renderer { get { return this.GetComponent<SpriteRenderer>(); } }

        public SpriteAnimator Animator { get { return this.GetComponent<SpriteAnimator>(); } }

        public SpriteRenderer GetAnyRenderer()
        {
            return Animator != null ? Animator : Renderer;
        }

        public UICanvas Canvas { get { return this.GetComponent<UICanvas>(); } }

        public CrazyScaleComponent CrazyScaleComponent { get { return this.GetComponent<CrazyScaleComponent>(); } }

        public ShakeComponent ShakeComponent { get { return this.GetComponent<ShakeComponent>(); } }

        public SimpleFlash SimpleFlash { get { return this.GetComponent<SimpleFlash>(); } }

        public LittleShake LittleShake { get { return this.GetComponent<LittleShake>(); } }

        private Prefabs _prefabs;
        public Prefabs Prefabs
        {
            get
            {
                if (_prefabs == null)
                {
                    _prefabs = UtilHelper.Prefabs(); ;
                }

                return _prefabs;
            }
        }

        private GameManager _gameManager;

        public GameManager GameManager
        {
            get
            {
                if (_gameManager == null)
                {
                    _gameManager = UtilHelper.GameManager();
                }

                return _gameManager;
            }
        }

        public int SpriteWidth { get; set; }

        public int SpriteHeight { get; set; }

        public float Alpha { get; set; }

        public float DepthHeight { get; set; }

        public float AlmostDisappearingAlpha { get; private set; }

        public List<SoundAnimator> SoundAnimators { get; set; }

        public class SoundAnimator
        {
            public string AnimationName { get; set; }

            public List<SoundPerFrame> SoundFrames { get; set; }

            public SoundAnimator(string animationName, List<SoundPerFrame> soundFrames)
            {
                AnimationName = animationName;
                SoundFrames = soundFrames;
            }
        }

        public class SoundPerFrame
        {
            public int ActualFrame { get; set; }

            public bool AlreadyPlayed { get; set; }

            public string SoundName { get; set; }

            public string AnimationName { get; set; }

            public bool AllowPitchChange => PitchMaxValue != 0;            

            public float PitchMaxValue { get; set; }
        }

        public Animated CloneAnimated(Vector2 pos)
        {
            var clone = base.Clone(pos) as Animated;
            clone.SpriteWidth = SpriteWidth;
            clone.SpriteHeight = SpriteHeight;
            clone.Alpha = 1f;
            clone.DepthHeight = DepthHeight;
            clone.AlmostDisappearingAlpha = 0.05f;
            clone.Shadow = new Shadow();

            if (SoundAnimators != null && SoundAnimators.Count > 0)
            {
                var transfer = new SoundAnimator[SoundAnimators.Count];
                SoundAnimators.CopyTo(0, transfer, 0, SoundAnimators.Count);

                clone.SoundAnimators = transfer.ToList();
            }

            return clone;
        }

        public override void OnAddedToScene()
        {
            base.OnAddedToScene();

            if (Shadow != null && Shadow.Scene == null)
            {
                this.Scene.AddEntity(Shadow);
            }

            AddComponent(new ShakeComponent());

            AddComponent(new CrazyScaleComponent());

            if (SoundAnimators != null)
            {
                AddComponent(new SoundAnimation(GameManager, Prefabs));
            }
        }

        public override void OnRemovedFromScene()
        {
            base.OnRemovedFromScene();

            if (Shadow != null && Shadow.Scene != null)
            {
                Shadow.Destroy();
            }
        }

        protected virtual bool Validate()
        {
            if (GameManager == null || GameManager.CantDoAnyAction)
                return false;

            if (Prefabs == null)
                return false;

            return true;
        }
    }
}
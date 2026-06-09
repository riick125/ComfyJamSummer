using ComfyJamSummer.Components.General;
using ComfyJamSummer.Components.Visuals;
using ComfyJamSummer.Helpers;
using ComfyJamSummer.Manager;
using ComfyJamSummer.Prefab;
using Microsoft.Xna.Framework;
using Nez;
using Nez.Sprites;

namespace ComfyJamSummer.Entities.Base
{
    public class Animated : Entity
    {
        public Shadow Shadow { get; set; }

        public SpriteRenderer Renderer { get { return this.GetComponent<SpriteRenderer>(); } }

        public SpriteAnimator Animator { get { return this.GetComponent<SpriteAnimator>(); } }

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
        public float AlmostDisappearingAlpha { get; private set; }

        public Animated CloneAnimated(Vector2 pos)
        {
            var clone = base.Clone(pos) as Animated;
            clone.Alpha = 1f;
            clone.AlmostDisappearingAlpha = 0.05f;
            clone.Shadow = new Shadow();

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
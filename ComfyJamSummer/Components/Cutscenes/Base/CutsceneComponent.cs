using ComfyJamSummer.Entities.Base;
using ComfyJamSummer.Extensions;
using ComfyJamSummer.Helpers;
using ComfyJamSummer.Prefab;
using ComfyJamSummer.UI;
using Nez;

namespace ComfyJamSummer.Components.Cutscenes.Base
{
    public class CutsceneComponent : SceneComponent
    {
        protected Prefabs _prefabs;

        public CutsceneComponent(float duration, string title = "", float executionTime = 1.75f, float startDelay = 0.25f, bool isSkippable = false)
        {
            _prefabs = UtilHelper.Prefabs();

            Duration = duration;
            TimeLeftToEndCutscene = duration;

            Start();

            if (!string.IsNullOrEmpty(title))
            {
                var presentationUI = Core.Scene?.CreateEntity("RoomEventPresentationUI");

                presentationUI.AddComponent(new TitlePresentationUI(title, executionTime, startDelay: startDelay));
            }

            HideShowPlayerUI(false);

            if (Core.Scene?.Camera != null)
            {
                var followCamera = Core.Scene.Camera.GetComponent<FollowCamera>();

                if (followCamera != null)
                {
                    followCamera.Enabled = false;
                }
            }
        }

        public float ElapsedTimeInState = 0f;

        public float Duration { get; private set; }

        public float TimeLeftToEndCutscene { get; private set; }

        public bool IsRunning { get; private set; }

        public bool IsPaused { get; set; }

        public void Start()
        {
            IsRunning = true;
        }

        public void Stop(bool removeCutscene = true)
        {
            UIHelper.DestroyCanvas<TitlePresentationUI>();

            IsRunning = false;

            ElapsedTimeInState = 0;

            if (removeCutscene)
                Core.Scene.RemoveSceneComponent(this);
        }

        public override void Update()
        {
            base.Update();

            if (IsPaused) return;

            if (IsRunning)
            {
                var deltaTime = Time.DeltaTime;

                ElapsedTimeInState += deltaTime;
                TimeLeftToEndCutscene -= deltaTime;
            }
        }

        public override void OnRemovedFromScene()
        {
            base.OnRemovedFromScene();

            HideShowPlayerUI(true);

            if (Core.Scene.Camera.Entity != null)
            {
                var followCamera = Core.Scene.Camera.GetComponent<FollowCamera>();

                if (followCamera != null)
                {
                    followCamera.Enabled = true;
                }
            }
        }

        public override void OnDisabled()
        {
            base.OnDisabled();

            HideShowPlayerUI(true);
        }

        private void HideShowPlayerUI(bool show)
        {
            var playerUI = UIHelper.GetPlayerUI();

            if (playerUI != null)
            {
                playerUI.Enabled = show;
            }
        }
    }
}
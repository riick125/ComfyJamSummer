using ComfyJamSummer.Entities.Base;
using Nez;
using System;

namespace ComfyJamSummer.Components.Cutscenes.Base
{
    public class CutscenePart : Animated
    {
        public T GetPartName<T>() where T : Enum
        {
            return (T)PartName;
        }

        public Enum PartName { get; set; }

        public float Duration { get; set; }

        public bool ManualFinish { get; set; }

        public float StartDelay { get; set; }

        public float TimeElapsed { get; private set; }

        public bool Finished { get; private set; }

        public bool IsActive { get; set; }

        public CutscenePart(Enum name, float duration = 0f, float startDelay = 0.05f)
        {
            PartName = name;
            Duration = duration;
            StartDelay = startDelay;

            if (duration <= 0)
            {
                ManualFinish = true;
            }
        }

        private void IncreaseTimeElapsed()
        {
            TimeElapsed += Time.DeltaTime;
        }

        public void Finish()
        {
            Finished = true;
            IsActive = false;
        }

        public override void Update()
        {
            base.Update();

            if (Finished)
            {
                return;
            }

            if (!IsActive)
            {
                return;
            }

            if (StartDelay > 0)
            {
                StartDelay -= Time.DeltaTime;
            }
            else
            {
                IncreaseTimeElapsed();

                if (!ManualFinish)
                {
                    if (TimeElapsed >= Duration)
                    {
                        Finish();
                    }
                }
            }
        }
    }
}
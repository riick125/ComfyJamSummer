using ComfyJamSummer.Entities.Base;
using Nez;
using System.Collections.Generic;
using System.Linq;

namespace ComfyJamSummer.Components.Cutscenes.Base
{
    public class CutscenePartManager : Component, IUpdatable
    {
        private Animated _manager;

        private bool _isRunning;

        private List<CutscenePart> _parts;

        private CutscenePart _actualPart;

        public CutscenePart ActualPart { get { return _actualPart; } }

        private int _actualIndex;

        public CutscenePartManager(List<CutscenePart> parts, bool autoStart = false)
        {
            _parts = parts;

            _parts.ForEach(x => Core.Scene.AddEntity(x));

            _isRunning = autoStart;

            _manager = Game1.CustomScene.CreateEntityCustom("CutscenePartManager");

            _manager.AddComponent(this);
        }

        public void Run()
        {
            if (_isRunning)
            {
                return;
            }

            _isRunning = true;

            _actualPart = _parts.FirstOrDefault();
            _actualPart.IsActive = true;
            _actualIndex = 0;
        }

        public void FinishPart()
        {
            if (_isRunning)
            {
                if (_actualPart != null)
                {
                    _actualPart.Finish();
                }
            }
        }

        public void Update()
        {
            if (this.Entity == null)
            {
                return;
            }

            if (this.Entity.IsDestroyed)
            {
                return;
            }

            if (!_isRunning)
            {
                return;
            }

            if (!_parts.Any())
            {
                this.Entity.Destroy();

                return;
            }

            if (_actualPart == null)
            {
                this.RemoveComponent();
                this.Entity.Destroy();

                return;
            }

            if (_actualPart.Finished)
            {
                GoToNextPart();
            }
        }

        private void GoToNextPart()
        {
            if (_actualIndex < _parts.Count - 1)
            {
                _actualIndex++;

                _actualPart = _parts[_actualIndex];
                _actualPart.IsActive = true;
            }
            else
            {
                DestroyEverything();
            }
        }

        public void DestroyEverything()
        {
            _parts.ForEach(x => x.Destroy());

            _actualPart = null;
            _parts = null;

            this.Entity.Destroy();
        }
    }
}
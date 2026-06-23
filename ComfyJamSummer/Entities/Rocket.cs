using ComfyJamSummer.Components.Cutscenes;
using ComfyJamSummer.Components.Visuals;
using ComfyJamSummer.Entities.Base;
using ComfyJamSummer.Entities.Configs;
using ComfyJamSummer.Enums;
using ComfyJamSummer.EventDatas;
using ComfyJamSummer.Helpers;
using ComfyJamSummer.UI;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Nez;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ComfyJamSummer.Entities
{
    public class Rocket : InteractableObject
    {
        public List<RocketBuildPhase> BuildPhases { get; set; }

        RocketBuildPhase _actualPhase;

        public int ActualProgress
        {
            get
            {
                if (BuildPhases == null)
                {
                    return 0;
                }

                if (_actualPhase == null)
                {
                    var phase = BuildPhases.OrderBy(x => Convert.ToInt32(x.PhaseName)).FirstOrDefault(x => !x.IsDone);

                    if (phase == null)
                    {
                        return 0;
                    }

                    return phase.ActualHits;
                }

                return _actualPhase.ActualHits;
            }
        }

        public int MaxProgress
        {
            get
            {
                if (BuildPhases == null)
                {
                    return 0;
                }

                if (_actualPhase == null)
                {
                    var phase = BuildPhases.OrderBy(x => Convert.ToInt32(x.PhaseName)).FirstOrDefault(x => !x.IsDone);

                    if (phase == null)
                    {
                        return 0;
                    }

                    return phase.HitsToNextPhase;
                }

                return _actualPhase.HitsToNextPhase;
            }
        }

        public Rocket CloneRocket(InteractableConfig config)
        {
            var clone = base.CloneInteractable(config) as Rocket;

            clone.AddComponent(new BoxCollider(clone.SpriteWidth, clone.SpriteHeight / 6f)
            {
                LocalOffset = new Vector2(0, clone.SpriteHeight / 3.5f),
                CollidesWithLayers = (int)CollisionLayer.Player,
                PhysicsLayer = (int)CollisionLayer.Map
            });

            AnimHelper.Play(clone.Animator, RocketAnim.Build_1);

            CreateBuildPhases(clone);

            clone.AddComponent(new FakeShadowComponent(12, SpriteHeight / 3, false));

            return clone;
        }
        public override void OnAddedToScene()
        {
            base.OnAddedToScene();


            CrabUI.Emitter?.Emit(UIEvent.UpdateBuildBar, new UIEventData() { Target = this });
        }
        void CreateBuildPhases(Rocket clone)
        {
            var totalPhases = Enum.GetValues<RocketAnim>().Where(x => x.ToString()
            .Contains("build", StringComparison.InvariantCultureIgnoreCase));

            clone.BuildPhases = new List<RocketBuildPhase>();

            var hitsPerPhase = 85;
            var hitsGrowPerPhase = 40;

            foreach (var item in totalPhases)
            {
                var phase = new RocketBuildPhase(hitsPerPhase, item);

                clone.BuildPhases.Add(phase);

                hitsPerPhase += hitsGrowPerPhase;
            }
        }

        public override void Update()
        {
            base.Update();
            InteractText.SetEnabled(BuildPhases.All(x => x.IsDone));
        }

        public void ProgressBuild()
        {
            if (BuildPhases == null)
            {
                return;
            }

            if (!BuildPhases.Any())
            {
                return;
            }

            if (_actualPhase == null)
            {
                _actualPhase = BuildPhases.OrderBy(x => Convert.ToInt32(x.PhaseName)).FirstOrDefault(x => !x.IsDone);

                if (BuildPhases.All(x => x.IsDone))
                {
                    AnimHelper.Play(Animator, RocketAnim.Done);
                    return;
                }
            }
            else
            {
                _actualPhase.ActualHits++;

                CrabUI.Emitter?.Emit(UIEvent.UpdateBuildBar, new UIEventData() { Target = this });

                if (_actualPhase.IsDone)
                {
                    if (_actualPhase.PhaseName < RocketAnim.Build_3)
                    {
                        AnimHelper.Play(Animator, _actualPhase.PhaseName + 1);
                    }
                    else
                    {
                        AnimHelper.Play(Animator, RocketAnim.Done);
                    }

                    _actualPhase = null;
                }
            }
        }

        public void Launch(Player player)
        {
            if (IsInteracting)
            {
                return;
            }

            if (BuildPhases == null)
            {
                return;
            }

            if (!BuildPhases.All(x => x.IsDone))
            {
                return;
            }

            if (CanPressInteractButton && Input.IsKeyPressed(Keys.E))
            {
                StartInteractingWithPlayer();

                player.Gun?.SetEnabled(false);

                Scene.AddSceneComponent(new EscapeCutscene(60, "YOU WIN!"));
            }
        }
    }

    public class RocketBuildPhase
    {
        public int HitsToNextPhase { get; private set; }

        public int ActualHits { get; set; }

        public RocketAnim PhaseName { get; set; }

        public bool IsDone => ActualHits >= HitsToNextPhase;

        public RocketBuildPhase(int hitsToNextPhase, RocketAnim phaseName)
        {
            HitsToNextPhase = hitsToNextPhase;
            PhaseName = phaseName;
        }
    }
}
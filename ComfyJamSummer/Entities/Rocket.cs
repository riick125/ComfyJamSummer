using ComfyJamSummer.Components.Cutscenes;
using ComfyJamSummer.Components.Visuals;
using ComfyJamSummer.Entities.Base;
using ComfyJamSummer.Entities.Configs;
using ComfyJamSummer.Enums;
using ComfyJamSummer.Helpers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended.Collisions.Layers;
using Nez;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;

namespace ComfyJamSummer.Entities
{
    public class Rocket : InteractableObject
    {
        public List<RocketBuildPhase> BuildPhases { get; set; }

        RocketBuildPhase _actualPhase;

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

        void CreateBuildPhases(Rocket clone)
        {
            var totalPhases = Enum.GetValues<RocketAnim>().Where(x => x.ToString()
            .Contains("build", StringComparison.InvariantCultureIgnoreCase));

            clone.BuildPhases = new List<RocketBuildPhase>();

            var hitsPerPhase = 2;// 75;
            var hitsGrowPerPhase = 0;// 50;

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

#if DEBUG
            if (_actualPhase != null)
            {
                Debug.DrawText(UtilHelper.CustomFont().FontNormal, _actualPhase.ActualHits + "/" + _actualPhase.HitsToNextPhase, position: this.Position, Color.White);
            }
#endif
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

                if (_actualPhase == null)
                {
                    return;
                }
            }
            else
            {
                _actualPhase.ActualHits++;

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
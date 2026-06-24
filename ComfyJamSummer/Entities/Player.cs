using ComfyJamSummer.Components.Gameplay;
using ComfyJamSummer.Components.Visuals;
using ComfyJamSummer.Entities.Base;
using ComfyJamSummer.Entities.Collectibles;
using ComfyJamSummer.Entities.Configs;
using ComfyJamSummer.Entities.Creatures;
using ComfyJamSummer.Enums;
using ComfyJamSummer.Helpers;
using Microsoft.Xna.Framework;
using Nez;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ComfyJamSummer.Entities
{
    public class Player : Creature
    {
        public float TimeLeftToBeAttackable { get; set; }

        public float ImmuneTime { get; set; }

        public bool IsImmune => TimeLeftToBeAttackable > 0;

        public Gun Gun { get; set; }

        public FriedChicken FriedChicken { get; set; }

        public Sandwich Sandwich { get; set; }

        public int DevouredSandwiches { get; set; }

        public int Points { get; set; }

        public Queue<int> PendingPoints { get; set; }

        public Vector2 OffsetCollectible { get; set; }

        public CircleCollider InteractAreaCollider { get { return this.GetComponents<CircleCollider>().FirstOrDefault(x => x.Tag == CreatureCollider.InteractArea.ToString()); } }


        float _collectibleFollowSpeed;

        float _pendingPointsProcessCd = 0.004f, _timeLeftToNextProcess;

        public Player ClonePlayer(PlayerConfig config)
        {
            var clone = base.CloneCreature(config) as Player;
            clone.ImmuneTime = 0.3f;
            clone.PendingPoints = new Queue<int>();
            clone._pendingPointsProcessCd = _pendingPointsProcessCd;
            clone.Name = EntityNames.PLAYER;

            clone.AddComponent(new PlayerController(UtilHelper.GameManager(), UtilHelper.Prefabs()));

            AnimHelper.Play(clone.Animator, CreatureAnim.Idle);

            clone.AddComponent(new FakeShadowComponent(8, clone.SpriteHeight / 3.4f));

            clone.OffsetCollectible = new Vector2(clone.SpriteWidth / 1.5f, -clone.SpriteHeight / 4);

            clone.AddComponent(new CircleCollider(5) { IsTrigger = true, Tag = CreatureCollider.InteractArea.ToString() });

            return clone;
        }

        public override void OnAddedToScene()
        {
            base.OnAddedToScene();

            if (Gun != null && Gun.Scene == null)
            {
                this.Scene.AddEntity(Gun);
            }
        }

        public override void Update()
        {
            base.Update();

            if (!IsAlive)
            {
                SpiralDisappear(false);
            }
            else
            {
                if (TimeLeftToBeAttackable > 0)
                {
                    TimeLeftToBeAttackable -= Time.DeltaTime;
                }
            }

            if (!Validate())
            {
                return;
            }

            DragCollectible(FriedChicken);

            DragCollectible(Sandwich);

            ProcessPendingPoints();
        }
        void ProcessPendingPoints()
        {
            if (_timeLeftToNextProcess > 0)
            {
                _timeLeftToNextProcess -= Time.DeltaTime;
                return;
            }

            if (!PendingPoints.Any())
                return;

            var pendingPoint = PendingPoints.Peek();

            var portion = Math.Max(1, pendingPoint / 10);

            _timeLeftToNextProcess = _pendingPointsProcessCd;

            if (portion >= pendingPoint)
            {
                Points += pendingPoint;
                PendingPoints.Dequeue();
            }
            else
            {
                var remaining = pendingPoint - portion;
                Points += portion;

                PendingPoints.Dequeue();
                PendingPoints.Enqueue(remaining);
            }
        }

        void DragCollectible(Collectible collectible)
        {
            var deltaTime = Time.DeltaTime;

            if (collectible != null && collectible.IsCollected)
            {
                _collectibleFollowSpeed = Speed * 0.04f;

                collectible.Position = Vector2.Lerp(collectible.Position, this.Position + OffsetCollectible, _collectibleFollowSpeed * deltaTime);
            }
        }
    }
}
using ComfyJamSummer.Components.General;
using ComfyJamSummer.Components.Visuals;
using ComfyJamSummer.Configs;
using ComfyJamSummer.Entities.Base;
using ComfyJamSummer.Entities.Configs;
using ComfyJamSummer.Entities.Debuffs;
using ComfyJamSummer.Enums;
using ComfyJamSummer.EventDatas;
using ComfyJamSummer.Helpers;
using ComfyJamSummer.UI;
using Microsoft.Xna.Framework;
using Nez;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ComfyJamSummer.Entities.Creatures
{
    public class Creature : Actor
    {
        public Island Island { get; private set; }
        public uint IslandId { get; set; }

        public string CreatureName { get; set; }

        public float Damage { get; set; }

        public float CriticalChance { get; set; }

        public float TimeLeftToNextAtk { get; set; }

        public float AtkSpeed { get; set; }

        public List<Debuff> DebuffsToGive { get; set; }

        float _dyingRotationSpeed = 1300f, _losingScaleSpeed = 1.75f, _losingColorSpeed = 4f;

        List<Vector2> _originalMoveDirections, _moveDirections;

        Vector2[] _directionTransfer;

        public bool GoingToDestination { get; set; }
        public Vector2 GoingDirection { get; set; }
        public Vector2 PerpendicularDirection { get; set; }
        public Vector2 GoingDestination { get; set; }
        public float TimeLeftToNextPatrol { get; set; }

        public float PatrolCooldown { get; set; }

        public float DyingRotationSpeed { get => _dyingRotationSpeed; }

        public float LosingScaleSpeed { get => _losingScaleSpeed; }

        public float LosingColorSpeed { get => _losingColorSpeed; }

        public Vector2 MinPosition
        {
            get
            {
                if (Island == null)
                {
                    return default;
                }

                var minX = Island.CenterPosition().X - Island.Width * 0.8f;
                var minY = Island.CenterPosition().Y - Island.Height * 0.8f;

                return new Vector2(minX, minY);
            }
        }

        public Vector2 MaxPosition
        {
            get
            {
                if (Island == null)
                {
                    return default;
                }

                var maxX = Island.CenterPosition().X + Island.Width * 0.8f;
                var maxY = Island.CenterPosition().Y - Island.Height / 2.5f;

                return new Vector2(maxX, maxY);
            }
        }

        public float DistanceLimitInSpriteSize { get; set; }

        float _safeDistancePatrol;

        public enum CreatureStates
        {
            Idle,
            Walking,
            Attacking
        }

        public Creature CloneCreature(CreatureConfig config)
        {
            var clone = base.CloneAnimated(config.Position) as Creature;
            clone._safeDistancePatrol = clone.SpriteWidth * 0.8f;
            clone.DistanceLimitInSpriteSize = clone.SpriteWidth * 2;
            clone.IslandId = config.IslandId;
            clone._dyingRotationSpeed = _dyingRotationSpeed * Nez.Random.Range(0.85f, 1.25f);
            clone._losingScaleSpeed = _losingScaleSpeed * Nez.Random.Range(0.95f, 1.5f);
            clone._losingColorSpeed = _losingColorSpeed;
            clone.ActualHP = config.HP;
            clone.MaxHP = config.HP;
            clone.Damage = config.Damage;
            clone.CriticalChance = config.CriticalChance;
            clone.Speed = config.Speed * Nez.Random.Range(0.91f, 1.04f);
            clone.AtkSpeed = config.AtkSpeed * Nez.Random.Range(0.95f, 1.15f);
            clone.PatrolCooldown = config.PatrolCooldown * Nez.Random.Range(0.95f, 1.15f);
            clone.DebuffsToGive = new List<Debuff>();
            clone.AddComponent(new SimpleFlash(Game1.FlashMaterial, clone.Animator == null ? clone.Renderer : clone.Animator));

            clone._originalMoveDirections = new List<Vector2>() { new Vector2(1, 0),
                new Vector2(1, -1),
                new Vector2(0, -1),
                new Vector2(-1, -1),
                new Vector2(-1, 0),
                new Vector2(-1, 1),
                new Vector2(0, 1),
                new Vector2(1, 1)
            };

            clone._moveDirections = new List<Vector2>();

            clone._directionTransfer = new Vector2[clone._originalMoveDirections.Count];

            return clone;
        }

        public override void OnAddedToScene()
        {
            base.OnAddedToScene();

            Island = this.Scene.EntitiesOfType<Island>().FirstOrDefault(x => x.Id == this.IslandId);
        }

        public void SpiralDisappear(bool destroy = true)
        {
            var deltaTime = Time.DeltaTime;

            RotationDegrees += DyingRotationSpeed * deltaTime;

            var scale = Scale.X;

            scale -= LosingScaleSpeed * deltaTime;

            scale = Mathf.Clamp01(scale);

            this.SetScale(scale);

            if (Shadow != null)
            {
                if (HasComponent<FakeShadowComponent>())
                {
                    RemoveComponent<FakeShadowComponent>();
                }

                Shadow.RotationDegrees = RotationDegrees;
                Shadow.SetScale(scale);
            }

            if (scale < 0.25f)
            {
                Alpha -= LosingColorSpeed * deltaTime;

                Alpha = Mathf.Clamp01(Alpha);

                Shadow.Alpha = Alpha;

                GetAnyRenderer().SetColor(Color.White * Alpha);

                Shadow.GetAnyRenderer().SetColor(Color.White * Alpha);

                if (scale <= 0)
                {
                    if (destroy && !IsDestroyed && Scene != null)
                    {
                        Destroy();
                    }
                }
            }
        }

        public void Idle()
        {
            AnimHelper.Play(Animator, CreatureAnim.Idle);
        }

        public void Patrol()
        {
            if (!Validate() || _originalMoveDirections == null)
                return;

            var island = UtilHelper.GetEntity<Island>();

            if (island == null)
                return;

            var deltaTime = Time.DeltaTime;

            var distanceMinX = Math.Abs(this.Position.X - MinPosition.X);
            var distanceMaxX = Math.Abs(this.Position.X - MaxPosition.X);

            var distanceMinY = Math.Abs(this.Position.Y - MinPosition.Y);
            var distanceMaxY = Math.Abs(this.Position.Y - MaxPosition.Y);

            var brokeMinLimitX = distanceMinX <= DistanceLimitInSpriteSize;
            var brokeMaxLimitX = distanceMaxX <= DistanceLimitInSpriteSize;

            var brokeMinLimitY = distanceMinY <= DistanceLimitInSpriteSize;
            var brokeMaxLimitY = distanceMaxY <= DistanceLimitInSpriteSize;

            if (!GoingToDestination)
            {
                if (TimeLeftToNextPatrol <= 0)
                {
                    _originalMoveDirections.CopyTo(_directionTransfer);
                    _moveDirections = _directionTransfer.ToList();

                    if (brokeMinLimitX)
                    {
                        _moveDirections.Remove(new Vector2(-1, -1));
                        _moveDirections.Remove(new Vector2(-1, 0));
                        _moveDirections.Remove(new Vector2(-1, 1));
                    }
                    else if (brokeMaxLimitX)
                    {
                        _moveDirections.Remove(new Vector2(1, -1));
                        _moveDirections.Remove(new Vector2(1, 0));
                        _moveDirections.Remove(new Vector2(1, 1));
                    }

                    if (brokeMinLimitY)
                    {
                        _moveDirections.Remove(new Vector2(-1, 0));
                        _moveDirections.Remove(new Vector2(1, 0));
                        _moveDirections.Remove(new Vector2(1, -1));
                        _moveDirections.Remove(new Vector2(-1, -1));
                        _moveDirections.Remove(new Vector2(0, -1));
                    }
                    else if (brokeMaxLimitY)
                    {
                        _moveDirections.Remove(new Vector2(-1, 0));
                        _moveDirections.Remove(new Vector2(1, 0));
                        _moveDirections.Remove(new Vector2(0, 1));
                        _moveDirections.Remove(new Vector2(1, 1));
                        _moveDirections.Remove(new Vector2(-1, 1));
                    }

                    if (_moveDirections.Any())
                    {
                        _moveDirections.Shuffle();

                        GoingDirection = _moveDirections[Nez.Random.Range(0, _moveDirections.Count)];

                        if (GoingDirection != default)
                        {
                            GoingDestination = this.Position + new Vector2(SpriteWidth * Nez.Random.Range(2.1f, 3.8f)) * GoingDirection;
                            PerpendicularDirection = DirectionHelper.PerpendicularDirection(this.Position, GoingDestination);

                            GoingToDestination = true;
                        }
                    }
                }
            }
            else
            {
                var pos = Move();

                if (Vector2.Distance(this.Position, GoingDestination) <= SpriteWidth * 0.4f)
                {
                    ResetPatrol();
                    return;
                }
                else
                {
                    if (GoingDirection.X < 0 && distanceMinX <= _safeDistancePatrol || GoingDirection.X > 0 && distanceMaxX <= _safeDistancePatrol)
                    {
                        ResetPatrol();
                        return;
                    }

                    if (GoingDirection.Y < 0 && distanceMinY <= _safeDistancePatrol || GoingDirection.Y > 0 && distanceMaxY <= _safeDistancePatrol)
                    {
                        ResetPatrol();
                        return;
                    }
                }

                this.Position = pos;

                AnimHelper.Play(Animator, CreatureAnim.Walk);
            }
        }

        void ResetPatrol()
        {
            PerpendicularDirection = default;
            GoingDirection = default;
            GoingToDestination = false;

            TimeLeftToNextPatrol = PatrolCooldown * Nez.Random.Range(0.92f, 1.25f);
        }

        Vector2 Move()
        {
            var deltaTime = Time.DeltaTime;

            var pos = this.Position;

            var vel = Vector2.Zero;

            vel += PerpendicularDirection * Speed * deltaTime;

            pos += vel;

            pos = ClampPosition(pos);

            return pos;
        }

        public Vector2 ClampPosition(Vector2 pos)
        {
            pos.X = Mathf.Clamp(pos.X, MinPosition.X, MaxPosition.X);
            pos.Y = Mathf.Clamp(pos.Y, MinPosition.Y, MaxPosition.Y);

            return pos;
        }

        public virtual Vector2 Stalk(Creature target)
        {
            if (target == null)
                return this.Position;

            if (target.BodyCollider == null)
                return this.Position;

            if (target.Id == this.Id || !target.IsAlive)
                return this.Position;

            var direction = target.Position - this.Position;
            direction.Normalize();

            var vel = Vector2.Zero;

            vel += direction * Speed * Time.DeltaTime;

            if (DirectionHelper.ValidateVelocity(direction, vel))
            {
                AnimHelper.Play(Animator, CreatureAnim.Walk);

                var pos = this.Position;
                pos += vel;

                return pos;
            }

            return this.Position;
        }

        public void Heal(float amount)
        {
            Prefabs?.PlaySoundRandomPitch(SoundFxName.Heal, 0.15f);

            ActualHP += amount;

            ActualHP = Mathf.Clamp(ActualHP, 0, MaxHP);

            var textOffset = new Vector2(SpriteWidth * (Nez.Random.Chance(50) ? 1 : -1), -SpriteHeight / 4);

            var config = new BesideTextConfig(this, $"+{(int)amount}", offset: textOffset, color: Color.Green);
            TextHelper.CreateGoingUpBesideText(config);

            if (this is Player)
            {
                PlayerUI.Emitter.Emit(UIEvent.HealBar, new UIEventData() { Target = this });
            }
        }

        public bool TakeDamage(float dmg, Bullet bullet = null)
        {
            if (!Validate() || !IsAlive)
            {
                return false;
            }

            if (dmg < 1)
            {
                dmg = 1;
            }

            var isCritical = false;

            if (bullet != null)
            {
                isCritical = Nez.Random.Chance(bullet.CriticalChance);

                dmg *= isCritical ? 1.5f : 1f; 
            }

            dmg = float.Round(dmg);

            PreviousHP = ActualHP;

            ActualHP -= dmg;
            ActualHP = Mathf.Clamp(ActualHP, 0, MaxHP);
            LastReduceValueTaken = dmg;

            var prefabs = UtilHelper.Prefabs();
            if (prefabs != null)
            {
                prefabs.PlaySoundRandomPitch(SoundFxName.Hit, 0.5f);

                var offset = Vector2.Zero;

                offset.X = Nez.Random.Range(1, SpriteWidth * 0.6f) * Nez.Random.MinusOneToOne();
                offset.Y = Nez.Random.Range(1, SpriteHeight * 0.6f) * Nez.Random.MinusOneToOne();

                var poof = prefabs.Poof.ClonePoof(this.Position + offset);
                poof.Scale /= 1.8f;
                poof.Animator.Speed = 2;
                this.Scene.AddEntity(poof);
            }

            if (this is Player)
                PlayerUI.Emitter.Emit(UIEvent.ReduceBar, new UIEventData() { Target = this });

            var textOffset = new Vector2(SpriteWidth * (Nez.Random.Chance(50) ? 1 : -1), -SpriteHeight / 4);

            var config = new BesideTextConfig(this, $"-{dmg}", offset: textOffset, color: Color.Red);
            TextHelper.CreateGoingUpBesideText(config);

            if (isCritical)
            {
                Core.Schedule(0.01f, t =>
                {
                    Prefabs?.PlaySoundRandomPitch(SoundFxName.Bash, 0.25f);

                    var textOffset = new Vector2(SpriteWidth * (Nez.Random.Chance(50) ? 1 : -1), -SpriteHeight / 4);

                    var config = new BesideTextConfig(this, $"Critical!", offset: textOffset, color: Constants.YELLOW_COLOR, duration: 1.5f);
                    TextHelper.CreateGoingUpBesideText(config);

                    t.Stop();
                });
            }

            if (!IsAlive)
            {
                AnimHelper.Play(Animator, CreatureAnim.Dying);

                prefabs?.PlaySoundRandomPitch(SoundFxName.Flesh, 0.04f);
            }

            CrazyScaleComponent?.SqueezeByDirection(ComfyJamSummer.Components.General.CrazyScaleComponent.SqueezeDirection.Horizontal);

            SimpleFlash?.Flash(0.25f);

            if (!IsAlive)
            {
                RemoveComponent<CrazyScaleComponent>();
            }

            return true;
        }
    }
}
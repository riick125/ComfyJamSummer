using ComfyJamSummer.Entities.Base;
using ComfyJamSummer.Entities.Configs;
using ComfyJamSummer.Entities.Creatures;
using ComfyJamSummer.Enums;
using ComfyJamSummer.Helpers;
using Microsoft.Xna.Framework;
using Nez;
using System.Collections.Generic;
using System.Linq;

namespace ComfyJamSummer.Entities
{
    public class Bullet : Animated
    {
        public OffensiveEffectCollisionEnum Target { get; set; }

        public float Damage { get; set; }

        public float CriticalChance { get; set; }

        public float Speed { get; set; }

        public bool Collided { get; set; }

        public float LifeTime { get; set; }

        public Vector2 Direction { get; set; }

        public List<uint> ObjectsHittedByBullet { get; set; }
        public bool CreateImpactEffect { get; set; }

        float _losingColorSpeed = 8f;

        public Bullet CloneBullet(BulletConfig config)
        {
            if (config == null)
            {
                return null;
            }

            var clone = base.CloneAnimated(config.Position) as Bullet;

            if (clone.Animator != null)
            {
                clone.Animator.FlipX = config.Direction.X < 0;
            }

            HydrateValues(clone, config);

            AnimHelper.Play(clone.Animator, BulletAnim.Idle);

            return clone;
        }

        public Bullet CloneBulletEnemy(BulletConfig config)
        {
            if (config == null)
            {
                return null;
            }

            var clone = base.CloneAnimated(config.Position) as Bullet;

            HydrateValues(clone, config);

            clone.Speed *= Nez.Random.Range(0.91f, 1.02f);

            return clone;
        }

        private void HydrateValues(Bullet clone, BulletConfig config)
        {
            clone._losingColorSpeed = _losingColorSpeed;
            clone.Target = config.Target;
            clone.Speed = config.Speed;
            clone.Damage = config.Damage;
            clone.CriticalChance = config.CriticalChance;
            clone.LifeTime = config.LifeTime;
            clone.Rotation = config.Rotation;
            clone.ObjectsHittedByBullet = new List<uint>();
            clone.Direction = SpreadBullet(config.Direction, config.MaxAngleSpread);

            clone.CreateImpactEffect = config.CreateImpactEffect;
        }

        public override void Update()
        {
            base.Update();

            if (!Validate())
                return;

            var deltaTime = Time.DeltaTime;

            if (Collided)
            {
                AnimHelper.Play(Animator, BulletAnim.Collision);

                Alpha -= _losingColorSpeed * deltaTime;

                GetAnyRenderer().SetColor(Color.White * Alpha);

                if (Alpha <= 0)
                {
                    this.Destroy();
                }
                return;
            }

            LifeTime -= deltaTime;

            if (LifeTime <= 0)
            {
                Collided = true;
            }
            else
            {
                var vel = Vector2.Zero;

                vel += Direction * Speed * deltaTime;

                if (float.IsNaN(Direction.X) || float.IsNaN(Direction.Y))
                {
                    Collided = true;
                }
                else if (float.IsInfinity(vel.X) || float.IsInfinity(vel.Y))
                {
                    Collided = true;
                }
                else
                {
                    this.Position += vel;

                    var hits = new Collider[8];

                    Physics.OverlapCircleAll(this.Position, 5, hits);

                    hits = hits.Where(x => x != null && x.Entity != null && x.Entity.Id != Id).ToArray();

                    foreach (var hit in hits)
                    {
                        if (ObjectsHittedByBullet.Contains(hit.Entity.Id))
                        {
                            continue;
                        }

                        ObjectsHittedByBullet.Add(hit.Entity.Id);

                        var wasHit = false;

                        switch (hit.Entity)
                        {
                            case Player player:
                                if (Target == OffensiveEffectCollisionEnum.Enemy)
                                {
                                    continue;
                                }

                                wasHit = player.TakeDamage(Damage, this);
                                break;

                            case Enemy enemy:
                                if (Target == OffensiveEffectCollisionEnum.Player)
                                {
                                    continue;
                                }

                                wasHit = enemy.TakeDamage(Damage, this);
                                break;
                        }

                        Collided = wasHit;
                    }
                }
            }
        }
        Vector2 SpreadBullet(Vector2 dir, float maxAngle)
        {
            float spreadAngle = Random.Range(-(maxAngle + Nez.Random.Range(1.35f, 2.5f)), (maxAngle + Nez.Random.Range(1.35f, 2.75f)));

            float rotateAngle = spreadAngle + Mathf.Atan2(dir.Y, dir.X) * Mathf.Rad2Deg;

            var newDirection = new Vector2(Mathf.Cos(rotateAngle * Mathf.Deg2Rad), Mathf.Sin(rotateAngle * Mathf.Deg2Rad));
            newDirection.Normalize();

            return newDirection;
        }
    }
}
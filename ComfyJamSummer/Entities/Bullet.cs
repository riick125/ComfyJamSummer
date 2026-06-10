using ComfyJamSummer.Entities.Base;
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

        public float Speed { get; set; }

        public bool Collided { get; set; }

        public float LifeTime { get; set; }

        public Vector2 Direction { get; set; }

        public List<uint> ObjectsHittedByBullet { get; set; }
        public bool CreateImpactEffect { get; set; }

        public Bullet CloneBullet(Gun gun, OffensiveEffectCollisionEnum target, Vector2 direction, float rotation, bool createImpactEffect = true)
        {
            var clone = base.CloneAnimated(gun.MuzzlePosition) as Bullet;

            clone.ObjectsHittedByBullet = new List<uint>();
            clone.Target = target;
            clone.Speed = gun.BulletSpeed;
            clone.Damage = gun.Damage;
            clone.LifeTime = LifeTime;
            clone.Rotation = rotation;
            clone.ObjectsHittedByBullet = new List<uint>();
            clone.Direction = SpreadBullet(direction, gun.MaxAngleSpread);

            clone.CreateImpactEffect = createImpactEffect;

            return clone;
        }

        public override void Update()
        {
            base.Update();

            if (Collided)
            {
                AnimHelper.Play(Animator, BulletAnim.Collision);

                return;
            }

            var deltaTime = Time.DeltaTime;

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

                        switch (hit.Entity)
                        {
                            case Player player:
                                if (Target == OffensiveEffectCollisionEnum.Enemy)
                                {
                                    continue;
                                }

                                player.TakeDamage(Damage);
                                break;

                            case Enemy enemy:
                                if (Target == OffensiveEffectCollisionEnum.Player)
                                {
                                    continue;
                                }

                                enemy.TakeDamage(Damage);
                                break;
                        }
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
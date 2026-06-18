using ComfyJamSummer.Entities;
using ComfyJamSummer.Enums;
using ComfyJamSummer.Helpers;
using ComfyJamSummer.Manager;
using ComfyJamSummer.Prefab;
using Microsoft.Xna.Framework;
using Nez;
using System.Linq;

namespace ComfyJamSummer.AI.Enemies
{
    public class CrabPissedOffState : BaseAIState<Crab>
    {
        float _speed;

        float _atkCd = 1f, _timeLeftToNextAtk;

        CircleCollider _collider;

        public CrabPissedOffState(Prefabs prefabs, GameManager manager) : base(prefabs, manager)
        {
        }

        public override void Begin()
        {
            base.Begin();

            var collider = _context.GetComponents<Collider>().FirstOrDefault(x => x.Tag == CreatureCollider.Body.ToString());

            if (collider == null)
            {
                _collider = _context.AddComponent(new CircleCollider(8) { Tag = CreatureCollider.Body.ToString() });
            }
        }

        public override void Update(float deltaTime)
        {
            base.Update(deltaTime);

            if (!Validate())
            {
                return;
            }

            AnimHelper.Play(_context.Animator, CrabAnim.Walk);

            var player = UtilHelper.Player();

            if (player == null)
            {
                return;
            }

            _context.Animator.FlipX = player.Position.X < _context.Position.X;

            _speed = player.Speed * 1.2f;

            var direction = player.Position - _context.Position;
            direction.Normalize();

            var vel = Vector2.Zero;

            vel += direction * _speed * Time.DeltaTime;

            if (DirectionHelper.ValidateVelocity(direction, vel))
            {
                _context.Position += vel;
            }

            if (player.BodyCollider != null && _collider != null && _collider.Overlaps(player.BodyCollider))
            {
                if (_timeLeftToNextAtk <= 0)
                {
                    player.TakeDamage(player.MaxHP * 0.17f);
                    _timeLeftToNextAtk = _atkCd;
                }
            }

            if (_timeLeftToNextAtk > 0)
            {
                _timeLeftToNextAtk -= deltaTime;
            }
        }
    }
}
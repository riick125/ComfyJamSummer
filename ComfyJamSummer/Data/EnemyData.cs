using AutoMapper;
using ComfyJamSummer.Entities;
using ComfyJamSummer.Entities.Configs;
using ComfyJamSummer.Enums;
using ComfyJamSummer.Prefab;
using Microsoft.Xna.Framework;

namespace ComfyJamSummer.Data
{
    public class EnemyData : BaseData
    {
        public EnemyData(Prefabs prefabs, IMapper mapper) : base(Constants.ENEMY_DATA_PATH, prefabs, mapper)
        {
        }

        public Enemy Create()
        {
            var enemy = CreateDummyRenderer<Enemy>(12, 12, Color.DarkRed, Constants.CREATURE_RENDER_LAYER);

            return enemy;
        }

        public Bullet CreateBullet()
        {
            var bullet = CreateDummyRenderer<Bullet>(6, 6, Color.Red, Constants.CREATURE_RENDER_LAYER - 1);

            return bullet;
        }

        public EnemyConfig InitializeEnemy(EnemyType type, Vector2 pos)
        {
            var maxHp = PlayerValues.HP;
            var speed = PlayerValues.SPEED;

            return _prefabs.EnemyConfig.CloneEnemy(type, maxHp, 0, speed, 0, pos, ColliderType.Player);
        }
    }
}

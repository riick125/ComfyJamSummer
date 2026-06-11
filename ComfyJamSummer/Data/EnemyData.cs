using AutoMapper;
using ComfyJamSummer.Entities;
using ComfyJamSummer.Entities.Configs;
using ComfyJamSummer.Enums;
using ComfyJamSummer.Helpers;
using ComfyJamSummer.Prefab;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;

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

        /// <summary>
        /// base values for enemies
        /// </summary>
        /// <returns></returns>
        public List<EnemyConfig> CreateAllBaseConfigs()
        {
            var result = new List<EnemyConfig>();

            var allEnemies = Enum.GetValues<EnemyType>();

            foreach (var type in allEnemies)
            {
                var config = new EnemyConfig()
                {
                    Type = type,
                    HP = EnemyValues.HP,
                    Damage = EnemyValues.DMG,
                    Speed = EnemyValues.SPEED,
                    BulletSpeed = EnemyValues.BULLET_SPEED,
                    AtkSpeed = EnemyValues.ATK_SPEED,
                    IdleTimeState = EnemyValues.IDLE_TIME_STATE,
                    MoveTimeState = EnemyValues.MOVE_TIME_STATE,
                    PatrolTimeState = EnemyValues.PATROL_TIME_STATE,
                    PatrolCooldown = EnemyValues.PATROL_COOLDOWN,
                    AtkTimeState = EnemyValues.ATK_TIME_STATE,
                    ColliderType = ColliderType.Enemy,
                };

                result.Add(config);
            }

            return result;
        }

        public EnemyConfig InitializeEnemy(uint islandId, EnemyType type,
            Vector2 pos)
        {
            var manager = UtilHelper.GameManager();

            if (manager == null)
            {
                return null;
            }

            if (_prefabs == null)
            {
                return null;
            }

            var selectedConfig = _prefabs.ListEnemyConfig?.FirstOrDefault(x=> x.Type == type);

            if (selectedConfig == null)
            {
                return null;
            }

            return selectedConfig.CloneEnemy(islandId, pos);
        }
    }
}

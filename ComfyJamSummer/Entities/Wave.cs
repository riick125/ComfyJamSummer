using ComfyJamSummer.Components.Gameplay;
using ComfyJamSummer.Entities.Base;
using ComfyJamSummer.Entities.Configs;
using ComfyJamSummer.Enums;
using ComfyJamSummer.Helpers;
using Nez;
using Nez.Sprites;
using Nez.Systems;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace ComfyJamSummer.Entities
{
    public class Wave : BasicEntity
    {
        Island _island;

        bool _initiatedCountdown;

        public bool InitiatedCountdown => _initiatedCountdown;

        float _timeLeftToStart, _startCooldown;

        public float TimeLeftToStart => _timeLeftToStart;

        List<Enemy> _enemies;
        int _aliveEnemyQtyLimit, _enemiesSpawnQty;

        public int EnemiesSpawnQty => _enemiesSpawnQty;

        int _totalEnemiesSpawned;

        int _index;

        public int Index => _index;

        public WaveController WaveController => this.GetComponent<WaveController>();

        CoroutineManager _coroutineManager;

        public Wave CloneWave(WaveConfig config)
        {
            var clone = base.Clone() as Wave;
            clone._coroutineManager = new CoroutineManager();

            clone._index = config.Index;
            clone._island = config.Island;
            clone._startCooldown = config.StartCooldown;
            clone._aliveEnemyQtyLimit = config.AliveEnemyQtyLimit;
            clone._enemiesSpawnQty = config.EnemiesSpawnQty;
            clone._enemies = new List<Enemy>();

            return clone;
        }

        public override void OnAddedToScene()
        {
            base.OnAddedToScene();

            AddComponent(new WaveController(_gameManager, _prefabs));
        }

        public bool StillGoing
        {
            get
            {
                if (_enemies.Count > 0)
                {
                    return _enemies.Any(x => x.IsAlive) || _totalEnemiesSpawned < _enemiesSpawnQty;
                }
                else
                {
                    return _totalEnemiesSpawned < _enemiesSpawnQty;
                }
            }
        }

        public bool Started
        {
            get
            {
                return _timeLeftToStart <= 0 && _initiatedCountdown;
            }
        }

        public List<Enemy> Enemies { get { return _enemies; } private set { } }

        public void Init()
        {
            if (_initiatedCountdown) return;

            _initiatedCountdown = true;
            _timeLeftToStart = _startCooldown;

            if (_gameManager != null && Index > 1)
            {
                _gameManager.UpdateModifiers();
            }
        }

        public void SpawnEnemy()
        {
            if (!ValidateWave())
            {
                return;
            }

            if (_totalEnemiesSpawned >= _enemiesSpawnQty)
            {
                return;
            }

            var aliveEnemies = Enemies.Count(x => x.IsAlive);

            if (aliveEnemies < _aliveEnemyQtyLimit)
            {
                var isWest = Nez.Random.Chance(50);

                var spawnPosition = isWest ? _island.GetRandomWestPosition() : _island.GetRandomEastPosition();

                var enemy = _prefabs.GetEnemy(_island.Id, EnemyType.Birb, spawnPosition);

                if (_coroutineManager == null)
                {
                    Enemies.Add(this.Scene.AddEntity(enemy));
                }
                else
                {
                    _coroutineManager.StartCoroutine(SpawnPoof(enemy, 0.05f));
                }

                _totalEnemiesSpawned++;

                if (_totalEnemiesSpawned >= _enemiesSpawnQty)
                {
                    enemy.WillSpawnChicken = true;
                }
            }
        }

        public void BuffEnemy(Enemy specificEnemy)
        {
            if (!ValidateWave())
                return;

            var buff = GameManager.GetBuffConfig();

            if (buff != null)
            {
                specificEnemy.Buff(buff);
            }
        }

        public override void Update()
        {
            base.Update();

            if (_coroutineManager != null)
            {
                _coroutineManager.Update();
            }

            if (!ValidateWave())
                return;

            var deltaTime = Time.DeltaTime;

            if (_initiatedCountdown)
            {
                if (!Started)
                {
                    _timeLeftToStart -= deltaTime;
                }
            }

        }

        private IEnumerator SpawnPoof(Enemy enemy, float duration)
        {
            var poof = this.Scene.AddEntity(_prefabs.Poof.ClonePoof(enemy.Position));

            yield return Coroutine.WaitForSeconds(duration);

            Enemies.Add(this.Scene.AddEntity(enemy));
        }

        bool ValidateWave()
        {
            if (Enemies == null)
                return false;

            if (!Validate())
                return false;

            if (_island == null)
                return false;

            return true;
        }
    }
}
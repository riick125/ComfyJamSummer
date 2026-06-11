using ComfyJamSummer.Components.Gameplay;
using ComfyJamSummer.Entities.Base;
using ComfyJamSummer.Entities.Configs;
using ComfyJamSummer.Enums;
using Nez;
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

        float _timeLeftToEnd, _duration;

        List<Enemy> _enemies;
        int _aliveEnemyQtyLimit;

        int _index;

        public int Index => _index;

        public WaveController WaveController => this.GetComponent<WaveController>();

        public Wave CloneWave(WaveConfig config)
        {
            var clone = base.Clone() as Wave;

            clone._island = config.Island;
            clone._startCooldown = config.StartCooldown;
            clone._duration = config.Duration;
            clone._aliveEnemyQtyLimit = config.AliveEnemyQtyLimit;
            clone._enemies = new List<Enemy>();

            return clone;
        }

        public override void OnAddedToScene()
        {
            base.OnAddedToScene();

            AddComponent(new WaveController(_gameManager, _prefabs));
        }

        public float Duration => _duration;

        public bool StillGoing
        {
            get
            {
                _timeLeftToEnd -= Time.DeltaTime;
                _timeLeftToEnd = Mathf.Clamp(_timeLeftToEnd, 0, 10);

                return _timeLeftToEnd > 0;
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
            _timeLeftToEnd = _duration;
        }

        public void SpawnEnemy()
        {
            if (!ValidateWave())
            {
                return;
            }

            var aliveEnemies = Enemies.Count(x => x.IsAlive);

            if (aliveEnemies < _aliveEnemyQtyLimit)
            {
                var isWest = Nez.Random.Chance(50);

                var spawnPosition = isWest ? _island.GetRandomWestPosition() : _island.GetRandomEastPosition();

                var enemy = _prefabs.GetEnemy(_island.Id, EnemyType.Birb, spawnPosition);

                Enemies.Add(this.Scene.AddEntity(enemy));
            }
        }

        public void BuffEnemies()
        {
            if (!ValidateWave())
                return;

            var buff = GameManager.GetBuffConfig();

            foreach (var enemy in Enemies)
            {
                enemy.Buff(buff);
            }
        }

        public override void Update()
        {
            base.Update();

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
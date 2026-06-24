using ComfyJamSummer.Entities;
using ComfyJamSummer.Manager;
using ComfyJamSummer.Prefab;
using Nez;

namespace ComfyJamSummer.Components.Gameplay
{
    public class WaveController : BaseComponent
    {
        Wave _wave;

        int _minEnemyQtyPerSpawn, _maxEnemyQtyPerSpawn;

        float _timeLeftToNextSpawn, _spawnCooldown;

        float _timeLeftToNextBuff, _buffCooldown;

        public bool CanSpawn { get { return _timeLeftToNextSpawn <= 0; } }

        public bool CanBuff { get { return _timeLeftToNextBuff <= 0; } }

        public WaveController(GameManager manager, Prefabs prefabs) : base(manager, prefabs)
        {
            _maxEnemyQtyPerSpawn = 8;
            _minEnemyQtyPerSpawn = _maxEnemyQtyPerSpawn / 2;

            _spawnCooldown = 2.75f;
        }

        public override void OnAddedToEntity()
        {
            base.OnAddedToEntity();

            _wave = this.Entity as Wave;

            if (_wave != null)
            {
                _buffCooldown = 4;
                _timeLeftToNextBuff = _buffCooldown;
            }
        }

        public void Process()
        {
            if (!Validate(this.Entity))
            {
                return;
            }

            var deltaTime = Time.DeltaTime;

            if (!_wave.InitiatedCountdown)
            {
                _wave.Init();
            }
            else if (_wave.Started)
            {
                if (_wave.StillGoing)
                {
                    if (CanSpawn)
                    {
                        var count = 0;

                        var qty = Nez.Random.Range(_minEnemyQtyPerSpawn, _maxEnemyQtyPerSpawn);

                        while (count < qty)
                        {
                            _wave.SpawnEnemy();

                            count++;
                        }

                        _timeLeftToNextSpawn = _spawnCooldown * Nez.Random.Range(0.75f, 1f);
                    }
                    else
                    {
                        _timeLeftToNextSpawn -= deltaTime;
                    }
                }
                else
                {
                    Entity.Destroy();

                    SetEnabled(false);
                }
            }
        }
    }
}

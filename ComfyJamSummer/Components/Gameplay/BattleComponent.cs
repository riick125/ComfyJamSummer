using ComfyJamSummer.Entities;
using ComfyJamSummer.Entities.Configs;
using ComfyJamSummer.Manager;
using ComfyJamSummer.Prefab;
using Nez;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ComfyJamSummer.Components.Gameplay
{
    public class BattleComponent : BaseComponent, IUpdatable
    {
        Island _island;

        float _timeLeftToNextWave, _nextWaveStartCooldown;

        int _wavesQty;
        List<Wave> _waves;

        Wave _actualWave;
        public Wave ActualWave { get => _actualWave; set => _actualWave = value; }

        public BattleComponent(Island island, GameManager manager, Prefabs prefabs) : base(manager, prefabs)
        {
            _island = island;
            _nextWaveStartCooldown = 30;

            _waves = new List<Wave>();
            _wavesQty = island.WavesQuantity;

            var qtyGrowPercent = WaveDefaultValues.ALIVE_ENEMIES_GROW_PERCENT;
            var qtyModifierValue = WaveDefaultValues.ALIVE_ENEMIES_MODIFIER_VALUE;
            var waveStartCooldown = WaveDefaultValues.START_COOLDOWN;

            for (int i = 0; i < _wavesQty; i++)
            {
                var qtyEnemyLimit = WaveDefaultValues.ALIVE_ENEMIES_QTY_LIMIT;
                var totalEnemiesSpawnQty = WaveDefaultValues.TOTAL_ENEMIES_SPAWN_QTY;

                qtyEnemyLimit = Convert.ToInt32(qtyEnemyLimit * qtyGrowPercent);
                totalEnemiesSpawnQty = Convert.ToInt32(totalEnemiesSpawnQty * qtyGrowPercent);

                var config = prefabs.WaveConfig.CloneWave(_island, i + 1, qtyEnemyLimit, totalEnemiesSpawnQty, waveStartCooldown);

                if (config != null)
                {
                    var wave = Core.Scene.AddEntity(prefabs.GetWave(config));

                    if (wave != null)
                    {
                        _waves.Add(wave);

                        qtyGrowPercent += qtyModifierValue;
                    }
                }
            }
        }

        public void Update()
        {
            if (!ValidateBattle())
                return;

            if (_actualWave == null)
            {
                _waves = _waves.OrderBy(x => x.Index).ToList();

                _actualWave = _waves.FirstOrDefault(x => !x.Started);
            }
            else
            {
                var waveController = _actualWave.WaveController;

                if (waveController != null)
                {
                    waveController.Process();
                }
            }
        }

        bool ValidateBattle()
        {
            if (!Validate(this.Entity))
                return false;

            if (_waves == null)
                return false;

            if (!_waves.Any())
                return false;

            return true;
        }
    }
}
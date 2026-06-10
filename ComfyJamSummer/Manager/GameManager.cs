using ComfyJamSummer.Configs;
using Nez;

namespace ComfyJamSummer.Manager
{
    public class GameManager : SceneComponent
    {
        public bool IsGamePaused { get; set; }

        public bool IsGameOver { get; set; }

        public bool IsAnyCutSceneRunning { get; set; }

        public bool CantDoAnyAction { get { return IsGameOver || IsAnyCutSceneRunning || IsGamePaused; } }

        public float TimeLeftToEndGameStartDelay { get; set; }

        BuffConfig _buffConfig;

        public BuffConfig ActualBuffConfig => _buffConfig;

        public GameManager()
        {
            _buffConfig = new BuffConfig();
        }

        public BuffConfig GetBuffConfig()
        {
            _buffConfig.HpModifier += BuffEnemyModifierValues.HP;
            _buffConfig.SpeedModifier += BuffEnemyModifierValues.SPEED;
            _buffConfig.DamageModifier += BuffEnemyModifierValues.DMG;
            _buffConfig.AtkSpeedModifier += BuffEnemyModifierValues.ATK_SPEED;

            return _buffConfig;
        }
    }
}
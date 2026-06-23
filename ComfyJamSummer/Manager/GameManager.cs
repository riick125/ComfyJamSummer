using ComfyJamSummer.Components.Cutscenes.Base;
using ComfyJamSummer.Configs;
using Nez;

namespace ComfyJamSummer.Manager
{
    public class GameManager : SceneComponent
    {
        public bool IsGamePaused { get; set; }

        public bool IsGameOver { get; set; }

        public bool IsAnyCutSceneRunning
        {
            get
            {
                if (this.Scene == null)
                {
                    return false;
                }

                return this.Scene.GetSceneComponent<CutsceneComponent>() != null;
            }
        }

        public bool CantDoAnyAction { get { return TimeLeftToEndGameStartDelay > 0 || IsGameOver || IsAnyCutSceneRunning || IsGamePaused; } }

        public float TimeLeftToEndGameStartDelay { get; set; }

        BuffConfig _buffConfig;

        public BuffConfig ActualBuffConfig => _buffConfig;

        public GameManager()
        {
            _buffConfig = new BuffConfig();
        }

        public BuffConfig GetBuffConfig()
        {
            return _buffConfig;
        }

        public void UpdateModifiers()
        {
            _buffConfig.HpModifier += BuffEnemyModifierValues.HP;
            _buffConfig.SpeedModifier += BuffEnemyModifierValues.SPEED;
            _buffConfig.DamageModifier += BuffEnemyModifierValues.DMG;
            _buffConfig.AtkSpeedModifier -= BuffEnemyModifierValues.ATK_SPEED;
        }
    }
}
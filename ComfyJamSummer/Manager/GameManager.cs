using ComfyJamSummer.Components.Cutscenes.Base;
using ComfyJamSummer.Configs;
using ComfyJamSummer.Extensions;
using ComfyJamSummer.Helpers;
using ComfyJamSummer.Prefab;
using Microsoft.Xna.Framework.Input;
using Nez;

namespace ComfyJamSummer.Manager
{
    public class GameManager : SceneComponent
    {
        public bool IsGameStarted { get; set; }

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

        public bool CantDoAnyAction { get { return TimeLeftToEndGameStartDelay > 0 || IsGameOver || IsAnyCutSceneRunning || IsGamePaused || !IsGameStarted; } }

        public float TimeLeftToEndGameStartDelay { get; set; }

        BuffConfig _buffConfig;

        public BuffConfig ActualBuffConfig => _buffConfig;

        Prefabs _prefabs;

        public GameManager()
        {
            _buffConfig = new BuffConfig();
        }

        public override void OnEnabled()
        {
            base.OnEnabled();

            if (_prefabs == null)
            {
                _prefabs = UtilHelper.Prefabs();
            }
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
            _buffConfig.BulletSpeedModifier += BuffEnemyModifierValues.BULLET_SPEED;
        }

        public override void Update()
        {
            base.Update();

            if (Input.IsKeyPressed(Keys.Escape))
            {
                SoundHelper.PlayRandomSound(Enums.SoundFxName.Collect_1);

                IsGamePaused = !IsGamePaused;

                Time.TimeScale = IsGamePaused ? 0 : 1;

                var pauseCanvas = UIHelper.GetCanvas(UINames.PAUSE);

                pauseCanvas?.SetEnabled(IsGamePaused);
            }

            IsGameStarted = UIHelper.GetCanvas(UINames.MENU) == null;

            if (Input.CurrentKeyboardState.GetPressedKeyCount() > 0 && !IsGameStarted)
            {
                SoundHelper.PlayRandomSound(Enums.SoundFxName.Collect_1);

                Time.TimeScale = 1;

                var menuCanvas = UIHelper.GetCanvas(UINames.MENU);

                menuCanvas?.Entity?.Destroy();
            }
        }
    }
}
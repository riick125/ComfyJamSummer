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
    }
}
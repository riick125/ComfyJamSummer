using Steamworks;

namespace ComfyJamSummer.Components.Extensions
{
    public class SteamManager
    {
        private bool m_bInitialized;

        public bool Initialized
        {
            get
            {
                return m_bInitialized;
            }
        }

        private bool _readyToStartSteamScript;

        public bool ReadyToStartSteamScript { get { return _readyToStartSteamScript; } }

        public SteamManager()
        {
            Initialize();
        }

        private void Initialize()
        {
            m_bInitialized = SteamAPI.Init();
        }

        public void Update()
        {
            if (!m_bInitialized)
            {
                return;
            }
            else
            {
                if (!_readyToStartSteamScript)
                {
                    if (Game1.SteamScript == null)
                    {
                        Game1.SteamScript = new SteamScript();
                    }

                    _readyToStartSteamScript = true;
                }
            }

            SteamAPI.RunCallbacks();
        }
    }
}
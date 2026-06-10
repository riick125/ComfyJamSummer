using ComfyJamSummer.Helpers;
using ComfyJamSummer.Manager;
using ComfyJamSummer.Prefab;
using Nez;

namespace ComfyJamSummer.Entities.Base
{
    public class BasicEntity : Entity
    {
        protected Prefabs _prefabs;

        public Prefabs Prefabs => _prefabs;


        protected GameManager _gameManager;

        public GameManager GameManager => _gameManager;

        public override void OnAddedToScene()
        {
            base.OnAddedToScene();

            _prefabs = UtilHelper.Prefabs();
            _gameManager = UtilHelper.GameManager();
        }

        protected virtual bool Validate()
        {
            if (this.Scene == null)
                return false;

            if (GameManager == null || GameManager.CantDoAnyAction)
                return false;

            if (Prefabs == null)
                return false;

            return true;
        }
    }
}
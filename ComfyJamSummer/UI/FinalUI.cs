using ComfyJamSummer.Extensions;
using ComfyJamSummer.Helpers;
using ComfyJamSummer.Manager;
using ComfyJamSummer.Prefab;
using ComfyJamSummer.Scenes;
using ComfyJamSummer.UI.Base;
using Microsoft.Xna.Framework.Input;
using Nez;
using Nez.UI;

namespace ComfyJamSummer.UI
{
    public class FinalUI : BaseUI
    {
        string _playAgain = "Press [R] to play again";

        Label _lblPlayAgain;

        CustomFont _font;

        bool _restarted;

        public FinalUI(GameManager manager, Prefabs prefabs) : base(manager, prefabs)
        {
        }

        public override void OnAddedToEntity()
        {
            base.OnAddedToEntity();

            _font = UtilHelper.CustomFont();

            if (_font != null)
            {
                _lblPlayAgain = _container.AddElement(new Label(_playAgain, _font.FontGiant));
                UIHelper.CentralizeElementPosXInScreen(_lblPlayAgain, Screen.Height * 0.9f);
            }
        }

        public override void Update()
        {
            base.Update();

            if (!_restarted)
            {
                if (Input.IsKeyPressed(Keys.R))
                {
                    Core.StartSceneTransition<FadeTransition>(new FadeTransition(() => new InGameScene())); 
                    _restarted = true;
                }      
            }
        }
    }
}
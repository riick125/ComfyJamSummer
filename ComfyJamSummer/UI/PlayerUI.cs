using ComfyJamSummer.Entities;
using ComfyJamSummer.Helpers;
using ComfyJamSummer.Manager;
using ComfyJamSummer.Prefab;
using ComfyJamSummer.UI.Base;
using Nez.UI;
using Microsoft.Xna.Framework;
using ComfyJamSummer.Extensions;
using Nez;

namespace ComfyJamSummer.UI
{
    public class PlayerUI : BaseUI
    {
        string _txtEatSandwich = "Press [SPACE] to eat sandwich";
        Label _lblEatSandwich;

        Player _player;

        public PlayerUI(GameManager manager, Prefabs prefabs) : base(manager, prefabs)
        {
            var font = UtilHelper.CustomFont();

            if (font != null)
            {
                _lblEatSandwich = new Label(_txtEatSandwich, new LabelStyle(font.FontBig, Color.White));
                _lblEatSandwich.SetVisible(false);
                UIHelper.CentralizeElementPosXInScreen(_lblEatSandwich, Screen.Height * 0.9f);

                _container.AddElement(_lblEatSandwich);
            }
        }

        public override void OnAddedToEntity()
        {
            base.OnAddedToEntity();

            _player = UtilHelper.Player();
        }

        public override void Update()
        {
            base.Update();

            if (!ValidatePlayer())
            {
                return;
            }

            _lblEatSandwich.SetVisible(_player.Sandwich != null);
        }

        bool ValidatePlayer()
        {
            if (!Validate())
            {
                return false;
            }

            if (_player == null)
            {
                return false;
            }

            if (_lblEatSandwich == null)
            {
                return false;
            }

            return true;
        }
    }
}
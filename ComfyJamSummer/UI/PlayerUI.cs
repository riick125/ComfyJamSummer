using ComfyJamSummer.Entities;
using ComfyJamSummer.Enums;
using ComfyJamSummer.EventDatas;
using ComfyJamSummer.Extensions;
using ComfyJamSummer.Helpers;
using ComfyJamSummer.Manager;
using ComfyJamSummer.Prefab;
using ComfyJamSummer.UI.Base;
using ComfyJamSummer.UI.CustomImages;
using ComfyJamSummer.UI.Enums;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Nez;
using Nez.Systems;
using Nez.UI;
using System.Collections.Generic;
using System.Linq;
using static ComfyJamSummer.UI.CustomImages.HealthBarImage;

namespace ComfyJamSummer.UI
{
    public class PlayerUI : BaseUI
    {
        string _txtRestart = "Press [R] to restart";
        string _txtEatSandwich = "Press [SPACE] to eat sandwich (don't do it)";
        Label _lblEatSandwich, _lblRestart;

        Player _player;

        Texture2D _healthBarTexture;

        Image _imgHeart;
        List<HealthBarImage> _healthBars;

        Label _lblHealth;

        Label _lblAmmo;

        public static Emitter<UIEvent, UIEventData> Emitter;

        CustomFont _customFont;

        public PlayerUI(GameManager manager, Prefabs prefabs) : base(manager, prefabs)
        {
            Emitter = new Emitter<UIEvent, UIEventData>();

            Emitter.AddObserver(UIEvent.ReduceBar, OnHit);
            Emitter.AddObserver(UIEvent.HealBar, OnHeal);

            _customFont = UtilHelper.CustomFont();

            if (_customFont != null)
            {
                _lblEatSandwich = _container.AddElement(new Label(_txtEatSandwich, new LabelStyle(_customFont.FontBig, Color.White)));
                _lblEatSandwich.SetVisible(false);
                UIHelper.CentralizeElementPosXInScreen(_lblEatSandwich, Screen.Height * 0.9f);

                _lblRestart = _container.AddElement(new Label(_txtRestart, new LabelStyle(_customFont.FontBig, Color.White)));
                _lblRestart.SetVisible(false);
                UIHelper.CentralizeElementPosXInScreen(_lblRestart, Screen.Height * 0.9f);
            }
        }

        public override void OnAddedToEntity()
        {
            base.OnAddedToEntity();

            if (_customFont == null)
            {
                this.RemoveComponent();

                return;
            }

            _player = UtilHelper.Player();

            CreateHealth();

            CreateAmmo();
        }

        public override void Update()
        {
            base.Update();

            if (!ValidatePlayer())
            {
                return;
            }

            _lblRestart.SetVisible(!_player.IsAlive);

            _lblEatSandwich.SetVisible(_player.Sandwich != null && _player.IsAlive);

            for (int i = 0; i < _healthBars.Count; i++)
            {
                var img = _healthBars[i];

                var isInvisible = img.GetX() == 0 || img.GetY() == 0;

                img.SetVisible(!isInvisible);

                switch (img.Name)
                {
                    case HealthBarTypeEnum.DamageTaken:
                        img.Process();
                        break;
                }
            }

            _lblAmmo.SetText($"{_player.Gun.ActualAmmo}/{_player.Gun.MagSize}");

            var spacing = Screen.Width * 0.985f;

            _lblAmmo.SetPosition(spacing - _lblAmmo.Width(), _imgHeart.GetY() + _lblAmmo.Height() / 2);
        }

        void CreateAmmo()
        {
            if (_player?.Gun == null)
            {
                return;
            }

            _lblAmmo = _container.AddElement(new Label($"{_player.Gun.ActualAmmo}/{_player.Gun.MagSize}", new LabelStyle(_customFont.FontBig, Constants.WHITE_COLOR)));

            var spacing = Screen.Width * 0.985f;

            _lblAmmo.SetPosition(spacing - _lblAmmo.Width(), _imgHeart.GetY() + _lblAmmo.Height() / 2);
        }

        void CreateHealth()
        {
            _healthBars = new List<HealthBarImage>();

            if (!ValidatePlayer())
            {
                return;
            }

            try
            {
                _lblHealth = _container.AddElement(new Label($"{(int)_player.ActualHP}/{(int)_player.MaxHP}", _customFont.FontNormal));

                var textureHeart = _prefabs.GetUITexture(UISprite.heart);

                _imgHeart = _container.AddElement(new Image(textureHeart));
                UIHelper.RescaleUIElementSize(_imgHeart);

                _imgHeart.SetOrigin(_imgHeart.Width() / 2, _imgHeart.Height() / 2);

                _imgHeart.SetPosition(Screen.Width * 0.012f, Screen.Height * 0.045f);

                var heartPos = new Vector2(_imgHeart.GetX() + _imgHeart.Width(), _imgHeart.GetY());

                var allBars = new List<UISprite>() { UISprite.health_bg, UISprite.health_bar, UISprite.health_border };

                foreach (var item in allBars)
                {
                    var texture = _prefabs.GetUITexture(item);

                    LifeBarType barType = LifeBarType.Bar;

                    switch (item)
                    {
                        case UISprite.health_bar:
                            barType = LifeBarType.Bar;

                            _healthBarTexture = _prefabs.GetUITexture(item);
                            break;

                        case UISprite.health_bg:
                            barType = LifeBarType.Background;
                            break;

                        case UISprite.health_border:
                            barType = LifeBarType.Border;
                            break;
                    }

                    var img = new HealthBarImage(texture, _player, barType, Enums.HealthBarTypeEnum.Normal);
                    UIHelper.RescaleUIElementSize(img);

                    img.SetOrigin(img.Width() / 2, img.Height() / 2);

                    img.SetPosition(heartPos.X + (img.Width() * 0.025f), heartPos.Y);

                    _healthBars.Add(img);
                    _container.AddElement(img);
                }
            }
            catch (System.Exception)
            {
                this.RemoveComponent();
            }

            UpdateLabelHealth();
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

            if (_player.Gun == null)
            {
                return false;
            }

            if (_lblEatSandwich == null)
            {
                return false;
            }

            if (_healthBars == null)
            {
                return false;
            }

            return true;
        }

        void OnHeal(UIEventData data)
        {
            var mob = data.Target;

            if (mob == null)
            {
                return;
            }

            var bar = _healthBars.FirstOrDefault(x => x.BarType == LifeBarType.Bar);

            if (bar != null)
            {
                bar.Process();
                UpdateLabelHealth();
            }
        }

        void UpdateLabelHealth()
        {
            var anyBar = _healthBars.FirstOrDefault(x=> x.BarType == LifeBarType.Border);

            if (anyBar != null)
            {
                _lblHealth.SetText($"{(int)_player.ActualHP}/{(int)_player.MaxHP}");
                _lblHealth.SetPosition(anyBar.GetX() + anyBar.Width() / 2 - _lblHealth.Width() / 2, anyBar.GetY() - _lblHealth.Height() / 2);
            }
        }

        void OnHit(UIEventData data)
        {
            var mob = data.Target;

            if (mob == null)
            {
                return;
            }

            for (int i = _healthBars.Count - 1; i >= 0; i--)
            {
                var removed = UIHelper.RemoveDamageTakenElementFromContainer(_container, mob.Id, _healthBars);

                if (removed)
                {
                    i--;
                }
            }

            var bar = _healthBars.FirstOrDefault(x => x.BarType == LifeBarType.Bar);

            if (bar != null)
            {
                bar.Process();

                UpdateLabelHealth();
            }

            var clonedHpBarTexture = UtilHelper.CloneTexture(_healthBarTexture);

            var img = new HealthBarImage(clonedHpBarTexture, mob, HealthBarImage.LifeBarType.Bar, HealthBarTypeEnum.DamageTaken);
            UIHelper.RescaleUIElementSize(img);
            img.InitializeDamageTaken(bar);

            img.SetColor(Constants.RED_COLOR);

            img.ValueTaken = mob.LastReduceValueTaken;
            img.ActualValueTaken = img.ValueTaken;
            img.SetVisible(false);

            _container.AddElement(img);

            if (bar != null)
            {
                img.SetPosition(bar.GetX() + img.Offset.X, bar.GetY());
            }

            _healthBars.Add(img);
        }
    }
}
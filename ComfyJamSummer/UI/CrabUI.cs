using ComfyJamSummer.Entities;
using ComfyJamSummer.Enums;
using ComfyJamSummer.EventDatas;
using ComfyJamSummer.Extensions;
using ComfyJamSummer.Helpers;
using ComfyJamSummer.Manager;
using ComfyJamSummer.Prefab;
using ComfyJamSummer.UI.Base;
using ComfyJamSummer.UI.CustomImages;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Nez;
using Nez.Systems;
using Nez.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using static ComfyJamSummer.UI.CustomImages.HealthBarImage;

namespace ComfyJamSummer.UI
{
    public class CrabUI : BaseUI
    {
        Image _imgPatience, _imgSatiation, _imgBuild;

        List<HealthBarImage> _patienceBars, _satiationBars, _buildBars;
        Texture2D _barTexture;

        Crab _crab;

        Rocket _rocket;

        public static Emitter<UIEvent, UIEventData> Emitter;

        public CrabUI(GameManager manager, Prefabs prefabs) : base(manager, prefabs)
        {
            Emitter = new Emitter<UIEvent, UIEventData>();

            Emitter.AddObserver(UIEvent.UpdatePatienceBar, UpdatePatienceBar);
            Emitter.AddObserver(UIEvent.UpdateSatiationBar, UpdateSatiationBar);
            Emitter.AddObserver(UIEvent.UpdateBuildBar, UpdateBuildBar);

            _patienceBars = new List<HealthBarImage>();

            _satiationBars = new List<HealthBarImage>();

            _buildBars = new List<HealthBarImage>();
        }

        public override void OnAddedToEntity()
        {
            base.OnAddedToEntity();

            _crab = UtilHelper.Crab();

            _rocket = UtilHelper.GetEntity<Rocket>();

            if (!ValidateCrab(true))
            {
                this.RemoveComponent();

                return;
            }

            CreatePatience();

            CreateSatiation();

            CreateBuild();
        }

        public override void Update()
        {
            base.Update();

            var isValid = ValidateCrab();

            _container.SetVisible(isValid);

            foreach (var item in _buildBars)
            {
                item.SetVisible(_rocket.BuildPhases.Any(x => !x.IsDone));
            }

            if (!isValid)
            {
                return;
            }

            if (_imgPatience != null)
            {
                var scale = UtilHelper.GetGameScale();

                var crabPos = _camera.WorldToScreenPoint(_crab.Position);

                var rocketPos = _camera.WorldToScreenPoint(_rocket.Position);

                SetPositionBars(scale, crabPos, _patienceBars, _imgPatience, _crab.SpriteHeight / 1.5f, false);

                SetPositionBars(scale, crabPos, _satiationBars, _imgSatiation, _crab.SpriteHeight / 1.5f, true);

                SetPositionBars(scale, rocketPos, _buildBars, _imgBuild, _rocket.SpriteHeight / 1.5f, false);
            }
        }

        void UpdateSatiationBar(UIEventData data)
        {
            if (!ValidateEvent(data, typeof(Crab))) return;

            var mob = data.Target as Crab;

            var bar = _satiationBars?.FirstOrDefault(x => x.BarType == LifeBarType.Bar);

            var color = mob.IsHungry ? Constants.RED_COLOR : Constants.GREEN_COLOR;

            ProcessUpdateBar(mob.ActualSatiation, mob.MaxSatiation, _satiationBars, color);
        }

        void UpdatePatienceBar(UIEventData data)
        {
            if (!ValidateEvent(data, typeof(Crab))) return;

            var mob = data.Target as Crab;

            var color = mob.ActualPatience < (mob.MaxPatience * 0.35f) ? Constants.RED_COLOR : Constants.GREEN_COLOR;

            ProcessUpdateBar(mob.ActualPatience, mob.MaxPatience, _patienceBars, color);
        }

        void UpdateBuildBar(UIEventData data)
        {
            if (!ValidateEvent(data, typeof(Rocket))) return;

            var mob = data.Target as Rocket;

            var color = mob.ActualProgress < (mob.MaxProgress * 0.75f) ? Constants.RED_COLOR : Constants.GREEN_COLOR;

            ProcessUpdateBar(mob.ActualProgress, mob.MaxProgress, _buildBars, color);
        }

        void ProcessUpdateBar(float actualValue, float maxValue, List<HealthBarImage> list, Color colorBar)
        {
            var bar = list?.FirstOrDefault(x => x.BarType == LifeBarType.Bar);

            if (bar != null)
            {
                bar.Process(actualValue, maxValue);

                bar.SetColor(colorBar);
            }
        }

        bool ValidateEvent(UIEventData data, Type type)
        {
            if (data.Target == null)
            {
                return false;
            }

            if (data.Target.GetType() != type)
            {
                return false;
            }

            return true;
        }

        private void SetPositionBars(float scale, Vector2 crabPos, List<HealthBarImage> listBars, Image imgIcon, float height, bool invertOffsetY)
        {
            var anyBar = listBars.MaxBy(x => x.Width());

            var spacing = anyBar.Width() * 0.025f;

            var totalWidth = imgIcon.Width() + anyBar.Width() + spacing;

            var offsetY = anyBar.Height() + (height * scale);

            var posIcon = crabPos - new Vector2(totalWidth / 2, 0);

            posIcon.Y -= offsetY * (invertOffsetY ? -1 : 1);

            imgIcon.SetPosition(posIcon.X, posIcon.Y);

            var posIconPatience = new Vector2(imgIcon.GetX() + imgIcon.Width(), imgIcon.GetY());

            foreach (var img in listBars)
            {
                img.SetPosition(posIconPatience.X + spacing, posIconPatience.Y + imgIcon.Height() / 2 - img.Height() / 2);
            }
        }

        void CreatePatience()
        {
            _imgPatience = CreateBar(UISprite.patience, _imgPatience, _patienceBars);
        }

        void CreateSatiation()
        {
            _imgSatiation = CreateBar(UISprite.satiation, _imgSatiation, _satiationBars);
        }

        void CreateBuild()
        {
            _imgBuild = CreateBar(UISprite.build, _imgBuild, _buildBars);
        }

        Image CreateBar(UISprite iconSpriteName, Image imgIcon, List<HealthBarImage> listBars)
        {

            try
            {
                var texturePatience = _prefabs.GetUITexture(iconSpriteName);

                imgIcon = _container.AddElement(new Image(texturePatience));
                UIHelper.RescaleUIElementSize(imgIcon);

                var alpha = 0.75f;

                imgIcon.SetColor(Color.White * alpha);

                var allBars = new List<UISprite>() { UISprite.mini_bar_bg, UISprite.mini_bar_bar, UISprite.mini_bar_border };

                foreach (var item in allBars)
                {
                    var textureBar = _prefabs.GetUITexture(item);

                    LifeBarType barType = LifeBarType.Bar;

                    switch (item)
                    {
                        case UISprite.mini_bar_bar:
                            barType = LifeBarType.Bar;

                            _barTexture = _prefabs.GetUITexture(item);
                            break;

                        case UISprite.mini_bar_bg:
                            barType = LifeBarType.Background;
                            break;

                        case UISprite.mini_bar_border:
                            barType = LifeBarType.Border;
                            break;
                    }

                    var img = new HealthBarImage(textureBar, _crab, barType, Enums.HealthBarTypeEnum.Normal, barType == LifeBarType.Bar ? Constants.GREEN_COLOR : default);
                    UIHelper.RescaleUIElementSize(img);

                    img.SetOrigin(img.Width() / 2, img.Height() / 2);

                    img.SetColor(img.GetColor() * alpha);

                    listBars.Add(img);
                    _container.AddElement(img);
                }
            }
            catch (System.Exception)
            {
                this.RemoveComponent();
            }

            return imgIcon;
        }

        bool ValidateCrab(bool ignoreManager = false)
        {
            if (!Validate(ignoreManager))
            {
                return false;
            }

            if (_crab == null)
            {
                return false;
            }

            if (_rocket == null)
            {
                return false;
            }

            return true;
        }
    }
}
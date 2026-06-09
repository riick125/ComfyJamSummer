using ComfyJamSummer.Data;
using ComfyJamSummer.Enums;
using ComfyJamSummer.Manager;
using ComfyJamSummer.Prefab;
using ComfyJamSummer.Scenes;
using ComfyJamSummer.UI.Animations;
using ComfyJamSummer.UI.Base;
using ComfyJamSummer.UI.CustomImages;
using Microsoft.Xna.Framework;
using Nez;
using Nez.UI;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ComfyJamSummer.UI
{
    public class CompanyLogoUI : BaseUI
    {
        private ShakableImage _kakapoHead, _companyName;
        private readonly Vector2 _kakapoHeadDestination, _companyNameDestination;
        private readonly Vector2 _kakapoHeadInitialPosition, _companyNameInitialPosition;

        private float _logoSpeed = 6000;
        private float _scaleIncreaserAmount = 0.085f;

        private readonly float _maxScale = 3;

        private List<UISimpleAnimationPart> _animationParts;
        private int _indexPart;
        private bool _leavingScene;

        private float _spacing;

        private float _alpha = 1f;
        private float _alphaDecreaserAmount = 10f;

        public CompanyLogoUI(GameManager manager, Prefabs prefabs) : base(manager, prefabs)
        {
            _spacing = Screen.Height * 0.01f;

            _container = new Container() { FillParent = true };

            var logoData = _prefabs.LogoData;

            if (logoData == null)
            {
                return;
            }

            var kakapoHeadTexture = logoData.CreateKakapoHeadLogo();

            var companyNameTexture = logoData.CreateKakapoTextLogo();

            _kakapoHead = _container.AddElement(new ShakableImage(_prefabs, kakapoHeadTexture));

            _companyName = _container.AddElement(new ShakableImage(_prefabs, companyNameTexture));

            _kakapoHeadDestination = new Vector2((_container.GetX() + Screen.Width / 2) - _kakapoHead.PreferredWidth / 2, (Screen.Height * 0.35f) - _kakapoHead.PreferredHeight / 2);

            _kakapoHead.SetPosition(_container.GetX() - _kakapoHead.PreferredWidth, _kakapoHeadDestination.Y);
            _kakapoHeadInitialPosition = new Vector2(_kakapoHead.GetX(), _kakapoHead.GetY());

            _companyNameDestination = new Vector2((_container.GetX() + Screen.Width / 2) - _companyName.PreferredWidth / 2, _kakapoHeadDestination.Y + (_kakapoHead.PreferredHeight + _spacing));

            _companyName.SetPosition(_container.GetX() + Screen.Width + _companyName.PreferredWidth, _companyNameDestination.Y);
            _companyNameInitialPosition = new Vector2(_companyName.GetX(), _companyName.GetY());

            Stage.AddElement(_container);

            _kakapoHead.SetOriginalPosition(_kakapoHeadDestination);
            _companyName.SetOriginalPosition(_companyNameDestination);

            _animationParts = new List<UISimpleAnimationPart>();

            _animationParts.Add(new UISimpleAnimationPart(LogoStatesEnum.Starting, 0.5f));
            _animationParts.Add(new UISimpleAnimationPart(LogoStatesEnum.KakapoHeadAppearing, 1f, 0, _kakapoHeadDestination));
            _animationParts.Add(new UISimpleAnimationPart(LogoStatesEnum.KakapoCompanyNameAppearing, 1f, 0.2f, _companyNameDestination));
            _animationParts.Add(new UISimpleAnimationPart(LogoStatesEnum.LogoGoingToTheScreen, 3f));
        }

        public override void Update()
        {
            base.Update();

            _kakapoHead.Update();
            _companyName.Update();

            var deltaTime = Time.DeltaTime;

            if (_animationParts == null)
            {
                return;
            }

            if (!_animationParts.Any())
            {
                return;
            }

            if (_animationParts.Any(x => !x.Finished))
            {
                var actualPart = _animationParts[_indexPart];

                if (actualPart != null)
                {
                    if (actualPart.StartDelay <= 0)
                    {
                        if (!actualPart.Finished)
                        {
                            var direction = Vector2.Zero;
                            var posX = 0f;

                            switch (actualPart.Name)
                            {
                                case LogoStatesEnum.KakapoHeadAppearing:
                                    direction = _kakapoHeadDestination - new Vector2(_kakapoHead.GetX(), _kakapoHead.GetY());
                                    direction.Normalize();

                                    posX = _kakapoHead.GetX();

                                    posX += direction.X * _logoSpeed * deltaTime;

                                    posX = Mathf.Clamp(posX, _kakapoHeadInitialPosition.X, _kakapoHeadDestination.X);

                                    _kakapoHead.SetPosition(posX, _kakapoHead.GetY());

                                    if (posX >= _kakapoHeadDestination.X)
                                    {
                                        if (_prefabs != null)
                                        {
                                            _kakapoHead.Shake(0.35f);

                                            _prefabs.PlaySound(SoundFxName.Piu_1);
                                            _prefabs.PlaySound(SoundFxName.Bash);
                                        }

                                        actualPart.TimeLeftToEnd = 0;
                                    }
                                    break;

                                case LogoStatesEnum.KakapoCompanyNameAppearing:
                                    direction = _companyNameDestination - new Vector2(_companyName.GetX(), _companyName.GetY());
                                    direction.Normalize();

                                    posX = _companyName.GetX();

                                    posX += direction.X * _logoSpeed * deltaTime;

                                    posX = Mathf.Clamp(posX, _companyNameDestination.X, _companyNameInitialPosition.X);

                                    _companyName.SetPosition(posX, _companyName.GetY());

                                    if (posX <= _companyNameDestination.X)
                                    {
                                        if (_prefabs != null)
                                        {
                                            _companyName.Shake(0.35f);

                                            _prefabs.PlaySound(SoundFxName.Piu_2);
                                            _prefabs.PlaySound(SoundFxName.Bash);
                                        }

                                        actualPart.TimeLeftToEnd = 0;
                                    }
                                    break;

                                case LogoStatesEnum.LogoGoingToTheScreen:
                                    if (!_kakapoHead.IsShaking && !_companyName.IsShaking)
                                    {
                                        var scale = _kakapoHead.GetScaleX();

                                        scale += _scaleIncreaserAmount * deltaTime;

                                        scale = Mathf.Clamp(scale, 1f, _maxScale);

                                        _kakapoHead.SetSize(_kakapoHead.GetWidth() * scale, _kakapoHead.GetHeight() * scale);
                                        _companyName.SetSize(_companyName.GetWidth() * scale, _companyName.GetHeight() * scale);

                                        _kakapoHead.SetPosition((_container.GetX() + Screen.Width / 2) - _kakapoHead.GetWidth() / 2, (Screen.Height * 0.35f) - _kakapoHead.GetHeight() / 2);
                                        _companyName.SetPosition((_container.GetX() + Screen.Width / 2) - _companyName.GetWidth() / 2, _kakapoHead.GetY() + (_kakapoHead.GetHeight() + _spacing));
                                    }
                                    break;
                            }

                            actualPart.TimeLeftToEnd -= deltaTime;
                        }
                        else
                        {
                            if (_indexPart < (_animationParts.Count - 1))
                            {
                                _indexPart++;
                            }
                        }
                    }
                    else
                    {
                        actualPart.StartDelay -= deltaTime;
                    }
                }
            }
            else
            {
                _alpha -= _alphaDecreaserAmount * deltaTime;

                _kakapoHead.SetColor(Color * _alpha);
                _companyName.SetColor(Color.White * _alpha);

                if (!_leavingScene)
                {
                    Core.StartSceneTransition(new FadeTransition(() => new InGameScene()) { FadeToColor = new Color(251, 255, 246), FadeInDuration = Constants.SLOW_SCENE_FADE_IN });

                    _leavingScene = true;
                }
            }
        }

        enum LogoStatesEnum
        {
            Starting,
            KakapoHeadAppearing,
            KakapoCompanyNameAppearing,
            LogoGoingToTheScreen
        }
    }
}

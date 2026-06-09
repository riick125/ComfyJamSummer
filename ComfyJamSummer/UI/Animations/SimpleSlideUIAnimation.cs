using System.Linq;
using ComfyJamSummer.Components.Gameplay;
using ComfyJamSummer.Helpers;
using ComfyJamSummer.UI.CustomButtons;
using ComfyJamSummer.UI.CustomElements;
using ComfyJamSummer.UI.CustomImages;
using ComfyJamSummer.UI.Enums;
using Nez;
using Nez.UI;
using Screen = Nez.Screen;

namespace ComfyJamSummer.UI.Animations
{
    public class SimpleSlideUIAnimation
    {
        public bool Finished { get; private set; }

        public SimpleSlideUIAnimationsEnum Type { get; set; }

        private Element _element;

        private float _actualVelocity;

        private float _originalVelocity, _velocityLostPercentage, _minVelocity;

        private float _destinationX, _destinationY;

        public SimpleSlideUIAnimation(Element element, SimpleSlideUIAnimationsEnum type, float destinationX, float destinationY)
        {
            // _velocityLostPercentage = Nez.Random.Range(0.0045f, 0.0085f);
            _velocityLostPercentage = Nez.Random.Range(0.8f, 0.82f);

            _element = element;

            Type = type;
            _originalVelocity = Screen.Width * 4;
            _actualVelocity = _originalVelocity;
            _minVelocity = _originalVelocity * 0.6f;

            _destinationX = destinationX;
            _destinationY = destinationY;

            switch (type)
            {
                case SimpleSlideUIAnimationsEnum.SlideToTop:
                    _element.SetPosition(_destinationX, Screen.Height);
                    break;

                case SimpleSlideUIAnimationsEnum.SlideToBottom:
                    _element.SetPosition(_destinationX, -Screen.Height);
                    break;

                case SimpleSlideUIAnimationsEnum.SlideToLeft:
                    _element.SetPosition(Screen.Width, _destinationY);
                    break;

                case SimpleSlideUIAnimationsEnum.SlideToRight:
                    _element.SetPosition(-Screen.Width, _destinationY);
                    break;
            }
        }

        public void Update()
        {
            if (Finished)
            {
                return;
            }

            var posX = _element.GetX();
            var posY = _element.GetY();

            var deltaTime = Time.AltDeltaTime;

            switch (Type)
            {
                case SimpleSlideUIAnimationsEnum.SlideToTop:
                    if (_element.GetY() > _destinationY)
                    {
                        posY -= _actualVelocity * deltaTime;
                    }
                    else
                    {
                        Finished = true;
                    }

                    if (_element.GetY() < (_destinationY / 2))
                    {
                        _actualVelocity *= _velocityLostPercentage;
                    }

                    UpdatePosition(ref posX, ref posY);
                    break;
                case SimpleSlideUIAnimationsEnum.SlideToBottom:
                    if (_element.GetY() < _destinationY)
                    {
                        posY += _actualVelocity * deltaTime;
                    }
                    else
                    {
                        Finished = true;
                    }

                    if (_element.GetY() > (_destinationY / 2))
                    {
                        _actualVelocity *= _velocityLostPercentage;
                    }

                    UpdatePosition(ref posX, ref posY);
                    break;
                case SimpleSlideUIAnimationsEnum.SlideToLeft:
                    if (_element.GetX() > _destinationX)
                    {
                        posX -= _actualVelocity * deltaTime;
                    }
                    else
                    {
                        Finished = true;
                    }

                    if (_element.GetX() < (_destinationX / 2))
                    {
                        _actualVelocity *= _velocityLostPercentage;
                    }

                    UpdatePosition(ref posX, ref posY);
                    break;
                case SimpleSlideUIAnimationsEnum.SlideToRight:
                    if (_element.GetX() < _destinationX)
                    {
                        posX += _actualVelocity * deltaTime;
                    }
                    else
                    {
                        Finished = true;
                    }

                    if (_element.GetX() > (_destinationX / 2))
                    {
                        _actualVelocity *= _velocityLostPercentage;
                    }

                    UpdatePosition(ref posX, ref posY);
                    break;
            }

            _actualVelocity = Mathf.Clamp(_actualVelocity, _minVelocity, _originalVelocity);

            if (Finished)
            {
                var squeezeDirection = Type == SimpleSlideUIAnimationsEnum.SlideToLeft || Type == SimpleSlideUIAnimationsEnum.SlideToRight ? SqueezeDirection.Horizontal : SqueezeDirection.Vertical;

                switch (_element)
                {
                    case Group group:
                        var children = group.GetChildren().Where(x => UtilHelper.HasSqueezeAnimationProperty(x)).ToList();

                        children.ForEach(y =>
                        {
                            SqueezeEverybody(y, squeezeDirection);
                        });
                        break;
                    default:
                        SqueezeEverybody(_element, squeezeDirection);
                        break;
                }
            }
        }

        private void SqueezeEverybody(Element element, SqueezeDirection squeezeDirection)
        {
            switch (element)
            {
                case CustomLabel label:
                    label.SqueezeAnimation?.SqueezeByDirection(squeezeDirection);
                    break;

                case CustomImageButton customImgButton:
                    customImgButton.SqueezeAnimation?.SqueezeByDirection(squeezeDirection);
                    break;

                case ShakableImageButton shakableImageButton:
                    shakableImageButton.SqueezeAnimation?.SqueezeByDirection(squeezeDirection);
                    break;

                case CustomTextButton customTextButton:
                    customTextButton.SqueezeAnimation?.SqueezeByDirection(squeezeDirection);
                    break;
            }
        }

        private void UpdatePosition(ref float posX, ref float posY)
        {
            switch (Type)
            {
                case SimpleSlideUIAnimationsEnum.SlideToTop:
                    posY = Mathf.Clamp(posY, _destinationY, Screen.Height);
                    break;
                case SimpleSlideUIAnimationsEnum.SlideToBottom:
                    posY = Mathf.Clamp(posY, -Screen.Height, _destinationY);
                    break;
                case SimpleSlideUIAnimationsEnum.SlideToLeft:
                    posX = Mathf.Clamp(posX, _destinationX, Screen.Width);
                    break;
                case SimpleSlideUIAnimationsEnum.SlideToRight:
                    posX = Mathf.Clamp(posX, -Screen.Width, _destinationX);
                    break;
            }

            _element.SetPosition(posX, posY);
        }
    }
}
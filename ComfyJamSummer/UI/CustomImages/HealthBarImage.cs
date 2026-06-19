using ComfyJamSummer.Entities.Base;
using ComfyJamSummer.Extensions;
using ComfyJamSummer.UI.Data;
using ComfyJamSummer.UI.Enums;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Nez;
using System.Xml.Linq;
using Color = Microsoft.Xna.Framework.Color;
using Image = Nez.UI.Image;

namespace ComfyJamSummer.UI.CustomImages
{
    public class HealthBarImage : Image
    {
        public Actor Actor { get; private set; }

        public LifeBarType BarType { get; set; }

        public float ValueTaken { get; set; }

        public float ActualValueTaken { get; set; }

        public float TimeLeftToReduceWidth { get; set; }

        public float OriginalWidth { get; private set; }

        public Vector2 Offset { get; set; }

        public HealthBarTypeEnum Name { get; set; }

        public float Alpha { get; set; } = 1f;

        public HealthBarImage(Texture2D texture, Actor actor, LifeBarType type, HealthBarTypeEnum name = HealthBarTypeEnum.Normal, Color color = default) : base(texture)
        {
            Name = name;

            Actor = actor;

            BarType = type;

            UserData = new ActorUIData(actor.Id);

            if (color == default)
            {
                SetColor(name == HealthBarTypeEnum.DamageTaken ? Color.White * 0.94f : type == LifeBarType.Bar ? Constants.GREEN_COLOR : Color.White);
            }
            else
            {
                SetColor(color);
            }

            TimeLeftToReduceWidth = 0.65f;
        }

        public void InitializeDamageTaken(HealthBarImage bar)
        {
            if (Name == HealthBarTypeEnum.DamageTaken)
            {
                var damageWidth = ((Actor.PreviousHP - Actor.ActualHP) / Actor.MaxHP) * (bar.Width());

                var hpBarEnd = (Actor.ActualHP / Actor.MaxHP) * this.Width();

                SetSize(damageWidth, this.GetHeight());
                SetWidth(damageWidth);

                Offset = new Vector2(hpBarEnd - this.Width() / 2, 0);
            }
        }

        public void Process(float actualValue = 0f, float maxValue = 0f)
        {
            if (BarType != LifeBarType.Bar)
            {
                return;
            }

            actualValue = actualValue != 0 ? actualValue : Actor.ActualHP;
            maxValue = maxValue != 0 ? maxValue : Actor.MaxHP;

            switch (Name)
            {
                case HealthBarTypeEnum.Normal:
                    if (OriginalWidth == 0)
                    {
                        OriginalWidth = this.Width();
                    }

                    var width = (actualValue / maxValue) * OriginalWidth;

                    width = Mathf.Clamp(width, 1f, OriginalWidth);
                    SetSize(width, this.GetHeight());
                    break;

                case HealthBarTypeEnum.DamageTaken:
                    if (TimeLeftToReduceWidth <= 0)
                    {
                        var alpha = Alpha;

                        alpha -= 3 * Time.DeltaTime;

                        alpha = Mathf.Clamp01(alpha);

                        SetColor(Constants.RED_COLOR * alpha);
                        Alpha = alpha;

                        var newWidth = GetWidth() * (0.85f);

                        newWidth = Mathf.Clamp(newWidth, 0, PreferredHeight);

                        SetSize(newWidth, GetHeight());
                    }
                    else
                    {
                        TimeLeftToReduceWidth -= Time.DeltaTime;
                    }
                    break;
            }

        }

        public enum LifeBarType
        {
            Border,
            Background,
            Bar
        }
    }
}
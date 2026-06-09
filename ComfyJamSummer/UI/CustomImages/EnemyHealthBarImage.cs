using ComfyJamSummer.Entities.Creatures;
using ComfyJamSummer.Enums;
using ComfyJamSummer.UI.Data;
using ComfyJamSummer.UI.Enums;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Nez;
using Nez.Sprites;
using Color = Microsoft.Xna.Framework.Color;
using Image = Nez.UI.Image;

namespace ComfyJamSummer.UI.CustomImages
{
    public class EnemyHealthBarImage : Image
    {
        public Creature Mob { get; private set; }

        public LifeBarType BarType { get; set; }


        public float DamageTaken { get; set; }

        public float ActualDamageTaken { get; set; }

        public float TimeLeftToReduceWidth { get; set; }

        public float OriginalWidth { get; private set; }

        public Vector2 Offset { get; set; }

        public HealthBarTypeEnum Name { get; set; }

        public float Alpha { get; set; } = 1f;

        public EnemyHealthBarImage(Texture2D texture, Creature mob, LifeBarType type, HealthBarTypeEnum name = HealthBarTypeEnum.Normal) : base(texture)
        {
            Name = name;

            Mob = mob;

            BarType = type;

            UserData = new EnemyUIData(mob.Id);

            SetColor(name == HealthBarTypeEnum.DamageTaken ? Color.White * 0.94f : Color.White * 0.82f);

            TimeLeftToReduceWidth = 0.55f;

            if (name == HealthBarTypeEnum.DamageTaken)
            {
                var animator = mob.GetComponent<SpriteAnimator>();

                if (animator != null)
                {
                    var damageWidth = ((mob.PreviousHP - mob.ActualHP) / mob.MaxHP) * PreferredWidth;

                    var hpBarEnd = (mob.ActualHP / mob.MaxHP) * PreferredWidth;

                    Offset = new Vector2(hpBarEnd, 0);
                    SetSize(damageWidth, this.GetHeight());
                    SetWidth(damageWidth);
                }
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
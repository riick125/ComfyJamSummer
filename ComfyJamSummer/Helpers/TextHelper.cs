using ComfyJamSummer.Components.Extensions;
using ComfyJamSummer.Entities.Base;
using Microsoft.Xna.Framework;
using Nez;
using Nez.BitmapFonts;
using System;
using System.Linq;
using System.Text;

namespace ComfyJamSummer.Helpers
{
    public static class TextHelper
    {
        private static readonly float _widthLimit = 210;

        public static Entity CreateText(BitmapFont font, string text, Microsoft.Xna.Framework.Color color, Vector2 pos = default, bool besideText = false, bool smallText = false, int maxLineWidth = 0)
        {
            var textEntity = besideText ? Core.Scene.AddEntity(new BesideText() { Color = color }) : Core.Scene.CreateEntity("");

            text = font.WrapText(text, maxLineWidth != 0 ? maxLineWidth : _widthLimit);

            textEntity.SetScale(smallText ? 0.75f : 0.5f);

            if (textEntity is BesideText besideTextEntity)
            {
                besideTextEntity.Font = font;
            }

            textEntity.AddComponent(
                new TextComponent(
                font,
                text,
                pos,
                color
                )
                { RenderLayer = 0 }
            );

            return textEntity;
        }

        public static void CreateGoingUpBesideText(BesideTextConfig config, bool needComponent = true)
        {
            var txt = CreateBesideText(config, needComponent);
            txt.IsGoingUp = true;
            config.Dispose();
        }

        public static BesideText CreateFollowBesideText(BesideTextConfig config, bool needComponent = true, bool isOverriding = false)
        {
            var txt = CreateBesideText(config, needComponent);
            txt.IsGoingUp = false;
            config.Dispose();

            return txt;
        }

        private static void CreateFollowTypingBesideText(BesideTextConfig config, bool needComponent = true)
        {
            if (CheckIfTalkTextAlreadyExists(config))
            {
                return;
            }

            config.HasTypingTextEffect = true;
            var txt = CreateBesideText(config, needComponent);
            txt.IsGoingUp = false;
            config.Dispose();
        }

        public static BesideText CreateBesideText(BesideTextConfig config, bool needComponent = true)
        {
            var customFont = UtilHelper.CustomFont();

            if (customFont == null)
            {
                return null;
            }

            config.Offset += new Vector2(Nez.Random.Range(1, 10) * Nez.Random.MinusOneToOne(), Nez.Random.Range(1, 10) * Nez.Random.MinusOneToOne());

            var txt = (BesideText)TextHelper.CreateText(config.SmallText ? customFont.FontSmallest() : customFont.FontNormal, config.Text, config.Color, Vector2.Zero, true, config.SmallText, config.MaxLineWidth);
            txt.EntityId = config.Target.Id;
            txt.Duration = config.Duration;
            txt.TimeLeftToDisappear = txt.Duration;
            txt.Text = config.Text;
            txt.Offset = config.Offset;
            txt.OriginalAlpha = config.Alpha;
            txt.Alpha = txt.OriginalAlpha;
            txt.ShouldWink = config.ShouldWink;
            txt.IsPermanent = config.IsPermanent;
            txt.HasTypingTextEffect = config.HasTypingTextEffect;
            txt.IsTalk = config.IsTalk;

            if (txt.IsTalk)
            {
                var spriteWidth = 0;
                var spriteHeight = 0;

                if (config.Target is Animated animated)
                {
                    spriteWidth = animated.SpriteWidth;

                    spriteHeight = animated.SpriteHeight;
                }

                var auxText = txt.Font.WrapText(txt.Text, _widthLimit);

                var measureString = txt.Font.MeasureString(auxText);

                config.Offset = new Vector2(-measureString.X / 4, (spriteHeight / 2) + measureString.Y / 2);
                txt.Offset = config.Offset;
            }

            if (needComponent)
            {
                txt.AddComponent(new BesideTextComponent(config.Target, config.Offset, config.Duration, config.Alpha));
            }

            return txt;
        }

        public static string RevertString(string s)
        {
            char[] arr = s.ToCharArray();
            Array.Reverse(arr);
            return new string(arr);
        }

        private static bool CheckIfTalkTextAlreadyExists(BesideTextConfig config)
        {
            var texts = Core.Scene.EntitiesOfType<BesideText>().Where(x => x.EntityId == config.Target.Id && x.IsTalk);

            return texts.Any();
        }

        public static void Talk(Animated entity, string phrase, float duration = 2.35f)
        {
            if (entity == null)
            {
                return;
            }

            if (string.IsNullOrEmpty(phrase))
            {
                return;
            }

            if (phrase.Length > 10)
            {
                duration = 5;
            }

            var config = new BesideTextConfig(entity, phrase, default, true, duration);
            config.IsTalk = true;
            config.HasTypingTextEffect = true;

            TextHelper.CreateFollowTypingBesideText(config);
        }

        //public static void Talk(Creature creature, List<GameText> phrases, float duration = 2.35f)
        //{
        //    if (!ValidatePhrase(creature, phrases))
        //    {
        //        return;
        //    }

        //    var filtered = phrases.Where(x => x.Value != creature.LastPhraseSpoken).ToList();

        //    filtered.Shuffle();

        //    var rdmPhrase = filtered[Nez.Random.Range(0, filtered.Count)];
        //    creature.LastPhraseSpoken = rdmPhrase.Value;

        //    if (rdmPhrase.Value.Length > 10)
        //    {
        //        duration = 5;
        //    }

        //    var config = new BesideTextConfig(creature, rdmPhrase.Value, default, true, duration);
        //    config.IsTalk = true;

        //    TextHelper.CreateFollowTypingBesideText(config);
        //}

        //private static bool ValidatePhrase(Creature creature, List<GameText> phrases, float duration = 2.35f)
        //{
        //    if (phrases == null)
        //    {
        //        return false;
        //    }

        //    if (phrases.Count <= 0)
        //    {
        //        return false;
        //    }

        //    if (creature == null)
        //    {
        //        return false;
        //    }

        //    var debuffText = creature.DebuffReceivedIconComponent;

        //    if (debuffText != null && debuffText.Icons != null && debuffText.Icons.Any())
        //    {
        //        return false;
        //    }

        //    return true;
        //}

        public static string AddSpacesBeforeUppercase(string input)
        {
            if (string.IsNullOrEmpty(input))
                return input;

            StringBuilder result = new StringBuilder();

            foreach (char c in input)
            {
                if (char.IsUpper(c) && result.Length > 0)
                {
                    result.Append(' ');
                }
                result.Append(c);
            }

            return result.ToString();
        }
        public static string AddUnderscoreBeforeUppercase(string input)
        {
            if (string.IsNullOrEmpty(input))
                return input;

            StringBuilder result = new StringBuilder();

            foreach (char c in input)
            {
                if (char.IsUpper(c) && result.Length > 0)
                {
                    result.Append('_');
                }
                result.Append(c);
            }

            return result.ToString();
        }

        public static string CapitalizeFirstLetter(string input)
        {
            if (string.IsNullOrEmpty(input))
                return input;

            return char.ToUpper(input[0]) + input.Substring(1);
        }
    }
}
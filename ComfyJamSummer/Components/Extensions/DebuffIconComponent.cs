using System.Collections.Generic;
using System.Linq;
using ComfyJamSummer.Entities.Creatures;
using ComfyJamSummer.Entities.Debuffs;
using ComfyJamSummer.Helpers;
using Microsoft.Xna.Framework;
using Nez;

namespace ComfyJamSummer.Components.Extensions
{
    public class DebuffIconComponent : Component, IUpdatable
    {
        public List<DebuffIcon> Icons;

        private Vector2 _defaultPosition;

        private bool _isGivingDebuffs;
        public bool IsGivingDebuffs { get { return _isGivingDebuffs; } private set { } }

        public DebuffIconComponent(bool isGivingDebuffs = false)
        {
            Icons = new List<DebuffIcon>();

            _isGivingDebuffs = isGivingDebuffs;
        }

        public override void OnAddedToEntity()
        {
            if (!this.Entity.GetType().IsSubclassOf(typeof(Creature)))
            {
                this.RemoveComponent();

                return;
            }

            var creature = this.Entity as Creature;

            if (creature != null)
            {
                var height = creature.SpriteHeight / 3.2f;

                height *= _isGivingDebuffs ? 1 : -1;

                _defaultPosition = new Vector2(creature.SpriteWidth / 1.8f, height);
            }

            if (_isGivingDebuffs)
            {
                foreach (var debuff in creature.DebuffsToGive)
                {
                    AddDebuffIcon(debuff);
                }
            }

            base.OnAddedToEntity();
        }

        public void AddDebuffIcon(Debuff debuff)
        {
            var localOffsetX = _defaultPosition.X;

            if (Icons.Any())
            {
                var spacingMultiplier = 1.2f;

                localOffsetX = _defaultPosition.X + (Icons.First().SpriteWidth * spacingMultiplier);
            }

            var offset = new Vector2(localOffsetX, _defaultPosition.Y);

            var prefabs = UtilHelper.Prefabs();

            if (prefabs != null)
            {
                var debuffIcon = prefabs.GetDebuffIcon(debuff, offset);

                if (_isGivingDebuffs)
                {
                    debuffIcon.SetScale(0.75f);
                }

                Core.Scene.AddEntity(debuffIcon);

                Icons.Add(debuffIcon);
            }
        }

        public void DestroyIcons()
        {
            if (Icons != null)
            {
                foreach (var item in Icons)
                {
                    if (!item.IsDestroyed)
                        item.Destroy();
                }

                Icons.Clear();
            }
        }

        public void Update()
        {
            var creature = this.Entity as Creature;

            if (!_isGivingDebuffs)
            {
                var deltaTime = Time.DeltaTime;

                for (int i = Icons.Count - 1; i >= 0; i--)
                {
                    var icon = Icons[i];

                    if (icon.TimeLeftToDisappear > 0)
                    {
                        if (icon.TimeLeftToDisappear < (icon.Duration * 0.15f))
                        {
                            icon.Alpha -= 5 * deltaTime;

                            icon.Animator.SetColor(Constants.SPRITE_COLOR * icon.Alpha);
                        }

                        icon.Position = creature.Position + icon.Offset;

                        icon.TimeLeftToDisappear -= deltaTime;
                    }
                    else
                    {
                        var newOffsetX = icon.Offset.X;

                        if (!icon.IsDestroyed && icon.Scene != null)
                            icon.Destroy();

                        Icons.RemoveAt(i);

                        for (int j = i; j < Icons.Count; j++)
                        {
                            if (j != i)
                            {
                                if (j - 1 >= 0 && j - 1 < Icons.Count)
                                {
                                    newOffsetX = Icons[j - 1].Offset.X;
                                }
                            }

                            Icons[j].Offset = new Vector2(newOffsetX, Icons[j].Offset.Y);
                        }
                    }
                }
            }
            else
            {
                foreach (var icon in Icons)
                {
                    icon.Position = creature.Position + icon.Offset;

                    //icon.Offset = new Vector2(newOffsetX, Icons[j].Offset.Y);
                }
            }
        }
    }
}
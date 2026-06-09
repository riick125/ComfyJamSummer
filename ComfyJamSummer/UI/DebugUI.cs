using System;
using System.Linq;
using ComfyJamSummer.Components.Extensions;
using ComfyJamSummer.Helpers;
using ComfyJamSummer.Manager;
using ComfyJamSummer.Prefab;
using ComfyJamSummer.UI.Base;
using Microsoft.Xna.Framework.Input;
using Nez;
using Nez.UI;

namespace ComfyJamSummer.UI
{
    public class DebugUI : BaseUI
    {
        Table _tableMain;

        CustomFont _customFont;

        public DebugUI(GameManager manager, Prefabs prefabs) : base(manager, prefabs)
        {
            _container.SetVisible(!_container.IsVisible());
        }

        #region public
        public override void OnAddedToEntity()
        {
            base.OnAddedToEntity();

            this.Entity.AddComponent(new DebugComponent());

            _customFont = UtilHelper.CustomFont();

            CreateMainTable();
        }

        public override void OnRemovedFromEntity()
        {
            base.OnRemovedFromEntity();

            this.Entity.RemoveAllComponents();
        }

        public override void Update()
        {
            base.Update();

            if (Core.Scene != null && Core.Scene.Camera != null)
            {
                this.Entity.SetPosition(Core.Scene.Camera.Position);
            }

            if (Input.IsKeyPressed(Keys.F9))
            {
                _container.SetVisible(!_container.IsVisible());
            }

            if (Input.IsKeyPressed(Keys.Enter))
            {
                CreateMainTable();
            }
        }
        #endregion

        #region private
        void CreateMainTable()
        {
            if (_tableMain != null)
            {
                _tableMain.Remove();
            }
            _container.ClearChildren();

            var padLeftRight = Screen.Width * 0.01f;
            var padTopBottom = Screen.Height * 0.01f;

            var multiplierPad = 5;

            _tableMain = _container.AddElement(new Table() { });
            _tableMain.Defaults();
            _tableMain.SetWidth(Screen.Width / 6);
            _tableMain.SetHeight(Screen.Height);
            _tableMain.SetBackground(new PrimitiveDrawable(Microsoft.Xna.Framework.Color.DarkGray));
            _tableMain.PadLeft(padLeftRight)
                .PadRight(padLeftRight)
                .PadTop(padTopBottom)
                .PadBottom(padTopBottom);
            _tableMain.Left();
            var debugRenderCheckBox = CreateCheckBox("Debug Render");

            debugRenderCheckBox.OnChanged += enabled => Core.DebugRenderEnabled = enabled;
            debugRenderCheckBox.IsChecked = Core.DebugRenderEnabled;

            var debugUiCheckBox = CreateCheckBox("Debug UI", DebugAllUI);
            debugUiCheckBox.PadBottom(padTopBottom * multiplierPad);

            // hack
            var skin = Skin.CreateDefaultSkin();
            _tableMain.Add(new TextButton("Kill Them All", skin));
            _tableMain.Row();

            var suicideTxtButton = _tableMain.Add(new TextButton("Suicide", skin));
            _tableMain.Row();

            suicideTxtButton.SetPadBottom(padTopBottom * multiplierPad);

            _tableMain.Add(new Label("Enemies:", skin));
            _tableMain.Row();

            var selectBoxCreatures = new SelectBox<string>(skin);
            selectBoxCreatures.SetItems(new string[] { "Juvenildo", "Evandro", "Cesar", "Ronilda" });
            _tableMain.Add(selectBoxCreatures);
            _tableMain.Row();

            var txtButton = new TextButton("Spawn", skin);
            txtButton.OnClicked += SpawnEnemy;

            _tableMain.Add(txtButton);

            var cells = _tableMain.GetCells();
            var padCellTop = _tableMain.GetHeight() * 0.0075f;
            var padCellHorizontal = _tableMain.GetWidth() * 0.0075f;

            foreach (var cell in cells)
            {
                if (cell.GetPadTop() <= 0)
                {
                    cell.SetPadTop(padCellTop);
                }

                if (cell.GetPadBottom() <= 0)
                {
                    cell.SetPadBottom(padCellTop);
                }

                cell.SetAlign(Align.Left);

                if (cell.HasElement())
                {
                    var btn = cell.GetElement<Element>();

                    switch (btn)
                    {
                        case Label lbl:
                            var styleLbl = lbl.GetStyle();

                            styleLbl.Font = _customFont.FontNormal;

                            lbl.SetStyle(styleLbl);
                            break;

                        case TextButton txtBtn:
                            var style = txtBtn.GetStyle();

                            style.Font = _customFont.FontSmall;

                            txtBtn.SetStyle(style);
                            break;

                        case SelectBox<string> selectBox:
                            var styleSelectBox = selectBox.GetStyle();
                            var styleListBox = selectBox.GetListBox().GetStyle();

                            styleSelectBox.Font = _customFont.FontSmall;
                            styleListBox.Font = _customFont.FontSmall;

                            selectBox.SetStyle(styleSelectBox);
                            break;
                    }
                }
            }
        }

        private void SpawnEnemy(Button obj)
        {
        }

        private void KillThemAll()
        {
        }

        private void Suicide()
        {
        }

        private void DebugAllUI(bool obj)
        {
            if (Core.Scene == null)
            {
                return;
            }

            var entities = Core.Scene.EntitiesOfType<Entity>().Where(x => x.HasComponent<UICanvas>());

            foreach (var item in entities)
            {
                var component = item.GetComponent<UICanvas>();

                if (component != null)
                {
                    var elements = component.Stage.GetElements().Where(x => x.GetType() == typeof(Container) || x.GetType().IsSubclassOf(typeof(Container)));

                    foreach (var elem in elements)
                    {
                        var converted = elem as Container;

                        if (converted != null)
                        {
                            converted.SetDebug(obj, true);

                            var tables = converted.GetChildren().Where(x => x.GetType() == typeof(Table)).Cast<Table>();

                            foreach (var table in tables)
                            {
                                table.SetDebug(obj);
                            }
                        }
                    }
                }
            }
        }

        CheckBox CreateCheckBox(string text, Action<bool> onChanged = null)
        {
            var checkBox = _tableMain.Add(new CheckBox(text, Skin.CreateDefaultSkin())).GetElement<CheckBox>();

            if (onChanged != null)
            {
                checkBox.OnChanged += onChanged;
            }

            _tableMain.Row();

            return checkBox;
        }

        private void SetSomeBool(bool isVisible) => Core.Instance.IsMouseVisible = isVisible;
        #endregion
    }
}
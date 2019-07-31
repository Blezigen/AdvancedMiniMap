using System;
using Ensage;
using Ensage.Common;
using Ensage.Common.Menu;
using Ensage.Common.Objects.UtilityObjects;
using Ensage.SDK.Input;
using Ensage.SDK.Menu;
using SharpDX;

namespace AdvancedMiniMap.Scripts
{
    public class BasePanelScript : BaseScript
    {
        private Sleeper _sleeper;
        private Vector2 _globalDif;
        private bool _movableLoaded;
        private IInputManager _inputManager;
        protected MenuItem<bool> Movable { get; set; }

        public Vector2 PanelPos { get; set; }
        public Vector2 PanelTopBarSize { get; set; }

        public MenuFactory MenuPosition { get; set; }
        public MenuItem<StringList> PositionType { get; set; }
        public MenuItem<Slider> PositionGlobalY { get; set; }
        public MenuItem<Slider> PositionGlobalX { get; set; }
        public MenuItem<Slider> PositionLocalX { get; set; }
        public MenuItem<Slider> PositionLocalY { get; set; }

        public Vector2 Position => new Vector2(PositionGlobalX.Value + PositionLocalX.Value, PositionGlobalY.Value + PositionLocalY.Value);


        public void LoadMovable(IInputManager valueInput)
        {
            if (_movableLoaded)
                return;
            _movableLoaded = true;
            _sleeper = new Sleeper();
            _inputManager = valueInput;
        }

        public BasePanelScript(Config config, string menuTitle, MenuFactory parentMenu = null) : base(config, menuTitle, parentMenu)
        {
            MenuPosition = Menu.Menu("Position", "position");
            PositionType = Menu.Item("Render type", new StringList("fixed", "absolute"));
            Movable = MenuPosition.Item("Movable", false);

            PositionGlobalX = MenuPosition.Item("Global X", new Slider(0, 0, 2500));
            PositionGlobalY = MenuPosition.Item("Global Y", new Slider(0, 0, 2500));
            PositionLocalX = MenuPosition.Item("Local X", new Slider(0, -50, 50));
            PositionLocalY = MenuPosition.Item("Local Y", new Slider(0, -50, 50));

            LoadMovable(Config.Main.Context.Value.Input);
            PanelPos = new Vector2(PositionGlobalX, PositionGlobalY);
            PanelTopBarSize = new Vector2(100, 16);
        }

        public override void OnDraw(object sender, EventArgs e)
        {
            var pos = new Vector2(PanelPos.X, PanelPos.Y);
            if (Movable && PositionType.Value != "fixed")
                if (CanMoveWindowRender(ref pos, PanelTopBarSize, true))
                {
                    PositionGlobalX.Item.SetValue(new Slider((int) pos.X, 0, 2500));
                    PositionGlobalY.Item.SetValue(new Slider((int) pos.Y, 0, 2500));
                    PanelPos = new Vector2(PositionGlobalX, PositionGlobalY);
                }
        }

        private bool CanMoveWindowRender(ref Vector2 startPos, Vector2 size, bool drawMovablePosition)
        {
            if (drawMovablePosition)
            {
                var rect = new RectangleF(Position.X, Position.Y, PanelTopBarSize.X, PanelTopBarSize.Y);
                var backColor = System.Drawing.Color.FromArgb(155, 155, 155, 155);
                var borderColor = System.Drawing.Color.FromArgb(0,0,0);

                Render.DrawFilledRectangle(rect, borderColor,backColor,2f);
            }
            var mPos = Game.MouseScreenPosition;
            if (Utils.IsUnderRectangle(mPos, startPos.X, startPos.Y, size.X, size.Y))
            {
                if ((_inputManager.ActiveButtons & MouseButtons.LeftDown) != 0)
                {
                    if (!_sleeper.Sleeping)
                    {
                        _globalDif = mPos - startPos;
                        _sleeper.Sleep(500);
                    }
                    startPos = mPos - _globalDif;
                    return true;
                }
            }
            return false;
        }
    }
}

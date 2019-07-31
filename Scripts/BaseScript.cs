using System;
using System.ComponentModel;
using System.Windows.Input;
using AdvancedMiniMap.Utilities;
using Ensage;
using Ensage.SDK.Menu;
using Ensage.SDK.Renderer;

namespace AdvancedMiniMap.Scripts
{
    public abstract class BaseScript : IBaseScript, IDisposable
    {
        /** Enable if script exist timer or other realtime update inform**/
        protected bool OnInGameUpdateEventEnable = false;

        protected MenuFactory Menu { get; }
        protected string MenuTitle;
        protected MenuItem<bool> Enable { get; set; }
        public MenuItem<bool> EnableAltHider { get; set; }
        protected Config Config { get; }

        protected IRenderer  Render { get; set; }
        protected RenderMode RenderMode => Drawing.RenderMode;

        protected bool IsDx9 => RenderMode == RenderMode.Dx9;
        protected bool IsDx11 => RenderMode == RenderMode.Dx11;

        public bool RAltIsDown { get; set; } = false;
        public bool LAltIsDown { get; set; } = false;

        public bool AltIsDown => RAltIsDown || LAltIsDown;

        protected BaseScript(Config config, string menuTitle, MenuFactory parentMenu = null)
        {
            Config = config;
            MenuTitle = menuTitle;

            Menu = parentMenu != null ? parentMenu.Menu(menuTitle) : Config.Factory.Menu(menuTitle);

            Enable = Menu.Item("Enable", true);
            EnableAltHider = Menu.Item("Enable hide UI to Alt", true);
            Render = Config.Main.Renderer;
        }



        public virtual void Load()
        {
            LoadTexture(RenderMode, Render);

            if (Enable)
            {
                OnActivate();
                Render.Draw += OnDraw;

                if (EnableAltHider)
                {
                    Game.OnWndProc += OnWndProc;
                }

                if (OnInGameUpdateEventEnable)
                    Game.OnIngameUpdate += OnInGameUpdate;
                ConsoleUtility.InfoWriteLine($"Script \"{MenuTitle}\" activated.");
            }

            Enable.PropertyChanged += EnableOnPropertyChanged;
        }

        protected virtual void OnInGameUpdate(EventArgs args){ }

        public virtual void UnLoad()
        {
            OnDeActivate();
            Enable.PropertyChanged -= EnableOnPropertyChanged;
        }

        private void EnableOnPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (Enable)
            {
                OnActivate();
                Render.Draw += OnDraw;
                Game.OnWndProc += OnWndProc;

                if (OnInGameUpdateEventEnable)
                    Game.OnIngameUpdate += OnInGameUpdate;

                ConsoleUtility.InfoWriteLine($"[{MenuTitle}] activated.");
            }
            else
            {
                OnDeActivate();
                Render.Draw -= OnDraw;
                Game.OnWndProc -= OnWndProc;

                if (OnInGameUpdateEventEnable)
                    Game.OnIngameUpdate -= OnInGameUpdate;

                ConsoleUtility.InfoWriteLine($"[{MenuTitle}] deactivated.");
            }
        }

        public virtual void OnWndProc(WndEventArgs args)
        {
            if (args.Msg == 0x0104)
            {
                var key = KeyInterop.KeyFromVirtualKey((int)args.WParam);
                if (key == Key.RightAlt)
                    RAltIsDown = true;

                if (key == Key.LeftAlt)
                    LAltIsDown = true;
            }
            if (args.Msg == 0x0105)
            {
                var key = KeyInterop.KeyFromVirtualKey((int)args.WParam);

                if (key == Key.RightAlt)
                    RAltIsDown = false;

                if (key == Key.LeftAlt)
                    LAltIsDown = false;
            }
        }

        public virtual void LoadTexture(RenderMode renderMode, IRenderer render) {}
        public virtual void OnDraw(object sender, EventArgs e){}

        public virtual void OnActivate(){}
        public virtual void OnDeActivate(){}
        public virtual void Dispose(){}
    }
}

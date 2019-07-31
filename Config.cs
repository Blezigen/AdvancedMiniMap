using System;
using AdvancedMiniMap.Scripts;
using Ensage.SDK.Menu;
using Ensage.SDK.Renderer;
using SharpDX;

namespace AdvancedMiniMap
{
    public class Config : IDisposable
    {
        public IRenderer Render { get; set; }
        public ITextureManager TextureManager { get; set; }

        public Config(AdvancedMiniMap plugin)
        {
            Factory = MenuFactory.Create("Advanced MiniMap");
            Factory.Target.SetFontColor(Color.YellowGreen);
            Main = plugin;
            DebugMessages = Factory.Item<bool>("Enable Debug");

            Render = Main.Context.Value.Renderer;
            TextureManager = Main.Context.Value.Renderer.TextureManager;

            MiniMapTowerScript = new MiniMapTowerScript(this, "Tower on MiniMap");
        }

        public bool EnableDebug = true;
        public AdvancedMiniMap Main;
        public MenuItem<bool> DebugMessages { get; set; }
        public MenuFactory Factory { get; }
        public MiniMapTowerScript MiniMapTowerScript { get; set; }


        public void Dispose()
        {
            MiniMapTowerScript.Dispose();

            Factory?.Dispose();
        }
    }
}

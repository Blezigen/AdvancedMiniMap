using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AdvancedMiniMap.Scripts;
using Ensage;
using Ensage.SDK.Menu;
using Ensage.SDK.Renderer;
using Ensage.SDK.Renderer.DX11;
using Ensage.SDK.VPK;
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

            if (Drawing.RenderMode == RenderMode.Dx11)
            {
                Render = new D3D11Renderer(
                    Main.D11Context,
                    Main.BrushCache,
                    new TextFormatCache(Main.D11Context),
                    new D3D11TextureManager(Main.D11Context, new VpkBrowser()));
                TextureManager = (D3D11TextureManager)Render.TextureManager;
            }
            else
            {
                Render = Main.Context.Value.Renderer;
                TextureManager = Main.Context.Value.Renderer.TextureManager;
            }

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

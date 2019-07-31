using System;
using System.ComponentModel.Composition;
using AdvancedMiniMap.Utilities;
using Ensage;
using Ensage.SDK.Renderer;
using Ensage.SDK.Renderer.DX11;
using Ensage.SDK.Renderer.DX9;
using Ensage.SDK.Service;
using Ensage.SDK.Service.Metadata;

namespace AdvancedMiniMap
{

    [ExportPlugin("AdvancedMiniMap", author: "Blezigen", version: "1.0.1")]
    public class AdvancedMiniMap : Plugin
    {
        public Lazy<IServiceContext> Context { get; set; }
        public ID3D11Context D11Context { get; }
        public ID3D9Context D9Context { get; set; }
        public BrushCache BrushCache { get; }
        public IRendererManager Renderer { get; set; }
        public Config Config;
        public Hero Owner;
        public Database Database;

        [ImportingConstructor]
        public AdvancedMiniMap(
            [Import] Lazy<IServiceContext> context,
            [Import] Lazy<ID3D11Context> d11Context,
            [Import] Lazy<BrushCache> brushCache,
            [Import] Lazy<ID3D9Context> d9Context)
        {
            Context = context;
            if (Drawing.RenderMode == RenderMode.Dx11)
            {
                D11Context = d11Context.Value;
                BrushCache = brushCache.Value;
            }
            else
            {
                D9Context = d9Context.Value;
            }
        }

        protected override void OnActivate()
        {
            ConsoleUtility.SetPrefix("[Advanced MiniMap]  ");
            Renderer = Context.Value.Renderer;
            Owner = (Hero)Context.Value.Owner;

            Config = new Config(this);
            ConsoleUtility.SetConfig(Config);
            ConsoleUtility.InfoWriteLine("Config loaded");

            Database = new Database();
            ConsoleUtility.InfoWriteLine("Database loaded");

            InitLocalScripts();
        }

        private void InitLocalScripts()
        {
            Config.MiniMapTowerScript.Load();
        }

        protected override void OnDeactivate()
        {
            Config.MiniMapTowerScript.UnLoad();
        }
    }
}

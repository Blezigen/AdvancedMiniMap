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
        public IRendererManager Renderer { get; set; }
        public Config Config;
        public Hero Owner;
        public Database Database;

        [ImportingConstructor]
        public AdvancedMiniMap([Import] Lazy<IServiceContext> context)
        {
            Context = context;
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

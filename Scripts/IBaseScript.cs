using System;
using Ensage;
using Ensage.SDK.Renderer;

namespace AdvancedMiniMap.Scripts
{
    public interface IBaseScript
    {
        void LoadTexture(RenderMode renderMode, IRenderer render);
        void OnDraw(object sender, EventArgs e);
        void OnActivate();
        void OnDeActivate();
    }
}

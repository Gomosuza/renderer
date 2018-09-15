using Microsoft.Xna.Framework.Graphics;
using Renderer;
using Renderer.Brushes;

namespace Terrain
{
    /// <summary>
    /// Brush that renders a skybox by ignoring depth buffer and applying the texture on all sides.
    /// </summary>
    public class SkyboxBrush : TextureBrush
    {
        private DepthStencilState _depth;

        /// <summary>
        /// Creates a new skybox brush using the provided texture.
        /// </summary>
        /// <param name="texture"></param>
        public SkyboxBrush(Texture2D texture) : base(texture)
        {
        }

        public override bool IsPrepared => base.IsPrepared && _depth != null;

        public override void Prepare(IRenderContext renderContext)
        {
            base.Prepare(renderContext);
            _depth = new DepthStencilState
            {
                DepthBufferEnable = false
            };
        }

        public override void Configure(BasicEffect effect)
        {
            base.Configure(effect);
            effect.GraphicsDevice.DepthStencilState = _depth;
        }
    }
}
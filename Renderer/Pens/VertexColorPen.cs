using Microsoft.Xna.Framework.Graphics;
using Renderer.Brushes;

namespace Renderer.Pens
{
    /// <summary>
    /// The <see cref="VertexColorPen"/> will use the vertex color for drawing.
    /// </summary>
    public class VertexColorPen : Pen
    {
        private readonly CullMode? _cullMode;
        private RasterizerState _rasterizer;
        private SamplerState _sampler;
        private bool _isPrepared;

        /// <summary>
        /// Creates a new instance of the VertexColorPen.
        /// </summary>
        /// <param name="cullMode">The explicit cull mode to be used. If not set the default mode of the rendercontext will be used.</param>
        public VertexColorPen(CullMode? cullMode = null)
        {
            _cullMode = cullMode;
        }

        /// <summary>
        /// Whether or not this pen is already prepared for drawing.
        /// If false and someone wants to use this pen, <see cref="Brush.Prepare"/> must be called before using the pen
        /// in order to prevent undetermined side effects.
        /// </summary>
        public override bool IsPrepared => _isPrepared;

        /// <summary>
        /// Configures the given effect to use this brush.
        /// </summary>
        /// <param name="effect"></param>
        public override void Configure(BasicEffect effect)
        {
            effect.LightingEnabled = false;
            effect.FogEnabled = false;
            effect.VertexColorEnabled = true;
            effect.TextureEnabled = false;

            if (_rasterizer != null)
                effect.GraphicsDevice.RasterizerState = _rasterizer;

            effect.GraphicsDevice.SamplerStates[0] = _sampler;
        }

        /// <summary>
        /// Prepares this brush for drawing.
        /// Wil be called before rendering it if <see cref="Pen.IsPrepared"/> returned false.
        /// </summary>
        public override void Prepare(IRenderContext renderContext)
        {
            _sampler = new SamplerState
            {
                Filter = TextureFilter.LinearMipPoint
            };
            if (_cullMode.HasValue)
            {
                _rasterizer = new RasterizerState
                {
                    CullMode = _cullMode.Value,
                    FillMode = FillMode.WireFrame,
                    DepthBias = -0.1f,
                    MultiSampleAntiAlias = true
                };
            }
            _isPrepared = true;
        }
    }
}
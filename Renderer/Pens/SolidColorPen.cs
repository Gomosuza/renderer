using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Renderer.Brushes;

namespace Renderer.Pens
{
    /// <summary>
    /// A pen that draws with a solid color.
    /// </summary>
    public class SolidColorPen : Pen
    {
        private readonly CullMode? _cullMode;
        private Color _color;
        private RasterizerState _rasterizer;
        private SamplerState _sampler;
        private bool _isPrepared;
        private Vector3 _precalculated;

        /// <summary>
        /// Creates a new instance of the solidcolor pen with the specific color.
        /// </summary>
        /// <param name="color"></param>
        /// <param name="cullMode">The explicit cull mode to be used. If not set the default mode of the rendercontext will be used.</param>
        public SolidColorPen(Color color, CullMode? cullMode = null)
        {
            _cullMode = cullMode;
            Color = color;
        }

        /// <summary>
        /// The color that the current pen is using.
        /// </summary>
        public Color Color
        {
            get { return _color; }
            set
            {
                _color = value;
                // precalculate internal color format as this usually doesn't change every update
                // this way we don't recaclulate on every draw
                _precalculated = ColorConverter.Convert(Color);
            }
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
            effect.AmbientLightColor = Vector3.Zero;
            effect.DiffuseColor = _precalculated;
            effect.FogEnabled = false;
            effect.VertexColorEnabled = false;
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
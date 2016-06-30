using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Renderer.Brushes
{
	/// <summary>
	/// A brush that paints with a single color.
	/// </summary>
	public sealed class SolidColorBrush : Brush
	{

		private Color _color;
		private Vector3 _precalculated;
		private readonly SamplerState _sampler;




		/// <summary>
		/// Create a new solid color brush with the specific color.
		/// </summary>
		/// <param name="color"></param>
		public SolidColorBrush(Color color)
		{
			Color = color;
			// better filter helps for solid objects (esp. borders)
			_sampler = new SamplerState
			{
				Filter = TextureFilter.LinearMipPoint
			};
		}




		/// <summary>
		/// The color used by the brush.
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
		/// Whether or not this brush is already prepared for drawing.
		/// If false and someone wants to use this brush, <see cref="Brush.Prepare"/> must be called before using the brush
		/// in order to prevent undetermined side effects.
		/// </summary>
		public override bool IsPrepared => true;




		/// <summary>
		/// Returns a string that represents the current object.
		/// </summary>
		/// <returns>
		/// A string that represents the current object.
		/// </returns>
		/// <filterpriority>2</filterpriority>
		public override string ToString()
		{
			return $"Color: {Color}";
		}

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

			effect.GraphicsDevice.SamplerStates[0] = _sampler;
		}

		/// <summary>
		/// Prepares this brush for drawing.
		/// Wil be called before rendering it if <see cref="Brush.IsPrepared"/> returned false.
		/// </summary>
		public override void Prepare(IRenderContext renderContext)
		{ }


	}
}
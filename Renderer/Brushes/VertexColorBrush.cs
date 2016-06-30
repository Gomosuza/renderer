using Microsoft.Xna.Framework.Graphics;

namespace Renderer.Brushes
{
	/// <summary>
	/// A brush suitable for <see cref="VertexPositionColor"/> or any other vertex with color.
	/// Will always use the color provided by the vertices.
	/// </summary>
	public class VertexColorBrush : Brush
	{
		private static VertexColorBrush _vertexColorBrush;

		/// <summary>
		/// Returns a default brush instance that can be used.
		/// </summary>
		public static VertexColorBrush Default => _vertexColorBrush ?? (_vertexColorBrush = new VertexColorBrush());

		/// <summary>
		/// Whether or not this brush is already prepared for drawing.
		/// If false and someone wants to use this brush, <see cref="Brush.Prepare"/> must be called before using the brush
		/// in order to prevent undetermined side effects.
		/// </summary>
		public override bool IsPrepared => true;

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
		}

		/// <summary>
		/// Prepares this brush for drawing.
		/// Wil be called before rendering it if <see cref="IsPrepared"/> returned false.
		/// </summary>
		public override void Prepare(IRenderContext renderContext)
		{ }
	}
}
using Microsoft.Xna.Framework.Graphics;

namespace Renderer.Brushes
{
	/// <summary>
	/// Describes a material that defines the visual aspects of an object (mostly meshes).
	/// There can be several kinds of materials.
	/// </summary>
	public abstract class Brush
	{

		/// <summary>
		/// Whether or not this brush is already prepared for drawing.
		/// If false and someone wants to use this brush, <see cref="Prepare"/> must be called before using the brush
		/// in order to prevent undetermined side effects.
		/// </summary>
		public abstract bool IsPrepared { get; }

		/// <summary>
		/// Configures the given effect to use this brush.
		/// </summary>
		/// <param name="effect"></param>
		public abstract void Configure(BasicEffect effect);

		/// <summary>
		/// Prepares this brush for drawing.
		/// Wil be called before rendering it if <see cref="IsPrepared"/> returned false.
		/// </summary>
		public abstract void Prepare(IRenderContext renderContext);
	}
}
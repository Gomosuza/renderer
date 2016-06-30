using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Renderer.Brushes;

namespace Renderer.Pens
{
	/// <summary>
	/// A pen that outlines a drawing.
	/// This will generally result in the wireframe being drawn over whichever object it is attached to.
	/// </summary>
	public class Pen
	{
		private readonly Brush _brush;

		/// <summary>
		/// Creates a new pen using the provided color.
		/// This is used to draw an object outline.
		/// </summary>
		/// <param name="color"></param>
		public Pen(Color color)
		{
			_brush = new SolidColorBrush(color);
		}

		/// <summary>
		/// Whether or not this pen is already prepared for drawing.
		/// If false and someone wants to use this pen, <see cref="Brush.Prepare"/> must be called before using the pen
		/// in order to prevent undetermined side effects.
		/// </summary>
		public virtual bool IsPrepared => _brush.IsPrepared;

		/// <summary>
		/// Configures the given effect to use this brush.
		/// </summary>
		/// <param name="effect"></param>
		public virtual void Configure(BasicEffect effect)
		{
			_brush.Configure(effect);
		}

		/// <summary>
		/// Prepares this brush for drawing.
		/// Wil be called before rendering it if <see cref="Brush.IsPrepared"/> returned false.
		/// </summary>
		public virtual void Prepare(IRenderContext renderContext)
		{
			_brush.Prepare(renderContext);
		}
	}
}
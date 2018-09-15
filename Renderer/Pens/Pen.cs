using Microsoft.Xna.Framework.Graphics;
using Renderer.Brushes;

namespace Renderer.Pens
{
    /// <summary>
    /// A pen that outlines a drawing.
    /// This will generally result in the wireframe being drawn over whichever object it is attached to.
    /// </summary>
    public abstract class Pen
    {
        /// <summary>
        /// Whether or not this pen is already prepared for drawing.
        /// If false and someone wants to use this pen, <see cref="Brush.Prepare"/> must be called before using the pen
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
        /// Wil be called before rendering it if <see cref="Pen.IsPrepared"/> returned false.
        /// </summary>
        public abstract void Prepare(IRenderContext renderContext);
    }
}
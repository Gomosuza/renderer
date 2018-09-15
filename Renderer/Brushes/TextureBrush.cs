using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Renderer.Brushes
{
    /// <summary>
    /// A brush that paints with a texture.
    /// </summary>
    public class TextureBrush : TextureColorBrush
    {
        /// <summary>
        /// Creates a textured brush using an existing texture.
        /// </summary>
        /// <param name="texture"></param>
        public TextureBrush(Texture2D texture) : base(texture, Color.White)
        {
        }

        /// <summary>
        /// Returns a string that represents the current object.
        /// </summary>
        /// <returns>
        /// A string that represents the current object.
        /// </returns>
        /// <filterpriority>2</filterpriority>
        public override string ToString()
        {
            return $"Texture brush: {Width}x{Height}";
        }
    }
}
using Microsoft.Xna.Framework;

namespace Renderer
{
    /// <summary>
    /// Helper class for colors.
    /// </summary>
    public static class ColorConverter
    {
        /// <summary>
        /// Converts from <see cref="Color"/> to <see cref="Vector3"/>.
        /// </summary>
        /// <param name="color"></param>
        /// <returns></returns>
        public static Vector3 Convert(Color color)
        {
            return new Vector3(color.R / 255f, color.G / 255f, color.B / 255f);
        }
    }
}
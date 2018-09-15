using Microsoft.Xna.Framework;

namespace Renderer.Extensions
{
    /// <summary>
    /// Helper methods for the <see cref="GameTime"/> object.
    /// </summary>
    public static class GameTimeExtensions
    {
        /// <summary>
        /// Returns the elapsed seconds of the gametime object.
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        public static float GetElapsedSeconds(this GameTime dt)
        {
            return (float)dt.ElapsedGameTime.TotalSeconds;
        }

        /// <summary>
        /// Returns the total elapsed seconds of the gametime object.
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        public static float GetTotalElapsedSeconds(this GameTime dt)
        {
            return (float)dt.TotalGameTime.TotalSeconds;
        }
    }
}
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Terrain
{
    /// <summary>
    /// Helper class to procedurally generate textures.
    /// </summary>
    public class TextureGenerator
    {
        private readonly GraphicsDevice _graphicsDevice;

        public TextureGenerator(GraphicsDevice graphicsDevice)
        {
            _graphicsDevice = graphicsDevice;
        }

        /// <summary>
        /// Creates a skybox texture which is essentially a texture that starts of blue at the bottom and becomes brighter towards the top.
        /// </summary>
        /// <returns></returns>
        public Texture2D CreateSkyboxTexture()
        {
            var texture = new RenderTarget2D(_graphicsDevice, 512, 512);
            var pixels = new Color[texture.Width * texture.Height];
            for (int y = 0; y < texture.Height; y++)
                for (int x = 0; x < texture.Width; x++)
                {
                    int i = x + y * texture.Width;
                    const int b = 255;
                    // basic blue gradient from bottom to top for now
                    int g;
                    var r = g = (int)((float)(texture.Height - y) / texture.Height * 128 + 127);
                    pixels[i] = Color.FromNonPremultiplied(r, g, b, 255);
                }
            texture.SetData(pixels);
            return texture;
        }

        /// <summary>
        /// Creates an ugly texture that changes color from top to bottom: white, brown, green, light brown (snow, dirt, gras and sand).
        /// Uses noise to make dirt, gras and sand look somewhat ok.
        /// </summary>
        /// <returns></returns>
        public Texture2D CreateTerrainTexture()
        {
            var texture = new RenderTarget2D(_graphicsDevice, 1024, 1024);
            var pixels = new Color[texture.Width * texture.Height];

            var tc1 = Color.Snow;
            var tc2 = Color.SaddleBrown;
            var tc3 = Color.LawnGreen;
            var tc4 = Color.SandyBrown;

            var targetLevel1 = .8f;
            var targetLevel2 = .5f;
            var targetLevel3 = .2f;
            for (int y = 0; y < texture.Height; y++)
            {
                var hProgress = y / (float)texture.Height;
                Color color;
                if (hProgress > targetLevel1)
                    color = tc1;
                else if (hProgress > targetLevel2)
                    color = tc2;
                else if (hProgress > targetLevel3)
                    color = tc3;
                else
                    color = tc4;
                for (int x = 0; x < texture.Width; x++)
                {
                    int i = x + y * texture.Width;
                    pixels[i] = color;
                }
            }
            texture.SetData(pixels);
            return texture;
        }
    }
}
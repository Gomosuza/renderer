using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace TerrainSample
{
	/// <summary>
	/// Helper class to procedurally generate textures.
	/// </summary>
	public class TextureGenerator
	{
		/// <summary>
		/// Creates a skybox texture which is essentially a texture that starts of blue at the bottom and becomes brighter towards the top.
		/// </summary>
		/// <param name="device"></param>
		/// <returns></returns>
		public Texture2D CreateSkyboxTexture(GraphicsDevice device)
		{
			var texture = new RenderTarget2D(device, 512, 512);
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
	}
}
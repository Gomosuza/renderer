using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Renderer.Brushes;
using Renderer.Meshes;
using System;

namespace Renderer.Extensions
{
	/// <summary>
	/// Extensions for the <see cref="IRenderContext"/> class.
	/// </summary>
	public static class DefaultRenderContextExtensions
	{
		/// <summary>
		/// Allows to swap render target in a simple manner.
		/// Note that the returned object is disposable and on calling Dispose the original rendertarget binding is restored.
		/// Recommend way of usage: using statement (all rendering within the using block).
		/// </summary>
		/// <param name="renderContext"></param>
		/// <param name="renderTarget">The rendertarget to write to. Use null if you want to render to the actuall backbuffer.</param>
		/// <param name="usage"></param>
		/// <returns></returns>
		public static IDisposable RenderTo(this IRenderContext renderContext, RenderTarget2D renderTarget, BackBufferSwapUsage usage = BackBufferSwapUsage.WpfSupport)
		{
			return new RenderTargetHelper(renderContext, renderTarget, usage);
		}

		/// <summary>
		/// Helper method that draws a basic texture in an inefficient way.
		/// Use SpriteBatch for your actual 2D code if you need to draw more stuff.
		/// This will insert one quad per texture call and no batching will occur.
		/// </summary>
		/// <param name="renderContext"></param>
		/// <param name="texture"></param>
		/// <param name="rectangle"></param>
		/// <param name="color"></param>
		public static void DrawTexture(this IRenderContext renderContext, Texture2D texture, Rectangle rectangle, Color color)
		{
			// default clip space coordinates, filling the entire screen, their range is -1;1, note that -1 for x is left but -1 for Y is bottom, so we need to flip y to get default coordinate system
			// recalculate to the position where the rectangle actually is
			var vp = renderContext.GraphicsDevice.Viewport.Bounds;
			if (!vp.Intersects(rectangle))
				return; // outside of view area

			// gives us value between 0 and 1, but we need value between -1 and 1, so multiply by 2 and subtract 1
			var minX = rectangle.Left / (float)vp.Width * 2 - 1;
			var maxX = rectangle.Right / (float)vp.Width * 2 - 1;
			// y coordinates must also be flipped
			var minY = 1 - rectangle.Top / (float)vp.Height * 2;
			var maxY = 1 - rectangle.Bottom / (float)vp.Height * 2;

			// this wastes resources as it rebuilds the vertices each draw call but we warned the user that this isn't an efficient way of drawing
			var desc = new TextureMeshDescriptionBuilder();
			desc.AddPlaneXy(minX, maxX, minY, maxY, 0, false, Vector2.One);
			var mesh = renderContext.MeshCreator.CreateMesh(desc);

			// half pixel offset as required by DirectX 9: http://drilian.com/2008/11/25/understanding-half-pixel-and-half-texel-offsets/
			// we get the size of a pixel by divinfing 1 / width and 1 / height, negate so we move back to top left corner
			var proj = Matrix.Identity * Matrix.CreateTranslation(-1f / vp.Width, -1f / vp.Height, 0);
			// using identity for world, view & projection leaves us with just the clip space coordinates calculated above

			renderContext.DrawMesh(mesh, Matrix.Identity, Matrix.Identity, proj, new TextureColorBrush(texture, color));
		}
	}
}
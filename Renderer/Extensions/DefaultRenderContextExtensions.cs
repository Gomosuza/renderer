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

			if (_quad == null)
			{
				// default clip space coordinates to fill entire screen
				const int minX = -1;
				const int maxX = 1;
				const int minY = 1;
				const int maxY = -1;
				// only create mesh once
				var desc = new TextureMeshDescriptionBuilder();
				desc.AddPlaneXy(minX, maxX, minY, maxY, 0, false, Vector2.One);
				_quad = renderContext.MeshCreator.CreateMesh(desc);
			}

			// quad is always filling entire clipspace, apply transform based on rectangle dimensions
			// clip space is -1 to 1 on x and 1 to -1 on y coordinate, so first scale down then transform. also note that origin is center of screen so we translate by 0.5f - .. to offset the origin to top left
			var world = Matrix.CreateScale(new Vector3(rectangle.Width / (float)vp.Width, rectangle.Height / (float)vp.Height, 0)) *
						Matrix.CreateTranslation(new Vector3(rectangle.X / (float)vp.Width * 2 - 1f + rectangle.Width / (float)vp.Width, 1f - rectangle.Y / (float)vp.Height * 2 - rectangle.Height / (float)vp.Height, 0));
			renderContext.DrawMesh(_quad, world, Matrix.Identity, Matrix.Identity, new TextureColorBrush(texture, color));
		}

		/// <summary>
		/// Used by DrawTexture method only.
		/// </summary>
		private static Mesh _quad;
	}
}
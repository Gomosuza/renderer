using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Renderer;
using Renderer.Brushes;
using Renderer.Extensions;
using Renderer.Meshes;
using Renderer.Pens;
using System;
using System.Linq;

namespace RenderingTargetSample
{
	public class Game : Microsoft.Xna.Framework.Game
	{
		private readonly GraphicsDeviceManager _graphicsDeviceManager;
		private IRenderContext _renderContext;
		private Texture2D _pixel;
		private Mesh _mesh;
		private RenderTarget2D _rt1, _rt2, _rt3;
		private ICamera _camera;

		public Game()
		{
			_graphicsDeviceManager = new GraphicsDeviceManager(this);
		}

		protected override void Initialize()
		{
			Window.Title = "RenderTargetSample - Esc to quit";
			base.Initialize();
			IsMouseVisible = true;
			IsFixedTimeStep = false;

			_pixel = new RenderTarget2D(GraphicsDevice, 1, 1);
			_pixel.SetData(new[] { Color.White });
			_renderContext = new DefaultRenderContext(_graphicsDeviceManager, Content);
			var builder = new TextureMeshDescriptionBuilder();
			builder.AddBox(new BoundingBox(Vector3.Zero, Vector3.One * 10), Vector2.One);
			var creator = new MeshCreator(GraphicsDevice);
			_mesh = creator.CreateMesh(builder);

			_camera = new StaticCamera(GraphicsDevice, Vector3.UnitZ * 50);

			_rt1 = CreateRenderTarget(GraphicsDevice, GraphicsDevice.Viewport.Width, GraphicsDevice.Viewport.Height);
			_rt2 = CreateRenderTarget(GraphicsDevice, GraphicsDevice.Viewport.Width, GraphicsDevice.Viewport.Height);
			_rt3 = CreateRenderTarget(GraphicsDevice, GraphicsDevice.Viewport.Width, GraphicsDevice.Viewport.Height);
		}

		private static RenderTarget2D CreateRenderTarget(GraphicsDevice device, int w, int h)
		{
			var rt = new RenderTarget2D(device, w, h, false, SurfaceFormat.Color, DepthFormat.Depth24, 1, RenderTargetUsage.PreserveContents);
			var colors =
				Enumerable.Range(0, w * h)
					.Select(i => Color.White)
					.ToArray();
			rt.SetData(colors);
			return rt;
		}

		protected override void Update(GameTime gameTime)
		{
			base.Update(gameTime);
			if (!IsActive)
				return;

			if (Keyboard.GetState().IsKeyDown(Keys.Escape))
				Exit();
			_camera.Update(gameTime);
		}

		protected override void Draw(GameTime gameTime)
		{
			base.Draw(gameTime);

			// this demonstrates the 4 options we have of using ChangeRenderTarget

			// 1. existing rendertarget = back buffer and we set a different rendertarget
			GraphicsDevice.SetRenderTarget(null);
			if (GraphicsDevice.RenderTargetCount > 0)
				throw new NotSupportedException("We set rendertarget to null, thus we should draw directly to back buffer");

			// anything within the using will render to the rendertarget
			using (_renderContext.RenderTo(_rt1, BackBufferSwapUsage.OriginalBehaviour))
			{
				if (GraphicsDevice.RenderTargetCount != 1)
					throw new NotSupportedException("We set rendertarget, thus we should draw directly to it");
				_renderContext.Clear(Color.Blue);

				_renderContext.Attach();
				// render a 3D cube that is rotating
				var s = gameTime.GetTotalElapsedSeconds();
				// cube is 10*10*10, center it so we rotate around its core
				var world = Matrix.CreateTranslation(new Vector3(-5, -5, -5)) * Matrix.CreateRotationY(s) * Matrix.CreateRotationX(s);
				var v = _camera.View;
				var p = _camera.Projection;
				_renderContext.DrawMesh(_mesh, world, v, p, new SolidColorBrush(Color.Green), new SolidColorPen(Color.Black));
				_renderContext.Detach();
			}
			// now we render to the original backbuffer again because ChangeRenderTarget reset the state automatically
			if (GraphicsDevice.RenderTargetCount > 0)
				throw new NotSupportedException("Rendertarget was set to null by Dispose of the using, thus we should draw directly to back buffer again");

			// 2. existing rendertarget and we set a different one
			GraphicsDevice.SetRenderTarget(_rt2);
			using (_renderContext.RenderTo(_rt3, BackBufferSwapUsage.OriginalBehaviour))
			{
				// this should fill rt3 and not rt2
				_renderContext.Clear(Color.Red);
			}
			// this should now fill rt2 and not rt3
			GraphicsDevice.SetRenderTarget(_rt2);
			_renderContext.Clear(Color.Green);

			if (GraphicsDevice.RenderTargetCount != 1)
				throw new NotSupportedException("Rendertarget was set to null by Dispose of the using, thus we should draw directly to back buffer again");

			// 3. existing rendertarget and set to backbuffer
			using (_renderContext.RenderTo(null, BackBufferSwapUsage.OriginalBehaviour))
			{
				if (GraphicsDevice.RenderTargetCount > 0)
					throw new NotSupportedException("Rendertarget was set to null by Dispose of the using, thus we should draw directly to back buffer again");
			}

			GraphicsDevice.SetRenderTarget(null);
			if (GraphicsDevice.RenderTargetCount > 0)
				throw new NotSupportedException("Rendertarget was set to null, thus we should draw directly to back buffer again");
			// 4. back buffer is set and we set to backbuffer
			using (_renderContext.RenderTo(null, BackBufferSwapUsage.OriginalBehaviour))
			{
				if (GraphicsDevice.RenderTargetCount > 0)
					throw new NotSupportedException("Rendertarget was set to null by Dispose of the using, thus we should draw directly to back buffer again");
			}

			if (GraphicsDevice.RenderTargetCount > 0)
				throw new NotSupportedException("Rendertarget was set to null by Dispose of the using, thus we should draw directly to back buffer again");

			// now draw the actual rendertargets onto backbuffer

			// clear with cornflower blue (visible only in the bottom right corner)
			_renderContext.Clear(Color.CornflowerBlue);
			_renderContext.Attach();

			int w = GraphicsDevice.Viewport.Width;
			int h = GraphicsDevice.Viewport.Height;

			// top left rendertarget contains the rotating cube and has a blue background
			_renderContext.DrawTexture(_rt1, new Rectangle(0, 0, w / 2, h / 2), Color.White);
			// top right has the rendertarget we filled with green
			_renderContext.DrawTexture(_rt2, new Rectangle(w / 2, 0, w / 2, h / 2), Color.White);
			// bottom left contains the rendertarget we filled with red
			_renderContext.DrawTexture(_rt3, new Rectangle(0, h / 2, w / 2, h / 2), Color.White);

			var border = new Rectangle(w - 50, h - 50, 50, 50);
			_renderContext.DrawTexture(_pixel, border, Color.Yellow);
			var border2 = new Rectangle(w - 100, h - 100, 100, 50);
			_renderContext.DrawTexture(_pixel, border2, Color.Red);
			_renderContext.Detach();
		}
	}
}
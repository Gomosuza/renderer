using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Renderer.Brushes;
using Renderer.Meshes;
using Renderer.Pens;
using System;

namespace Renderer
{
	/// <summary>
	/// Default render context implementation. Draws directly to back buffer by default and uses basic effect to apply pen and brush effects.
	/// </summary>
	public class DefaultRenderContext : IRenderContext
	{
		private readonly BasicEffect _effect;
		private readonly RasterizerState _fillState;
		private readonly RasterizerState _wireFrameState;
		private bool _attached;

		/// <summary>
		/// Default render context implementation.
		/// </summary>
		public DefaultRenderContext(IGraphicsDeviceService graphicsDeviceService, ContentManager content, CullMode cullMode = CullMode.CullCounterClockwiseFace)
		{
			if (graphicsDeviceService == null)
			{
				throw new ArgumentNullException(nameof(graphicsDeviceService));
			}
			if (graphicsDeviceService.GraphicsDevice == null)
			{
				throw new ArgumentNullException(nameof(graphicsDeviceService));
			}
			if (content == null)
			{
				throw new ArgumentNullException(nameof(content));
			}

			GraphicsDevice = graphicsDeviceService.GraphicsDevice;
			GraphicsDeviceService = graphicsDeviceService;

			Content = content;

			MeshCreator = new MeshCreator(GraphicsDevice);

			_fillState = new RasterizerState
			{
				CullMode = cullMode,
				FillMode = FillMode.Solid,
				MultiSampleAntiAlias = true
			};
			_wireFrameState = new RasterizerState
			{
				CullMode = cullMode,
				FillMode = FillMode.WireFrame,
				DepthBias = -0.1f,
				MultiSampleAntiAlias = true
			};

			_effect = new BasicEffect(GraphicsDevice);
		}

		/// <summary>
		/// A content manager that is used to load content on demand for the current rendercontext.
		/// </summary>
		public ContentManager Content { get; }

		/// <summary>
		/// The attached graphics device.
		/// </summary>
		public GraphicsDevice GraphicsDevice { get; }

		/// <summary>
		/// The attached graphics device service.
		/// </summary>
		public IGraphicsDeviceService GraphicsDeviceService { get; }

		/// <summary>
		/// Allows creation of meshes
		/// </summary>
		public IMeshCreator MeshCreator { get; }

		/// <summary>
		/// Get or set the render target to use.
		/// </summary>
		public RenderTarget2D RenderTarget { get; set; }

		/// <summary>
		/// Clears the target surface the render context is currently writing to.
		/// </summary>
		/// <param name="color"></param>
		public void Clear(Color color)
		{
			GraphicsDevice.BlendState = BlendState.AlphaBlend;
			GraphicsDevice.Clear(color);
		}

		/// <summary>
		/// Must be called before any rendering can occur.
		/// </summary>
		public void Attach()
		{
			if (_attached)
				throw new NotSupportedException("RenderContext is already attached");
			_attached = true;
			GraphicsDevice.DepthStencilState = DepthStencilState.Default;
		}

		/// <summary>
		/// Renders the mesh with either brush or pen. Both are permitted as well.
		/// If none are provided, an exception is thrown.
		/// </summary>
		/// <param name="mesh"></param>
		/// <param name="world"></param>
		/// <param name="view"></param>
		/// <param name="projection"></param>
		/// <param name="brush"></param>
		/// <param name="pen"></param>
		public virtual void DrawMesh(Mesh mesh, Matrix world, Matrix view, Matrix projection, Brush brush = null, Pen pen = null)
		{
			if (!_attached)
				throw new NotSupportedException("RenderContext is not yet attached");

			if (brush == null && pen == null)
			{
				throw new NotSupportedException("You must set either brush, pen or both. Setting none would result in nothing getting drawn.");
			}
			mesh.Attach();

			_effect.World = world;
			_effect.View = view;
			_effect.Projection = projection;

			if (brush != null)
			{
				GraphicsDevice.RasterizerState = _fillState;
				if (!brush.IsPrepared)
				{
					brush.Prepare(this);
				}
				brush.Configure(_effect);

				foreach (var pass in _effect.CurrentTechnique.Passes)
				{
					pass.Apply();
					mesh.Draw();
				}
			}
			if (pen != null)
			{
				GraphicsDevice.RasterizerState = _wireFrameState;
				if (!pen.IsPrepared)
				{
					pen.Prepare(this);
				}
				pen.Configure(_effect);

				foreach (var pass in _effect.CurrentTechnique.Passes)
				{
					pass.Apply();
					mesh.Draw();
				}
			}
			mesh.Detach();
			GraphicsDevice.RasterizerState = _fillState;
		}

		/// <summary>
		/// Must be called after all rendering calls have been made.
		/// This will flush all remaining draw calls and clear the pipeline.
		/// </summary>
		public virtual void Detach()
		{
			if (!_attached)
				throw new NotSupportedException("RenderContext is not attached");
			_attached = false;
		}
	}
}
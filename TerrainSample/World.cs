using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Renderer;
using System.Collections.Generic;

namespace TerrainSample
{
	/// <summary>
	/// World implementation. Dedicated to rendering skybox and terrain.
	/// </summary>
	public class World
	{
		private int _height;
		private int _width;
		private readonly IRenderContext _renderContext;
		private static readonly Dictionary<string, Texture2D> _textureCache = new Dictionary<string, Texture2D>();

		/// <summary>
		/// Creates a new world instance with randomized terrain.
		/// </summary>
		/// <param name="renderContext"></param>
		/// <param name="width"></param>
		/// <param name="height"></param>
		public World(IRenderContext renderContext, int width, int height)
		{
			_renderContext = renderContext;
			_width = width;
			_height = height;
		}

		public void Draw(IRenderContext renderContext, ICamera camera, GameTime dt)
		{

		}
	}
}
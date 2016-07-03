using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Renderer;
using Renderer.Brushes;
using Renderer.Meshes;
using Renderer.Pens;
using System;
using System.Collections.Generic;
using Plane = Renderer.Meshes.Plane;

namespace Terrain
{
	/// <summary>
	/// World implementation. Dedicated to rendering skybox and terrain.
	/// </summary>
	public class World
	{
		private readonly int _height;
		private readonly int _width;
		private readonly IRenderContext _renderContext;
		private Mesh _skybox;
		private Mesh _terrain;
		private SkyboxBrush _skyboxTextureBrush;
		private TextureBrush _terrainBrush;

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

			var textureGenerator = new TextureGenerator(renderContext.GraphicsDevice);
			CreateSkybox(textureGenerator);
			CreateTerrain(textureGenerator);
		}

		/// <summary>
		/// Cell size of each terrain quad.
		/// </summary>
		public int CellSize => 10;

		private void CreateTerrain(TextureGenerator textureGenerator)
		{
			// for now just render a cube so we have a point of reference

			var height = 45;
			var heightmap = CalculateRandomHeightmap(_width + 1, _height + 1, height);
			var terrain = new TextureMeshDescriptionBuilder();
			var vertices = new List<VertexPositionColorTexture>();
			for (int y = 0; y < _height; y++)
				for (int x = 0; x < _width; x++)
				{
					// add a quad per cell
					var xCoord = x * CellSize;
					var zCoord = y * CellSize;
					var bbox = new BoundingBox(new Vector3(xCoord, 0, zCoord), new Vector3(xCoord + CellSize, 0, zCoord + CellSize));

					vertices.AddRange(new[]
				   {
						new VertexPositionColorTexture(new Vector3(bbox.Max.X, heightmap[x+1,y], bbox.Min.Z), Color.White,  new Vector2(heightmap[x+1,y] / (float)height)),
						new VertexPositionColorTexture(new Vector3(bbox.Min.X, heightmap[x,y+1], bbox.Max.Z), Color.White,  new Vector2(heightmap[x,y+1] / (float)height)),
						new VertexPositionColorTexture(new Vector3(bbox.Min.X, heightmap[x,y], bbox.Min.Z), Color.White,  new Vector2(heightmap[x,y] / (float)height)),
						new VertexPositionColorTexture(new Vector3(bbox.Min.X, heightmap[x,y+1], bbox.Max.Z), Color.White,  new Vector2(heightmap[x,y+1] / (float)height)),
						new VertexPositionColorTexture(new Vector3(bbox.Max.X, heightmap[x+1,y], bbox.Min.Z), Color.White,  new Vector2(heightmap[x+1,y] / (float)height)),
						new VertexPositionColorTexture(new Vector3(bbox.Max.X, heightmap[x+1,y+1], bbox.Max.Z), Color.White, new Vector2(heightmap[x+1,y+1] / (float)height))
					});
				}

			terrain.AddVertices(vertices);
			_terrain = _renderContext.MeshCreator.CreateMesh(terrain);

			var terrainTex = _textureCache.GetOrCreateValue("terrain", key => textureGenerator.CreateTerrainTexture());
			_terrainBrush = new TextureBrush(terrainTex);
		}

		private static int[,] CalculateRandomHeightmap(int width, int length, int maxHeight)
		{
			var h = new int[width, length];
			var rnd = new Random();
			for (int y = 0; y < length; y++)
				for (int x = 0; x < width; x++)
				{
					h[x, y] = rnd.Next(0, maxHeight);
				}
			return h;
		}

		private void CreateSkybox(TextureGenerator textureGenerator)
		{
			var skybox = new TextureMeshDescriptionBuilder();
			var bbox = new BoundingBox(-Vector3.One, Vector3.One);

			// add the 4 walls manually
			skybox.AddPlane(bbox, Plane.NegativeX, false, Vector2.One);
			skybox.AddPlane(bbox, Plane.PositiveX, false, Vector2.One);
			skybox.AddPlane(bbox, Plane.NegativeZ, false, Vector2.One);
			skybox.AddPlane(bbox, Plane.PositiveZ, false, Vector2.One);

			_skybox = _renderContext.MeshCreator.CreateMesh(skybox);

			var skyboxTex = _textureCache.GetOrCreateValue("skybox", key => textureGenerator.CreateSkyboxTexture());

			_skyboxTextureBrush = new SkyboxBrush(skyboxTex);
		}

		public void Draw(IRenderContext renderContext, ICamera camera, GameTime dt)
		{
			var camTransform = Matrix.CreateTranslation(camera.GetPosition());

			// always center skybox around player
			DrawSkybox(renderContext, camera, camTransform);
			DrawTerrain(renderContext, camera, Matrix.Identity);
		}

		private void DrawSkybox(IRenderContext renderContext, ICamera camera, Matrix world)
		{
			renderContext.DrawMesh(_skybox, world, camera.View, camera.Projection, _skyboxTextureBrush);
		}

		private void DrawTerrain(IRenderContext renderContext, ICamera camera, Matrix world)
		{
			renderContext.DrawMesh(_terrain, world, camera.View, camera.Projection, _terrainBrush, new SolidColorPen(Color.Black));
		}
	}
}
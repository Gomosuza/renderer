using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Renderer;
using Renderer.Brushes;
using Renderer.Meshes;
using Renderer.Pens;
using System.Collections.Generic;
using Plane = Renderer.Meshes.Plane;

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
		private Mesh _skybox;
		private Mesh _terrain;
		private SkyboxBrush _skyboxTextureBrush;
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

			CreateSkybox();
			CreateTerrain();
		}

		private void CreateTerrain()
		{
			// for now just render a cube so we have a point of reference
			var terrain = new TextureMeshDescriptionBuilder();
			terrain.AddBox(new BoundingBox(Vector3.Zero, Vector3.One * 5), Vector2.One);
			_terrain = _renderContext.MeshCreator.CreateMesh(terrain);
		}

		private void CreateSkybox()
		{
			var skybox = new TextureMeshDescriptionBuilder();
			var bbox = new BoundingBox(-Vector3.One, Vector3.One);

			// add the 4 walls manually
			skybox.AddPlane(bbox, Plane.NegativeX, false, Vector2.One);
			skybox.AddPlane(bbox, Plane.PositiveX, false, Vector2.One);
			skybox.AddPlane(bbox, Plane.NegativeZ, false, Vector2.One);
			skybox.AddPlane(bbox, Plane.PositiveZ, false, Vector2.One);
			
			_skybox = _renderContext.MeshCreator.CreateMesh(skybox);

			var gen = new TextureGenerator();
			var skyboxTex = _textureCache.GetOrCreateValue("skybox", key => gen.CreateSkyboxTexture(_renderContext.GraphicsDevice));

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
			renderContext.DrawMesh(_terrain, world, camera.View, camera.Projection, new SolidColorBrush(Color.Green));
		}
	}
}
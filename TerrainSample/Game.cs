using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Renderer;
using Renderer.Extensions;

namespace TerrainSample
{
	/// <summary>
	/// Basic procedurally generated terrain.
	/// </summary>
	public class Game : Microsoft.Xna.Framework.Game
	{
		private readonly GraphicsDeviceManager _graphicsDeviceManager;
		private IRenderContext _renderContext;
		private FirstPersonCamera _camera;
		private World _world;

		public Game()
		{
			_graphicsDeviceManager = new GraphicsDeviceManager(this)
			{
				PreferMultiSampling = true
			};
		}

		protected override void Initialize()
		{
			base.Initialize();
			IsMouseVisible = false;
			IsFixedTimeStep = false;

			_renderContext = new DefaultRenderContext(_graphicsDeviceManager, Content);

			const int width = 1024;
			const int height = 1024;
			_world = new World(_renderContext, width, height);

			// center camera in world
			var center = new Vector3(_world.CellSize * width / 2f, 50, _world.CellSize * height / 2f);
			_camera = new FirstPersonCamera(GraphicsDevice, center, CameraMode.Plane);
		}

		protected override void Update(GameTime gameTime)
		{
			base.Update(gameTime);

			if (!IsActive)
				return;
			// don't reset camera if window is not focused

			var mouseState = Mouse.GetState();

			var center = new Point(GraphicsDevice.Viewport.Width / 2, GraphicsDevice.Viewport.Height / 2);
			var diff = mouseState.Position - center;
			var t = gameTime.GetElapsedSeconds();
			const float factor = 0.1f;
			_camera.AddHorizontalRotation(diff.X * t * factor);
			_camera.AddVerticalRotation(diff.Y * t * factor);

			Mouse.SetPosition(center.X, center.Y);

			var keyboardState = Keyboard.GetState();

			var movement = Vector3.Zero;
			if (keyboardState.IsKeyDown(Keys.W))
			{
				movement += -Vector3.UnitZ;
			}
			if (keyboardState.IsKeyDown(Keys.A))
			{
				movement += -Vector3.UnitX;
			}
			if (keyboardState.IsKeyDown(Keys.S))
			{
				movement += Vector3.UnitZ;
			}
			if (keyboardState.IsKeyDown(Keys.D))
			{
				movement += Vector3.UnitX;
			}
			_camera.Move(movement);

			_camera.Update(gameTime);
		}

		protected override void Draw(GameTime gameTime)
		{
			base.Draw(gameTime);

			_renderContext.Clear(Color.White);
			_renderContext.Attach();

			_world.Draw(_renderContext, _camera, gameTime);

			_renderContext.Detach();
		}
	}
}
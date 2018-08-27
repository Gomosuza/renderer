using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;
using Terrain;

namespace TerrainSample
{
	/// <summary>
	/// Basic procedurally generated terrain.
	/// </summary>
	public class Game : Microsoft.Xna.Framework.Game
	{
		private readonly GraphicsDeviceManager _graphicsDeviceManager;
		private TerrainScene _terrainScene;

		public Game()
		{
			_graphicsDeviceManager = new GraphicsDeviceManager(this)
			{
				PreferMultiSampling = true
			};
		}

		protected override void Initialize()
		{
			Window.Title = "TerrainSample - Esc to quit";
			base.Initialize();
			IsMouseVisible = false;
			IsFixedTimeStep = false;

			_terrainScene = new TerrainScene(_graphicsDeviceManager, Content);
			Mouse.SetPosition(GraphicsDevice.Viewport.Width / 2, GraphicsDevice.Viewport.Height / 2);
		}

		protected override void Update(GameTime gameTime)
		{
			base.Update(gameTime);

			if (!IsActive)
				return;

			var mouseState = Mouse.GetState();
			var keyboardState = Keyboard.GetState();

			if (keyboardState.IsKeyDown(Keys.Escape))
			{
				Environment.Exit(0);
				return;
			}

			_terrainScene.Update(gameTime, mouseState, keyboardState, Mouse.SetPosition);
		}

		protected override void Draw(GameTime gameTime)
		{
			base.Draw(gameTime);

			_terrainScene.Draw(gameTime);
		}
	}
}
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Renderer;
using Renderer.Extensions;
using System;

namespace Terrain
{
    public class TerrainScene
    {
        private readonly IRenderContext _renderContext;
        private readonly FirstPersonCamera _camera;
        private readonly World _world;

        public TerrainScene(IGraphicsDeviceService graphicsDeviceManager, ContentManager content)
        {
            _renderContext = new DefaultRenderContext(graphicsDeviceManager, content);

            const int width = 1024;
            const int height = 1024;
            _world = new World(_renderContext, width, height);

            // center camera in world
            var center = new Vector3(_world.CellSize * width / 2f, 50, _world.CellSize * height / 2f);
            _camera = new FirstPersonCamera(graphicsDeviceManager.GraphicsDevice, center, CameraMode.Plane);
        }

        public void Draw(GameTime gameTime)
        {
            _renderContext.Clear(Color.White);
            _renderContext.Attach();

            _world.Draw(_renderContext, _camera, gameTime);

            _renderContext.Detach();
        }

        public void Update(GameTime gameTime, MouseState mouseState, KeyboardState keyboardState, Action<int, int> setCursor)
        {
            var center = new Point(_renderContext.GraphicsDevice.Viewport.Width / 2, _renderContext.GraphicsDevice.Viewport.Height / 2);
            var diff = mouseState.Position - center;
            var t = gameTime.GetElapsedSeconds();
            const float factor = 0.1f;
            _camera.AddHorizontalRotation(diff.X * t * factor);
            _camera.AddVerticalRotation(diff.Y * t * factor);

            setCursor(center.X, center.Y);

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
    }
}
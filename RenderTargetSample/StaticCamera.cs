using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace RenderingTargetSample
{
    /// <summary>
    /// Basic camera implementation.
    /// </summary>
    public sealed class StaticCamera : ICamera
    {
        private const float NearZ = 0.5f;
        private readonly GraphicsDevice _device;
        private bool _dirty;

        /// <summary>
        /// Creates a new instance of the camera.
        /// The camera will face negative Z direction by default.
        /// </summary>
        /// <param name="device"></param>
        /// <param name="initialPosition">The starting point of the camera.</param>
        /// <param name="farZ">The far clipping plane.</param>
        public StaticCamera(GraphicsDevice device, Vector3 initialPosition, float farZ = 1000)
        {
            _device = device;
            FarZ = farZ;

            Position = initialPosition;
            _dirty = true;
        }

        /// <summary>
        /// The far plane value that this camera uses.
        /// </summary>
        public float FarZ { get; }

        /// <summary>
        /// The position of the camera in the world.
        /// </summary>
        public Vector3 Position { get; }

        /// <summary>
        /// The projection matrix that represents the current camera.
        /// </summary>
        public Matrix Projection { get; private set; }

        /// <summary>
        /// The view matrix that represents the current camera.
        /// </summary>
        public Matrix View { get; private set; }

        /// <summary>
        /// Updates the cameras internal values.
        /// </summary>
        /// <param name="dt"></param>
        public void Update(GameTime dt) => Update();

        private void Update()
        {
            if (!_dirty)
            {
                return;
            }

            var rotation = Matrix.Identity;

            var rotated = Vector3.Transform(Vector3.UnitZ * -1, rotation);
            var final = Position + rotated;

            var up = Vector3.Transform(Vector3.Up, rotation);
            View = Matrix.CreateLookAt(Position, final, up);

            Projection = Matrix.CreatePerspectiveFieldOfView(MathHelper.PiOver4, _device.Viewport.AspectRatio, NearZ, FarZ);

            _dirty = false;
        }
    }
}
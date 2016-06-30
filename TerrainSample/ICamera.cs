using Microsoft.Xna.Framework;

namespace TerrainSample
{
	/// <summary>
	/// The interface for the camera, describing what portion of the level is being rendered.
	/// Responsible for transforming level to screen coordinates and vice-verca.
	/// </summary>
	public interface ICamera
	{
		/// <summary>
		/// The projection matrix that represents the current camera.
		/// </summary>
		Matrix Projection { get; }

		/// <summary>
		/// The view matrix that represents the current camera.
		/// </summary>
		Matrix View { get; }

		/// <summary>
		/// Calculates the distance to the camera for the given point.
		/// </summary>
		/// <returns></returns>
		float DistanceToCamera(Vector3 world);

		/// <summary>
		/// Calculates the squared distance to the camera for the given point (faster than <see cref="DistanceToCamera"/>.
		/// </summary>
		/// <returns></returns>
		float DistanceToCameraSquared(Vector3 world);

		/// <summary>
		/// Returns the camera position.
		/// </summary>
		/// <returns></returns>
		Vector3 GetPosition();

		/// <summary>
		/// Calculates a ray that travels from the camera position
		/// to the far plane that contains the given screen-space-point.
		/// </summary>
		/// <returns></returns>
		Ray ScreenToWorld(Vector2 screen);

		/// <summary>
		/// Updates the cameras internal values.
		/// </summary>
		/// <param name="dt"></param>
		void Update(GameTime dt);

		/// <summary>
		/// Transforms a position from world to screen coordinates.
		/// </summary>
		/// <returns></returns>
		Vector2 WorldToScreen(Vector3 world);
	}
}
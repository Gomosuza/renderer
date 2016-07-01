namespace Terrain
{
	/// <summary>
	/// Camera modes.
	/// </summary>
	public enum CameraMode
	{
		/// <summary>
		/// In plane mode, the camera will fly directly in the direction it is pointed.
		/// This may e.g. result in the camera flying straight up when forward is pressed and the user is looking straight up.
		/// </summary>
		Plane,
		/// <summary>
		/// In person mode, the camera will be "locked" to the ground. If the player presses forward, it will not alter his vertical position.
		/// </summary>
		Person
	}
}
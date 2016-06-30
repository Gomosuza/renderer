using Microsoft.Xna.Framework.Graphics;

namespace Renderer.Meshes
{
	/// <summary>
	/// The mesh description can be fed to the <see cref="IMeshCreator"/> which will then create a proper mesh from it.
	/// </summary>
	public interface IMeshDescription<out T> where T : struct, IVertexType
	{
		/// <summary>
		/// The primitive type in use.
		/// </summary>
		PrimitiveType PrimitiveType { get; }

		/// <summary>
		/// The total vertex count.
		/// </summary>
		int VertexCount { get; }

		/// <summary>
		/// The actual vertices.
		/// </summary>
		T[] Vertices { get; }
	}
}
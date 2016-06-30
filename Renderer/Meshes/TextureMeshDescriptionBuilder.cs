using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace Renderer.Meshes
{
	/// <summary>
	/// The mesh builder allows the creation of meshes by adding various forms to itself.
	/// By passing it to a <see cref="IMeshCreator"/> a mesh is created by copying the data from its current description.
	/// This type will always use <see cref="PrimitiveType.TriangleList"/> and <see cref="VertexPositionColorTexture"/>.
	/// </summary>
	public sealed class TextureMeshDescriptionBuilder : GenericMeshDescriptionBuilder<VertexPositionColorTexture>
	{
		/// <summary>
		/// Creates a new plane on the XZ axis.
		/// </summary>
		/// <param name="minX"></param>
		/// <param name="maxX"></param>
		/// <param name="minZ"></param>
		/// <param name="maxZ"></param>
		/// <param name="y"></param>
		/// <param name="faceNegativeAxis">If true, will face in negative Y direction, otherwise in positive Y direction.</param>
		/// <param name="tileSize"></param>
		public override void AddPlaneXz(float minX, float maxX, float minZ, float maxZ, float y, bool faceNegativeAxis, Vector2 tileSize)
		{
			// the color we set here doesn't matter because it is overriden by TextureColorBrush's color property
			var vertices = new List<VertexPositionColorTexture>
			{
				new VertexPositionColorTexture(new Vector3(maxX, y, minZ), Color.White, new Vector2(tileSize.X, 0)),
				new VertexPositionColorTexture(new Vector3(minX, y, maxZ), Color.White, new Vector2(0, tileSize.Y)),
				new VertexPositionColorTexture(new Vector3(minX, y, minZ), Color.White, new Vector2(0, 0)),
				new VertexPositionColorTexture(new Vector3(minX, y, maxZ), Color.White, new Vector2(0, tileSize.Y)),
				new VertexPositionColorTexture(new Vector3(maxX, y, minZ), Color.White, new Vector2(tileSize.X, 0)),
				new VertexPositionColorTexture(new Vector3(maxX, y, maxZ), Color.White, new Vector2(tileSize.X, tileSize.Y))
			};

			if (!faceNegativeAxis)
			{
				vertices.Reverse();
			}
			AddVertices(vertices);
		}

		/// <summary>
		/// Creates a new plane on the YZ axis.
		/// </summary>
		/// <param name="minY"></param>
		/// <param name="maxY"></param>
		/// <param name="minZ"></param>
		/// <param name="maxZ"></param>
		/// <param name="x"></param>
		/// <param name="faceNegativeAxis">If true, will face in negative X direction, otherwise in positive X direction.</param>
		/// <param name="tileSize"></param>
		public override void AddPlaneYz(float minY, float maxY, float minZ, float maxZ, float x, bool faceNegativeAxis, Vector2 tileSize)
		{
			// the color we set here doesn't matter because it is overriden by TextureColorBrush's color property
			var vertices = new List<VertexPositionColorTexture>
			{
				new VertexPositionColorTexture(new Vector3(x, maxY, minZ), Color.White, new Vector2(0, 0)),
				new VertexPositionColorTexture(new Vector3(x, minY, minZ), Color.White, new Vector2(0, tileSize.Y)),
				new VertexPositionColorTexture(new Vector3(x, minY, maxZ), Color.White, new Vector2(tileSize.X, tileSize.Y)),
				new VertexPositionColorTexture(new Vector3(x, maxY, maxZ), Color.White, new Vector2(tileSize.X, 0)),
				new VertexPositionColorTexture(new Vector3(x, maxY, minZ), Color.White, new Vector2(0, 0)),
				new VertexPositionColorTexture(new Vector3(x, minY, maxZ), Color.White, new Vector2(tileSize.X, tileSize.Y))
			};

			if (!faceNegativeAxis)
			{
				vertices.Reverse();
			}
			AddVertices(vertices);
		}

		/// <summary>
		/// Creates a new plane on the XY axis.
		/// </summary>
		/// <param name="minX"></param>
		/// <param name="maxX"></param>
		/// <param name="minY"></param>
		/// <param name="maxY"></param>
		/// <param name="z"></param>
		/// <param name="faceNegativeAxis">If true, will face in negative Z direction, otherwise in positive Z direction.</param>
		/// <param name="tileSize"></param>
		public override void AddPlaneXy(float minX, float maxX, float minY, float maxY, float z, bool faceNegativeAxis, Vector2 tileSize)
		{
			// the color we set here doesn't matter because it is overriden by TextureColorBrush's color property
			var vertices = new List<VertexPositionColorTexture>
			{
				new VertexPositionColorTexture(new Vector3(minX, maxY, z), Color.White, new Vector2(0, 0)),
				new VertexPositionColorTexture(new Vector3(maxX, minY, z), Color.White, new Vector2(tileSize.X, tileSize.Y)),
				new VertexPositionColorTexture(new Vector3(minX, minY, z), Color.White, new Vector2(0, tileSize.Y)),
				new VertexPositionColorTexture(new Vector3(maxX, minY, z), Color.White, new Vector2(tileSize.X, tileSize.Y)),
				new VertexPositionColorTexture(new Vector3(minX, maxY, z), Color.White, new Vector2(0, 0)),
				new VertexPositionColorTexture(new Vector3(maxX, maxY, z), Color.White, new Vector2(tileSize.X, 0))
			};

			if (!faceNegativeAxis)
			{
				vertices.Reverse();
			}
			AddVertices(vertices);
		}
	}
}
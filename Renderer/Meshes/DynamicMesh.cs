using Microsoft.Xna.Framework.Graphics;
using System;

namespace Renderer.Meshes
{
	/// <summary>
	/// Mutable, unindexes mesh.
	/// </summary>
	public abstract class DynamicMesh : Mesh
	{
		/// <summary>
		/// Calculates the number of primitives requires for a specific vertex count using the specific primitive type.
		/// </summary>
		/// <param name="type"></param>
		/// <param name="vertexCount"></param>
		/// <returns></returns>
		/// <exception cref="ArgumentException"></exception>
		/// <exception cref="InvalidOperationException"></exception>
		public static int CalcPrimitives(PrimitiveType type, int vertexCount)
		{
			switch (type)
			{
				case PrimitiveType.LineList:
					if (vertexCount == 1 || vertexCount % 2 != 0)
					{
						throw new ArgumentException("LineList requires a vertex-count that is a multiple of 2");
					}

					return vertexCount / 2;

				case PrimitiveType.LineStrip:
					if (vertexCount < 1)
					{
						throw new ArgumentException("LineStrip requires a vertex-count greater 1");
					}

					return vertexCount - 1;

				case PrimitiveType.TriangleList:
					if (vertexCount % 3 != 0)
					{
						throw new ArgumentException("TriangleList requires a vertex-count that is a multiple of 3");
					}

					return vertexCount / 3;

				case PrimitiveType.TriangleStrip:
					if (vertexCount < 3)
					{
						throw new ArgumentException("Not enough triangles, at least 3 required");
					}

					return vertexCount - 2;

				default:
					throw new InvalidOperationException($"Unknown primitive type: {type}");
			}
		}

		/// <summary>
		/// Updates the mesh with the provided vertices.
		/// </summary>
		/// <param name="vertices"></param>
		/// <typeparam name="T"></typeparam>
		public abstract void Update<T>(T[] vertices)
			where T : struct, IVertexType;

		/// <summary>
		/// Updates the mesh with the provided vertices of the provided primitive type.
		/// </summary>
		/// <param name="vertices"></param>
		/// <param name="type"></param>
		/// <typeparam name="T"></typeparam>
		public abstract void Update<T>(T[] vertices, PrimitiveType type)
			where T : struct, IVertexType;
	}
}
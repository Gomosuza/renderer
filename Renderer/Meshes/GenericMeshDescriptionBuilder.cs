using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;

namespace Renderer.Meshes
{
	/// <summary>
	/// This base type implements mesh creation and allows plugging in different vertex types by deriving from the class.
	/// Use instance methods such as <see cref="AddBox"/> to quickly build your mesh.
	/// </summary>
	/// <typeparam name="T"></typeparam>
	public abstract class GenericMeshDescriptionBuilder<T> : IMeshDescription<T> where T : struct, IVertexType
	{
		private readonly List<T> _vertices;

		/// <summary>
		/// Creates a new mesh description builder.
		/// This type will always use <see cref="PrimitiveType.TriangleList"/>.
		/// </summary>
		protected GenericMeshDescriptionBuilder()
		{
			// no need to support other types, we always use this
			// if the user wants other type he can implement it himself
			PrimitiveType = PrimitiveType.TriangleList;
			_vertices = new List<T>();
		}

		/// <summary>
		/// The primitive type in use.
		/// </summary>
		public PrimitiveType PrimitiveType { get; }

		/// <summary>
		/// The total vertex count.
		/// </summary>
		public int VertexCount => _vertices.Count;

		/// <summary>
		/// The actual vertices.
		/// </summary>
		public T[] Vertices => _vertices.ToArray();

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
		public abstract void AddPlaneYz(float minY, float maxY, float minZ, float maxZ, float x, bool faceNegativeAxis, Vector2 tileSize);

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
		public abstract void AddPlaneXy(float minX, float maxX, float minY, float maxY, float z, bool faceNegativeAxis, Vector2 tileSize);

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
		public abstract void AddPlaneXz(float minX, float maxX, float minZ, float maxZ, float y, bool faceNegativeAxis, Vector2 tileSize);

		/// <summary>
		/// Call this method to add vertices to the <see cref="Vertices"/> array from a derived class.
		/// </summary>
		/// <param name="vertices"></param>
		public void AddVertices(List<T> vertices)
		{
			_vertices.AddRange(vertices);
		}

		/// <summary>
		/// Method that allows to add a box to the mesh.
		/// As with all boxes it is outwards facing.
		/// </summary>
		/// <param name="box">The bounding box to add to the mesh.</param>
		/// <param name="tileSize">The texture scaling. The scale is applied equaly on all faces.</param>
		public void AddBox(BoundingBox box, Vector2 tileSize)
		{
			AddPlane(box, Plane.PositiveX, true, tileSize);
			AddPlane(box, Plane.PositiveY, true, tileSize);
			AddPlane(box, Plane.PositiveZ, true, tileSize);

			AddPlane(box, Plane.NegativeX, true, tileSize);
			AddPlane(box, Plane.NegativeY, true, tileSize);
			AddPlane(box, Plane.NegativeZ, true, tileSize);
		}

		/// <summary>
		/// Adds a plane from the bounding box that is on the specific side, and making it face the provided direction.
		/// </summary>
		/// <param name="box"></param>
		/// <param name="side">The side of the cube to draw.</param>
		/// <param name="faceOutwards">If true, will render the outside plane, otherwise the inside plane.</param>
		/// <param name="tileSize"></param>
		public void AddPlane(BoundingBox box, Plane side, bool faceOutwards, Vector2 tileSize)
		{
			switch (side)
			{
				case Plane.NegativeY:
					// Floor
					AddPlaneXz(box.Min.X, box.Max.X, box.Min.Z, box.Max.Z, box.Min.Y, !faceOutwards, tileSize);
					break;

				case Plane.PositiveY:
					// Ceil
					AddPlaneXz(box.Min.X, box.Max.X, box.Min.Z, box.Max.Z, box.Max.Y, faceOutwards, tileSize);
					break;

				case Plane.NegativeZ:
					// wall furthest away from screen
					AddPlaneXy(box.Min.X, box.Max.X, box.Min.Y, box.Max.Y, box.Min.Z, !faceOutwards, tileSize);
					break;

				case Plane.PositiveZ:
					// wall closest to screen
					AddPlaneXy(box.Min.X, box.Max.X, box.Min.Y, box.Max.Y, box.Max.Z, faceOutwards, tileSize);
					break;

				case Plane.NegativeX:
					// wall to the left
					AddPlaneYz(box.Min.Y, box.Max.Y, box.Min.Z, box.Max.Z, box.Min.X, !faceOutwards, tileSize);
					break;

				case Plane.PositiveX:
					// wall to the right
					AddPlaneYz(box.Min.Y, box.Max.Y, box.Min.Z, box.Max.Z, box.Max.X, faceOutwards, tileSize);
					break;

				default:
					throw new ArgumentException();
			}
		}

		/// <summary>
		/// Method that allows to add a room to the mesh.
		/// As with all rooms it faces inwards.
		/// </summary>
		/// <param name="box">The bounding box to add to the mesh.</param>
		/// <param name="tileSize">The texture scaling. The scale is applied equaly on all faces.</param>
		public void AddRoom(BoundingBox box, Vector2 tileSize)
		{
			AddPlane(box, Plane.PositiveX, false, tileSize);
			AddPlane(box, Plane.PositiveY, false, tileSize);
			AddPlane(box, Plane.PositiveZ, false, tileSize);

			AddPlane(box, Plane.NegativeX, false, tileSize);
			AddPlane(box, Plane.NegativeY, false, tileSize);
			AddPlane(box, Plane.NegativeZ, false, tileSize);
		}

		/// <summary>
		/// Removes all vertices from the builder.
		/// </summary>
		public void Clear()
		{
			_vertices.Clear();
		}
	}
}
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;

namespace Renderer.Meshes
{
	/// <summary>
	/// Default implementation of <see cref="IMeshCreator"/>.
	/// </summary>
	public class MeshCreator : IMeshCreator
	{
		private readonly GraphicsDevice _device;

		/// <summary>
		/// Creates a new mesh creator based on the graphics device.
		/// </summary>
		/// <param name="device"></param>
		public MeshCreator(GraphicsDevice device)
		{
			_device = device;
		}

		/// <summary>
		/// Creates an empty dynamic mesh of the given type that allows data to be added to it later.
		/// This type of mesh can be modified later.
		/// </summary>
		/// <param name="type"></param>
		/// <param name="vertexType"></param>
		/// <param name="declaration"></param>
		/// <param name="usage"></param>
		/// <returns></returns>
		public DynamicMesh CreateDynamicMesh(PrimitiveType type, Type vertexType, VertexDeclaration declaration, DynamicMeshUsage usage)
		{
			// according to shawnhar (http://xboxforums.create.msdn.com/forums/t/5136.aspx)
			//
			// ''For persistent geometry (data that is drawn many times without changing), DrawPrimitives with a vertex buffer is dramatically faster.
			// For dynamic geometry(which is created on the fly each time it is drawn), you should use DrawUserPrimitives, however.
			// This is able to do the copy in a more efficient way than if you manually created a vertex buffer, SetData into it, and then drew from that.''
			//
			// However for DynamicMeshUsage.UpdateOften we use our appendingmesh which uses circlular buffer/appends the data to the buffer, it is even faster than DrawUserPrimitives

			switch (usage)
			{
				case DynamicMeshUsage.UpdateOften:
					return new AppendingMesh(_device, vertexType, declaration, type);
				case DynamicMeshUsage.UpdateSeldom:
					return new UpdatableDynamicMesh(_device, type);
				default:
					throw new ArgumentOutOfRangeException(nameof(usage), usage, null);
			}
		}

		/// <summary>
		/// Creates a dynamic mesh based on the provided raw vertices.
		/// This type of mesh can be modified later.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="type"></param>
		/// <param name="vertices"></param>
		/// <param name="usage"></param>
		/// <returns></returns>
		public DynamicMesh CreateDynamicMesh<T>(PrimitiveType type, List<T> vertices, DynamicMeshUsage usage)
			where T : struct, IVertexType
		{
			if (vertices == null)
			{
				throw new ArgumentNullException();
			}
			if (vertices.Count == 0)
			{
				throw new ArgumentException("For dynamic meshes at least one vertex must be provided to get the vertex decleration type to use.");
			}

			var vertexType = typeof(T);
			var decl = vertices[0].VertexDeclaration;

			var mesh = CreateDynamicMesh(type, vertexType, decl, usage);
			mesh.Update(vertices.ToArray());
			return mesh;
		}

		/// <summary>
		/// Creates a dynamic mesh based of the description.
		/// This type of mesh can be modified later.
		/// </summary>
		/// <param name="description"></param>
		/// <param name="usage"></param>
		/// <returns></returns>
		public DynamicMesh CreateDynamicMesh<T>(IMeshDescription<T> description, DynamicMeshUsage usage) where T : struct, IVertexType
		{
			if (description == null)
			{
				throw new ArgumentNullException(nameof(description));
			}

			return CreateDynamicMesh(description.PrimitiveType, description.Vertices, usage);
		}

		/// <summary>
		/// Creates a fixed mesh based of the vertices. It can no longer be modifed.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="type"></param>
		/// <param name="vertices"></param>
		/// <returns></returns>
		public Mesh CreateMesh<T>(PrimitiveType type, List<T> vertices)
			where T : struct, IVertexType
		{
			var mesh = new StaticMesh<T>(_device, type, vertices.ToArray());
			return mesh;
		}

		/// <summary>
		/// Creates a fixed mehs based of the provided description. It can no longer be modified.
		/// </summary>
		/// <param name="description"></param>
		/// <returns></returns>
		public Mesh CreateMesh<T>(IMeshDescription<T> description) where T : struct, IVertexType
		{
			return CreateMesh(description.PrimitiveType, description.Vertices);
		}
	}
}
using Microsoft.Xna.Framework.Graphics;
using System;

namespace Renderer.Meshes
{
	/// <summary>
	/// The static mesh is a vertexbuffer based implementation for fast drawing.
	/// </summary>
	/// <typeparam name="T"></typeparam>
	internal class StaticMesh<T> : Mesh
		where T : struct, IVertexType
	{
		private readonly VertexBuffer _buffer;
		private readonly GraphicsDevice _device;
		private readonly int _primitives;
		private readonly PrimitiveType _type;

		public StaticMesh(GraphicsDevice device, PrimitiveType type, T[] vertices)
		{
			_type = type;
			_device = device;

			if (vertices == null)
			{
				throw new ArgumentNullException(nameof(vertices));
			}

			if (vertices.Length == 0)
			{
				_primitives = 0;
				_buffer = null;
			}
			else
			{
				// Throws in case the primitive count is off...
				_primitives = DynamicMesh.CalcPrimitives(type, vertices.Length);

				var decl = vertices[0].VertexDeclaration;

				_buffer = new VertexBuffer(_device, decl, vertices.Length, BufferUsage.WriteOnly);
				_buffer.SetData(vertices);
			}
		}

		public override int Primitives => _primitives;

		public override PrimitiveType Type => _type;

		public override int Vertices => _buffer?.VertexCount ?? 0;

		/// <summary>
		/// Allows the mesh to attach itself to the graphics device.
		/// Called before <see cref="Mesh.Draw"/>.
		/// </summary>
		public override void Attach()
		{
			if (_primitives == 0)
			{
				return;
			}

			_device.SetVertexBuffer(_buffer);
		}

		/// <summary>
		/// Allows the mesh to dettach itself from the graphics device.
		/// Called after <see cref="Mesh.Draw"/>.
		/// </summary>
		public override void Detach()
		{
			_device.SetVertexBuffer(null);
		}

		/// <summary>
		/// Draw call. Allows the mesh to render itself.
		/// </summary>
		public override void Draw()
		{
			if (_primitives == 0)
			{
				return;
			}

			_device.DrawPrimitives(_type, 0, _primitives);
		}
	}
}
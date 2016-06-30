using Microsoft.Xna.Framework.Graphics;
using System;

namespace Renderer.Meshes
{
	/// <summary>
	/// Implementation of a dynamic mesh that allows its content to be fully replaced at any time.
	/// </summary>
	internal class UpdatableDynamicMesh : DynamicMesh
	{
		private readonly GraphicsDevice _device;

		private int _bufferMaxVertices;
		private int _primitives;
		private PrimitiveType _type;
		private DynamicVertexBuffer _vertexBuffer;
		private int _vertices;

		public UpdatableDynamicMesh(GraphicsDevice device, PrimitiveType type)
		{
			_type = type;
			_device = device;
		}

		public override int Primitives => _primitives;

		public override PrimitiveType Type => _type;

		public override int Vertices => _vertices;

		public override void Update<T>(T[] vertices)
		{
			if (vertices == null)
			{
				throw new ArgumentNullException(nameof(vertices));
			}

			if (vertices.Length == 0)
			{
				_primitives = 0;
				_vertices = 0;
			}
			else
			{
				// Throws in case the primitive count is off...
				_primitives = CalcPrimitives(_type, vertices.Length);

				var decl = vertices[0].VertexDeclaration;

				if (_vertexBuffer == null || _bufferMaxVertices < vertices.Length)
				{
					_vertexBuffer = new DynamicVertexBuffer(_device, decl, vertices.Length, BufferUsage.WriteOnly);

					_bufferMaxVertices = vertices.Length;
				}

				_vertices = vertices.Length;
				_vertexBuffer.SetData(vertices);
			}
		}

		public override void Update<T>(T[] vertices, PrimitiveType type)
		{
			var oldType = _type;

			try
			{
				_type = type;
				Update(vertices);
			}
			catch
			{
				_type = oldType;

				throw;
			}
		}

		public override void Attach()
		{
			if (_primitives == 0)
			{
				return;
			}

			_device.SetVertexBuffer(_vertexBuffer);
		}

		public override void Detach()
		{
			_device.SetVertexBuffer(null);
		}

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
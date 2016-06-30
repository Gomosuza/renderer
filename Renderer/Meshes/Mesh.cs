﻿using Microsoft.Xna.Framework.Graphics;

namespace Renderer.Meshes
{
	/// <summary>
	/// The mesh describes a vertex based 3D object.
	/// </summary>
	public abstract class Mesh
	{
		/// <summary>
		/// Number of primitives.
		/// </summary>
		public abstract int Primitives { get; }

		/// <summary>
		/// Type of primitives.
		/// </summary>
		public abstract PrimitiveType Type { get; }

		/// <summary>
		/// Number of vertices.
		/// </summary>
		public abstract int Vertices { get; }

		/// <summary>
		/// Allows the mesh to attach itself to the graphics device.
		/// Called before <see cref="Draw"/>.
		/// </summary>
		public abstract void Attach();

		/// <summary>
		/// Allows the mesh to dettach itself from the graphics device.
		/// Called after <see cref="Draw"/>.
		/// </summary>
		public abstract void Detach();

		/// <summary>
		/// Draw call. Allows the mesh to render itself.
		/// </summary>
		public abstract void Draw();
	}
}
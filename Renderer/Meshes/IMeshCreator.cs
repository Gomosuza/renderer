using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;

namespace Renderer.Meshes
{
    /// <summary>
    /// The mesh creator helps creating meshes.
    /// It accepts either raw vertices or <see cref="IMeshDescription{T}"/> which must previously be filled with information.
    /// </summary>
    public interface IMeshCreator
    {
        /// <summary>
        /// Creates an empty dynamic mesh of the given type that allows data to be added to it later.
        /// This type of mesh can be modified later.
        /// </summary>
        /// <param name="type"></param>
        /// <param name="vertexType"></param>
        /// <param name="declaration"></param>
        /// <param name="usage"></param>
        /// <returns></returns>
        DynamicMesh CreateDynamicMesh(PrimitiveType type, Type vertexType, VertexDeclaration declaration, DynamicMeshUsage usage);

        /// <summary>
        /// Creates a dynamic mesh based on the provided raw vertices.
        /// This type of mesh can be modified later.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="type"></param>
        /// <param name="vertices"></param>
        /// <param name="usage"></param>
        /// <returns></returns>
        DynamicMesh CreateDynamicMesh<T>(PrimitiveType type, List<T> vertices, DynamicMeshUsage usage)
            where T : struct, IVertexType;

        /// <summary>
        /// Creates a dynamic mesh based of the description.
        /// This type of mesh can be modified later.
        /// </summary>
        /// <param name="description"></param>
        /// <param name="usage"></param>
        /// <returns></returns>
        DynamicMesh CreateDynamicMesh<T>(IMeshDescription<T> description, DynamicMeshUsage usage)
            where T : struct, IVertexType;

        /// <summary>
        /// Creates a fixed mesh based of the vertices. It can no longer be modifed.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="type"></param>
        /// <param name="vertices"></param>
        /// <returns></returns>
        Mesh CreateMesh<T>(PrimitiveType type, List<T> vertices)
            where T : struct, IVertexType;

        /// <summary>
        /// Creates a fixed mehs based of the provided description. It can no longer be modified.
        /// </summary>
        /// <param name="description"></param>
        /// <returns></returns>
        Mesh CreateMesh<T>(IMeshDescription<T> description)
            where T : struct, IVertexType;
    }
}
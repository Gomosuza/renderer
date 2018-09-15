using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Renderer.Brushes;
using Renderer.Meshes;
using Renderer.Pens;
using System;

namespace Renderer
{
    /// <summary>
    /// The rendercontext holds all information required for rendering.
    /// If you mix 2D and 3D graphics you need to be aware that the renderer will make two passes: first it draws all 3D, then all 2D on top.
    /// If for some reason you need immediate rendering (so some 3D stuff ends up on top of 2D stuff) then you need to use multiple render contexts and sort them yourself.
    /// </summary>
    public interface IRenderContext
    {
        /// <summary>
        /// A content manager that is used to load content on demand for the current rendercontext.
        /// </summary>
        ContentManager Content { get; }

        /// <summary>
        /// The attached graphics device.
        /// </summary>
        GraphicsDevice GraphicsDevice { get; }

        /// <summary>
        /// The attached graphics device service.
        /// </summary>
        IGraphicsDeviceService GraphicsDeviceService { get; }

        /// <summary>
        /// Allows creation of meshes
        /// </summary>
        IMeshCreator MeshCreator { get; }

        /// <summary>
        /// Clears the target surface the render context is currently writing to.
        /// </summary>
        /// <param name="color"></param>
        void Clear(Color color);

        /// <summary>
        /// Must be called before any rendering can occur.
        /// </summary>
        void Attach();

        /// <summary>
        /// Must be called after all rendering calls have been made.
        /// This will flush all remaining draw calls and clear the pipeline.
        /// </summary>
        void Detach();

        /// <summary>
        /// Draws a mesh using the provided pen and brush.
        /// Note that while either pen or brush must be set (or both).
        /// </summary>
        /// <param name="mesh"></param>
        /// <param name="world"></param>
        /// <param name="view"></param>
        /// <param name="projection"></param>
        /// <param name="brush"></param>
        /// <param name="pen"></param>
        /// <exception cref="NotSupportedException">Thrown when both brush and pen are null.</exception>
        void DrawMesh(Mesh mesh, Matrix world, Matrix view, Matrix projection, Brush brush = null, Pen pen = null);
    }
}
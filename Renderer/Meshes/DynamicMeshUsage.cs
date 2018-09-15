namespace Renderer.Meshes
{
    /// <summary>
    /// Dynamic mesh usage enum.
    /// Knowing about the usage of dynamic meshes can help improve performance.
    /// </summary>
    public enum DynamicMeshUsage
    {
        /// <summary>
        /// For dynamic geometry (which is created on the fly e.g. each time it is drawn).
        /// </summary>
        UpdateOften,
        /// <summary>
        /// For persistent geometry (data that is drawn many times without changing).
        /// This uses DrawPrimitives with a vertex buffer and is dramatically faster.
        /// </summary>
        UpdateSeldom
    }
}
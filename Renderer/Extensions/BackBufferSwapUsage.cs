namespace Renderer.Extensions
{
    /// <summary>
    /// The behaviour is only used when ChangeRenderTarget is called with a null rendertarget (= user wants to render to backbuffer).
    /// </summary>
    public enum BackBufferSwapUsage
    {
        /// <summary>
        /// The bahviour adds support for non-default behaviour.
        /// Specifically the WPF Interop implementation which never actually uses the xna/monogame back buffer.
        /// Instead it always uses a rendertarget which is of type D3D11Host and can then be rendered by WPF directly.
        /// With this behaviour the back buffer will not be explicitely set to null if a rendertarget is set. Instead it is assumed that the existing rendertarget is the back buffer.
        /// </summary>
        WpfSupport = 0,
        /// <summary>
        /// This behaviour reflects the default xna/monogame behaviour.
        /// If user wants to render to the backbuffer then he gets to directly render to the backbuffer.
        /// Which explicitely sets the rendertarget to null.
        /// </summary>
        OriginalBehaviour = 1
    }
}
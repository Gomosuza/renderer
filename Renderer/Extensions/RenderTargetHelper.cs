using Microsoft.Xna.Framework.Graphics;
using System;

namespace Renderer.Extensions
{
    /// <summary>
    /// Helper class dedicated to swapping rendertargets.
    /// </summary>
    internal class RenderTargetHelper : IDisposable
    {
        private readonly IRenderContext _renderContext;
        private RenderTargetBinding? _originalRenderTarget;

        private readonly bool _resetToBackBuffer;

        /// <summary>
        /// When invoked will insert the renderTargets as the currently active one and save itself a reference back to the original one.
        /// On <see cref="Dispose"/> the rendertarget is detached and the original one is swapped back in.
        /// </summary>
        /// <param name="renderContext"></param>
        /// <param name="renderTarget">The rendertarget to be used.</param>
        /// <param name="usage"></param>
        public RenderTargetHelper(IRenderContext renderContext, RenderTarget2D renderTarget, BackBufferSwapUsage usage)
        {
            _renderContext = renderContext;

            // check if there is an existing rendertarget bound to the graphicsdevice
            // there are 4 possible options. existing rendertarget and target rendertarget can both be null or some rendertarget so we need to check which state it is for us
            // the 4 options: null & null, null & rt, rt & null, rt & rt
            _originalRenderTarget = renderContext.GraphicsDevice.RenderTargetCount > 0
                ? renderContext.GraphicsDevice.GetRenderTargets()[0]
                : (RenderTargetBinding?)null;

            // user requested backbuffer when he only provided one RT which he set to null
            var backBufferRequested = renderTarget == null;
            if (backBufferRequested)
            {
                if (_originalRenderTarget == null)
                {
                    // 1. option, rendertarget is null and user wants to draw to backbuffer -> no need for swapping
                    _resetToBackBuffer = true;
                    return;
                }
                // 3. option: user wants to draw to the back buffer, but a rendertarget already exists

                // user wants to render directly to backbuffer, however someone has already set a rendertarget
                // this is the case e.g. for the WPF interop: https://github.com/MarcStan/MonoGame.Framework.WpfInterop
                // which doesn't write to the normal backbuffer but writes to a 3D11 image host instead (which in turn can be displayed in WPF)
                // so we can't actually render to backbuffer as it won't be visible then
                // instead user selected WpfSupport, so don't set to null but keep the rendertarget that is set
                _resetToBackBuffer = usage != BackBufferSwapUsage.WpfSupport;
            }
            else
            {
                // 2. and 4. option
                // we set a rendertarget and then later reset to the original rendertarget (which is either null or an existing one)
                _resetToBackBuffer = true;
            }

            // only set the target rt if we are actually going to swap back to source
            // otherwise source = target
            if (_resetToBackBuffer)
            {
                renderContext.GraphicsDevice.SetRenderTarget(renderTarget);
            }
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        /// <filterpriority>2</filterpriority>
        public void Dispose()
        {
            // reset back to original buffer if requested
            if (!_resetToBackBuffer)
                return;

            if (_originalRenderTarget.HasValue)
                _renderContext.GraphicsDevice.SetRenderTargets(_originalRenderTarget.Value);
            else
                _renderContext.GraphicsDevice.SetRenderTargets(null);
        }
    }
}
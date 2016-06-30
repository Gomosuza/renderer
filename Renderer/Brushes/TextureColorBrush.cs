using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace Renderer.Brushes
{
	/// <summary>
	/// A brush that paints with a texture and tints with a color.
	/// </summary>
	public class TextureColorBrush : Brush
	{
		private readonly SamplerState _sampler;
		private readonly Texture2D _texture;
		private Vector3 _precalculated;
		private Color _color;

		/// <summary>
		/// Creates a new brush with a texture and color.
		/// </summary>
		/// <param name="texture"></param>
		/// <param name="color"></param>
		public TextureColorBrush(Texture2D texture, Color color)
		{
			if (texture == null)
			{
				throw new ArgumentNullException(nameof(texture));
			}
			_texture = texture;
			// improve rendering of textures with a better filter
			_sampler = new SamplerState
			{
				Filter = TextureFilter.LinearMipPoint
			};
			Color = color;
		}

		/// <summary>
		/// The color used by the brush.
		/// </summary>
		public Color Color
		{
			get { return _color; }
			set
			{
				_color = value;
				// precalculate internal color format as this usually doesn't change every update
				// this way we don't recaclulate on every draw
				_precalculated = ColorConverter.Convert(Color);
			}
		}

		/// <summary>
		/// Returns the height of the texture in use.
		/// </summary>
		public virtual int Height => _texture.Height;

		/// <summary>
		/// Returns the size of the texture in use.
		/// Note that this call will return <see cref="Vector2.Zero"/> if the texture has not yet been loaded via <see cref="Prepare"/>.
		/// </summary>
		public virtual Vector2 Size => new Vector2(Width, Height);

		/// <summary>
		/// Returns the width of the texture in use.
		/// </summary>
		public virtual int Width => _texture.Width;

		/// <summary>
		/// Whether or not this brush is already prepared for drawing.
		/// If false and someone wants to use this brush, <see cref="Brush.Prepare"/> must be called before using the brush
		/// in order to prevent undetermined side effects.
		/// </summary>
		public override bool IsPrepared => true;

		/// <summary>
		/// Configures the given effect to use this brush.
		/// </summary>
		/// <param name="effect"></param>
		public override void Configure(BasicEffect effect)
		{
			effect.AmbientLightColor = Vector3.Zero;
			effect.DiffuseColor = _precalculated;
			effect.Alpha = _color.A / 255f;

			effect.LightingEnabled = false;
			effect.Texture = _texture;
			effect.TextureEnabled = true;
			effect.FogEnabled = false;
			effect.VertexColorEnabled = true;

			// use better filter so textures don't flicker when the screen is moved
			_texture.GraphicsDevice.SamplerStates[0] = _sampler;
			// required to properly render alpha colors
			_texture.GraphicsDevice.BlendState = BlendState.AlphaBlend;
		}

		/// <summary>
		/// Prepares this brush for drawing.
		/// Wil be called before rendering it if <see cref="Brush.IsPrepared"/> returned false.
		/// </summary>
		public override void Prepare(IRenderContext renderContext)
		{ }

		/// <summary>
		/// Returns a string that represents the current object.
		/// </summary>
		/// <returns>
		/// A string that represents the current object.
		/// </returns>
		/// <filterpriority>2</filterpriority>
		public override string ToString()
		{
			return $"Texture & colored brush: {_texture.Width}x{_texture.Height}, Color: {_color}";
		}
	}
}
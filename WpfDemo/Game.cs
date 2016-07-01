using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame.Framework.WpfInterop;
using MonoGame.Framework.WpfInterop.Input;
using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using Terrain;

namespace WpfDemo
{
	public class Game : WpfGame
	{
		private IGraphicsDeviceService _graphicsDeviceManager;
		private WpfKeyboard _keyboard;
		private WpfMouse _mouse;
		private TerrainScene _terrainScene;
		private bool _first = true;

		protected override void Initialize()
		{
			base.Initialize();

			// must be initialized. required by Content loading and rendering (will add itself to the Services)
			_graphicsDeviceManager = new WpfGraphicsDeviceService(this);

			// wpf and keyboard need reference to the host control in order to receive input
			// this means every WpfGame control will have it's own keyboard & mouse manager which will only react if the mouse is in the control
			_keyboard = new WpfKeyboard(this);
			_mouse = new WpfMouse(this);

			_terrainScene = new TerrainScene(_graphicsDeviceManager, Content);
		}

		protected override void Update(GameTime time)
		{
			if (_first)
			{
				// WPF window is not yet loaded when initialize is called, thus SetCursor does not work there, instead call it in the first update
				_mouse.SetCursor(GraphicsDevice.Viewport.Width / 2, GraphicsDevice.Viewport.Height / 2);
				_first = false;
			}
			// every update we can now query the keyboard & mouse for our WpfGame
			var mouseState = _mouse.GetState();
			if (mouseState.X == 0 && mouseState.Y == 0)
			{
				// WPF returns empty mouseposition at startup, until window is at least once rendered. only then will it return proper mouse positions
				// so we deferr updating until we get a valid mousestate
				return;
			}
			var keyboardState = _keyboard.GetState();
			if (!IsActive())
			{
				return;
			}
			if (keyboardState.IsKeyDown(Keys.Escape))
			{
				Environment.Exit(0);
			}
			_terrainScene.Update(time, mouseState, keyboardState, (x, y) => _mouse.SetCursor(x, y));
		}

		private static bool IsActive()
		{
			var activatedHandle = GetForegroundWindow();
			if (activatedHandle == IntPtr.Zero)
			{
				// No window is currently activated
				return false;
			}

			var procId = Process.GetCurrentProcess().Id;
			int activeProcId;
			GetWindowThreadProcessId(activatedHandle, out activeProcId);

			return activeProcId == procId;
		}

		[DllImport("user32.dll", CharSet = CharSet.Auto, ExactSpelling = true)]
		private static extern IntPtr GetForegroundWindow();

		[DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
		private static extern int GetWindowThreadProcessId(IntPtr handle, out int processId);

		protected override void Draw(GameTime time)
		{
			_terrainScene.Draw(time);
		}
	}
}
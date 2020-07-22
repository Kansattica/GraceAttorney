using System;
using System.Collections.Generic;
using System.Text;
using Encompass;
using GraceAttorney.DependencyInjection;
using GraceAttorney.Messages;
using Microsoft.Xna.Framework;

namespace GraceAttorney.Engines
{
	[Receives(typeof(ToggleFullscreenMessage))]
	class FullScreenEngine : Spawner<ToggleFullscreenMessage>
	{
		private readonly GraphicsDeviceManager _graphics;
		private readonly UpdatedSize _windowBounds;
		private Point _windowedScreenSize;
		public FullScreenEngine(GraphicsDeviceManager graphics, UpdatedSize windowBounds)
		{
			_graphics = graphics;
			_windowBounds = windowBounds;
		}

		protected override void Spawn(in ToggleFullscreenMessage message)
		{
			if (!_graphics.IsFullScreen)
				_windowedScreenSize = new Point(_windowBounds.Width, _windowBounds.Height);

			_graphics.ToggleFullScreen();

			if (!_graphics.IsFullScreen)
			{
				_graphics.PreferredBackBufferWidth = _windowedScreenSize.X;
				_graphics.PreferredBackBufferHeight = _windowedScreenSize.Y;
				_graphics.ApplyChanges();
			}
		}
	}
}

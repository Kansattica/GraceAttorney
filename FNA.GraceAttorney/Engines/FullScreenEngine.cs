using System;
using System.Collections.Generic;
using System.Text;
using Encompass;
using FNA.GraceAttorney.Messages;
using Microsoft.Xna.Framework;

namespace FNA.GraceAttorney.Engines
{
	[Receives(typeof(ToggleFullscreenMessage))]
	class FullScreenEngine : Engine
	{
		private Point _windowedScreenSize;
		public override void Update(double dt)
		{
			if (!SomeMessage<ToggleFullscreenMessage>()) { return; }

			var game = GraceAttorneyGame.Game;
			if (!game.Graphics.IsFullScreen)
				_windowedScreenSize = new Point(game.Window.ClientBounds.Width, game.Window.ClientBounds.Height);

			game.Graphics.ToggleFullScreen();

			if (!game.Graphics.IsFullScreen)
			{
				game.Graphics.PreferredBackBufferWidth = _windowedScreenSize.X;
				game.Graphics.PreferredBackBufferHeight = _windowedScreenSize.Y;
				game.Graphics.ApplyChanges();
			}
		}
	}
}

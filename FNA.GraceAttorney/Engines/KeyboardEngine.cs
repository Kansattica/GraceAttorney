using System;
using System.Collections.Generic;
using System.Text;
using Encompass;
using FNA.GraceAttorney.Messages;
using Microsoft.Xna.Framework.Input;

namespace FNA.GraceAttorney.Engines
{
	[Sends(typeof(ToggleFullscreenMessage))]
	class KeyboardEngine : Engine
	{
		private KeyboardState _keyboardPrev = new KeyboardState();
		public override void Update(double dt)
		{
			KeyboardState keyboardCur = Keyboard.GetState();

			if (KeysPressed(keyboardCur, Keys.Enter))
				SendMessage(new ToggleFullscreenMessage());

			_keyboardPrev = keyboardCur;
		}

		private bool KeysPressed(KeyboardState current, params Keys[] keys)
		{
			foreach (var key in keys)
			{
				if (!(current.IsKeyDown(key) && _keyboardPrev.IsKeyUp(key)))
					return false;
			}
			return true;
		}
	}
}

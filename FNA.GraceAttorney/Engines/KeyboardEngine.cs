using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Encompass;
using FNA.GraceAttorney.Messages;
using Microsoft.Xna.Framework.Input;

namespace FNA.GraceAttorney.Engines
{
	[Sends(typeof(ToggleFullscreenMessage), typeof(ClearBackgroundMessage), typeof(NewBackgroundMessage), typeof(NewCharacterMessage))]
	class KeyboardEngine : Engine
	{
		private KeyboardState _keyboardPrev = new KeyboardState();
		public override void Update(double dt)
		{
			KeyboardState keyboardCur = Keyboard.GetState();

			if (KeysPressed(keyboardCur, Keys.Enter))
				SendMessage(new ToggleFullscreenMessage());

			if (KeysPressed(keyboardCur, Keys.C))
				SendMessage(new ClearBackgroundMessage());

			if (KeysPressed(keyboardCur, Keys.B))
				SendMessage(new NewBackgroundMessage(Path.Combine("Case1", "background")));

			if (KeysPressed(keyboardCur, Keys.Up))
				SendMessage(new NewCharacterMessage(assetName: Path.Combine("Case1", "birdcall"), enterFrom: EntranceDirection.Top));

			if (KeysPressed(keyboardCur, Keys.Down))
				SendMessage(new NewCharacterMessage(assetName: Path.Combine("Case1", "birdcall"), enterFrom: EntranceDirection.Bottom));

			if (KeysPressed(keyboardCur, Keys.Left))
				SendMessage(new NewCharacterMessage(assetName: Path.Combine("Case1", "birdcall"), enterFrom: EntranceDirection.Left));

			if (KeysPressed(keyboardCur, Keys.Right))
				SendMessage(new NewCharacterMessage(assetName: Path.Combine("Case1", "birdcall"), enterFrom: EntranceDirection.Right));

			if (KeysPressed(keyboardCur, Keys.F))
				SendMessage(new NewCharacterMessage(assetName: Path.Combine("Case1", "birdcall"), enterFrom: EntranceDirection.FadeIn));

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

using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Encompass;
using GraceAttorney.Components;
using GraceAttorney.Messages;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace GraceAttorney.Engines
{
	[Sends(typeof(ToggleFullscreenMessage), typeof(ClearBackgroundMessage), typeof(NewBackgroundMessage),
		typeof(CharacterEnterMessage), typeof(CharacterExitByPositionMessage), typeof(CharacterExitByNameMessage), 
		typeof(ClearAllCharactersMessage), typeof(NewDialogueMessage))]
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
				SendMessage(new NewBackgroundMessage("court"));

			if (KeysPressed(keyboardCur, Keys.Up))
				SendMessage(new CharacterEnterMessage(characterName: "Bird Call", pose: "standing", enterFrom: EnterExitDirection.Top));

			if (KeysPressed(keyboardCur, Keys.Down))
				SendMessage(new CharacterEnterMessage(characterName: "Bird Call", pose: "standing", enterFrom: EnterExitDirection.Bottom));

			if (KeysPressed(keyboardCur, Keys.Left))
				SendMessage(new CharacterEnterMessage(characterName: "Bird Call", pose: "standing", enterFrom: EnterExitDirection.Left));

			if (KeysPressed(keyboardCur, Keys.Right))
				SendMessage(new CharacterEnterMessage(characterName: "Bird Call", pose: "standing", enterFrom: EnterExitDirection.Right));

			if (KeysPressed(keyboardCur, Keys.F))
				SendMessage(new CharacterEnterMessage(characterName: "Bird Call", pose: "standing", enterFrom: EnterExitDirection.Fade));

			if (KeysPressed(keyboardCur, Keys.O))
				SendMessage(new CharacterEnterMessage(characterName: "Bird Call", pose: "standing", enterFrom: EnterExitDirection.NoAnimation));

			if (KeysPressed(keyboardCur, Keys.W))
				SendMessage(new CharacterExitByPositionMessage(DrawLocation.Center, EnterExitDirection.Top));

			if (KeysPressed(keyboardCur, Keys.S))
				SendMessage(new CharacterExitByPositionMessage(DrawLocation.Center, EnterExitDirection.Bottom));

			if (KeysPressed(keyboardCur, Keys.A))
				SendMessage(new CharacterExitByPositionMessage(DrawLocation.Center, EnterExitDirection.Left));

			if (KeysPressed(keyboardCur, Keys.D))
				SendMessage(new CharacterExitByPositionMessage(DrawLocation.Center, EnterExitDirection.Right));

			if (KeysPressed(keyboardCur, Keys.X))
				SendMessage(new CharacterExitByPositionMessage(DrawLocation.Center, EnterExitDirection.Fade));

			if (KeysPressed(keyboardCur, Keys.P))
				SendMessage(new CharacterExitByPositionMessage(DrawLocation.Center, EnterExitDirection.NoAnimation));

			if (KeysPressed(keyboardCur, Keys.Y))
				SendMessage(new ClearAllCharactersMessage());

			if (KeysPressed(keyboardCur, Keys.Z))
				SendMessage(new CharacterExitByNameMessage("Bird Call", EnterExitDirection.Top));

			if (KeysPressed(keyboardCur, Keys.T))
				SendMessage(new NewDialogueMessage(new DialogueComponent
				(
					dialogue: "I'm saying something else now! Suck it, nerds. I'm so gay all the time.",
					display: true,
					justification: JustifyText.Left,
					nameTagLocation: NameTagLocation.Left,
					speaker: "Still Grace",
					textColor: Color.White
				), 30));

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

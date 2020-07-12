using System;
using System.Collections.Generic;
using System.Text;
using Encompass;
using GraceAttorney.Components;

namespace GraceAttorney.Messages
{
	readonly struct CharacterExitMessage : IMessage
	{
		public readonly Entity Character;
		public readonly EnterExitDirection ExitTo;
		public CharacterExitMessage(Entity character, EnterExitDirection exitTo = EnterExitDirection.Fade)
		{
			Character = character;
			ExitTo = exitTo;
		}
	}
}

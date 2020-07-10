using System;
using System.Collections.Generic;
using System.Text;
using Encompass;
using GraceAttorney.Components;

namespace GraceAttorney.Messages
{
	readonly struct NewDialogueMessage : IMessage
	{
		public readonly DialogueComponent Dialogue;

		public NewDialogueMessage(DialogueComponent dialogue)
		{
			Dialogue = dialogue;
		}
	}
}

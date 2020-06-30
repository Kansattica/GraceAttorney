using System;
using System.Collections.Generic;
using System.Text;
using Encompass;
using FNA.GraceAttorney.Components;

namespace FNA.GraceAttorney.Messages
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

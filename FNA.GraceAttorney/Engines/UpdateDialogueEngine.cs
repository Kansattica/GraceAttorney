using System;
using System.Collections.Generic;
using System.Text;
using Encompass;
using FNA.GraceAttorney.Components;
using FNA.GraceAttorney.Messages;

namespace FNA.GraceAttorney.Engines
{
	[Receives(typeof(NewDialogueMessage))]
	[Reads(typeof(DialogueComponent))]
	[Writes(typeof(DialogueComponent), 0)]
	class UpdateDialogueEngine : Engine
	{
		public override void Update(double dt)
		{
			if (!SomeMessage<NewDialogueMessage>()) { return; }

			var entity = ReadEntity<DialogueComponent>();

			SetComponent(entity, ReadMessage<NewDialogueMessage>().Dialogue);
		}
	}
}

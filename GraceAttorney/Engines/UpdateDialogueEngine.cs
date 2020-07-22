using System;
using System.Collections.Generic;
using System.Text;
using Encompass;
using GraceAttorney.Components;
using GraceAttorney.Messages;

namespace GraceAttorney.Engines
{

	[DefaultWritePriority(0)]
	[Receives(typeof(NewDialogueMessage))]
	[Reads(typeof(DialogueComponent))]
	[Writes(typeof(DialogueComponent), typeof(AnimatedTextComponent))]
	class UpdateDialogueEngine : Engine
	{
		public override void Update(double dt)
		{
			if (!SomeMessage<NewDialogueMessage>()) { return; }

			ref readonly var entity = ref ReadEntity<DialogueComponent>();

			ref readonly var message = ref ReadMessage<NewDialogueMessage>();

			SetComponent(entity, message.Dialogue);
			SetComponent(entity, new AnimatedTextComponent(message.CharactersPerSecond, charactersVisible: 0));
		}
	}
}

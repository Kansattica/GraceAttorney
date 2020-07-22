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
	class UpdateDialogueEngine : Spawner<NewDialogueMessage>
	{
		protected override void Spawn(in NewDialogueMessage message)
		{
			ref readonly var entity = ref ReadEntity<DialogueComponent>();

			SetComponent(entity, message.Dialogue);
			SetComponent(entity, new AnimatedTextComponent(message.CharactersPerSecond, charactersVisible: 0));
		}
	}
}

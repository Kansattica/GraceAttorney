using System;
using System.Collections.Generic;
using System.Text;
using Encompass;
using GraceAttorney.Components;

namespace GraceAttorney.Engines
{
	[DefaultWritePriority(1)]
	[Reads(typeof(AnimatedTextComponent), typeof(DialogueComponent))]
	[Writes(typeof(AnimatedTextComponent))]
	class TextAnimationEngine : Engine
	{
		public override void Update(double dt)
		{
			foreach (ref readonly var entity in ReadEntities<AnimatedTextComponent>())
			{
				ref readonly var animatingText = ref GetComponent<AnimatedTextComponent>(entity);
				double charactersVisible = animatingText.CharactersVisible + (animatingText.CharactersPerSecond * dt);

				if (HasComponent<DialogueComponent>(entity))
				{
					ref readonly var dialogue = ref GetComponent<DialogueComponent>(entity).Dialogue;

					// plus double the line count because the string can have hyphens and newlines and such added when printing
					if (dialogue != null && charactersVisible >= dialogue.Length + (Constants.ExpectedLineCount * 2)) 
					{
						RemoveComponent<AnimatedTextComponent>(entity);
						continue;
					}
				}

				SetComponent(entity, new AnimatedTextComponent(
						charactersPerSecond: animatingText.CharactersPerSecond,
						charactersVisible: charactersVisible
					));
			}
		}
	}
}

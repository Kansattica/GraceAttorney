using System;
using System.Collections.Generic;
using System.Text;
using Encompass;
using FNA.GraceAttorney.Components;

namespace FNA.GraceAttorney.Engines
{
	[DefaultWritePriority(1)]
	[Reads(typeof(AnimatedTextComponent), typeof(DialogueComponent))]
	[Writes(typeof(AnimatedTextComponent), typeof(DialogueComponent))]
	class TextAnimationEngine : Engine
	{
		public override void Update(double dt)
		{
			foreach ((var animatingText, var entity) in ReadComponentsIncludingEntity<AnimatedTextComponent>())
			{
				double charactersVisible = animatingText.CharactersVisible + (animatingText.CharactersPerSecond * dt);

				if (HasComponent<DialogueComponent>(entity))
				{
					var dialogue = GetComponent<DialogueComponent>(entity).Dialogue;

					// +6 because the string can have hyphens and such added when printing
					if (dialogue != null && charactersVisible >= dialogue.Length + 6) 
					{
						RemoveComponent<AnimatedTextComponent>(entity);
						continue;
					}
				}

				SetComponent(entity, 
					new AnimatedTextComponent {
						CharactersPerSecond = animatingText.CharactersPerSecond,
						CharactersVisible = charactersVisible
					});


			}
		}
	}
}

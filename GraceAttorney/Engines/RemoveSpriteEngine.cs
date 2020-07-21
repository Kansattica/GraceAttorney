using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Encompass;
using GraceAttorney.Components;
using GraceAttorney.Messages;

namespace GraceAttorney.Engines
{
	[DefaultWritePriority(4)]
	[Receives(typeof(RemoveSpriteMessage))]
	[Reads(typeof(AnimatedSpriteComponent))]
	[Writes(typeof(SpriteComponent), typeof(AnimatedSpriteComponent))]
	class RemoveSpriteEngine : Engine
	{
		public override void Update(double dt)
		{
			foreach (ref readonly var message in ReadMessages<RemoveSpriteMessage>())
			{
				ref readonly var character = ref message.Sprite;

				RemoveComponent<SpriteComponent>(character);

				if (HasComponent<AnimatedSpriteComponent>(character))
					RemoveComponent<AnimatedSpriteComponent>(character);
			}
		}
	}
}

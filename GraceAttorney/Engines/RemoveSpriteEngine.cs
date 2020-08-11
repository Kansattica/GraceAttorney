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
	[Writes(typeof(SpriteComponent), typeof(AnimatedSpriteComponent), typeof(CharacterComponent), typeof(BackgroundComponent))]
	class RemoveSpriteEngine : Spawner<RemoveSpriteMessage>
	{
		protected override void Spawn(in RemoveSpriteMessage message)
		{
			ref readonly var character = ref message.Sprite;

			RemoveComponent<SpriteComponent>(character);
			RemoveComponent<AnimatedSpriteComponent>(character);
			RemoveComponent<CharacterComponent>(character);
			RemoveComponent<BackgroundComponent>(character);
		}
	}
}

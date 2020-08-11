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
	[Receives(typeof(CharacterExitByPositionMessage))]
	[Reads(typeof(SpriteComponent), typeof(CharacterComponent))]
	[Sends(typeof(CharacterExitMessage))]
	class CharacterExitByPositionEngine : Spawner<CharacterExitByPositionMessage>
	{
		protected override void Spawn(in CharacterExitByPositionMessage message)
		{
			// this is slow, but remember that there's only, like, three of these on screen at most.
			// we can even probably stop after we find one if it becomes a problem.
			foreach (ref readonly var entity in ReadEntities<CharacterComponent>())
			{
				ref readonly var sprite = ref GetComponent<SpriteComponent>(entity);
				var positionToLeave = Helpers.CalculatePosition(message.Location, sprite.FrameWidth, sprite.FrameHeight);
				if (sprite.Position == positionToLeave)
				{
					SendMessage(new CharacterExitMessage(entity, message.Direction));
				}
			}
		}
	}
}

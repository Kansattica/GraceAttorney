using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Encompass;
using GraceAttorney.Components;
using GraceAttorney.Messages;
using Microsoft.Xna.Framework;

namespace GraceAttorney.Engines
{
	[Receives(typeof(CharacterExitMessage))]
	[Reads(typeof(SpriteComponent), typeof(PositionOverrideComponent))]
	[Sends(typeof(StartMotionMessage), typeof(RemoveSpriteMessage), typeof(StartFadeMessage))]
	class CharacterExitEngine : Spawner<CharacterExitMessage>
	{
		protected override void Spawn(in CharacterExitMessage message)
		{
			if (message.ExitTo == EnterExitDirection.NoAnimation)
				SendMessage(new RemoveSpriteMessage(message.Character));
			else if (message.ExitTo == EnterExitDirection.Fade)
				SendMessage(new StartFadeMessage(message.Character, FadeDirection.FadeOut));
			else
			{
				// it's probably not good that there's two places to check for correct position info
				// probably take the position stuff off SpriteComponent in the future 
				ref readonly var sprite = ref GetComponent<SpriteComponent>(message.Character);

				var currentPosition = CurrentPosition(sprite, message.Character);
				SendMessage(new StartMotionMessage(message.Character,
					Helpers.CalculateOffscreenPosition(message.ExitTo, currentPosition,
						sprite.FrameWidth, sprite.FrameHeight), currentPosition,
					removeAfterAnimating: true));
			}
		}

		private Vector2 CurrentPosition(in SpriteComponent spriteComponent, Entity character)
		{
			if (HasComponent<PositionOverrideComponent>(character))
			{
				return GetComponent<PositionOverrideComponent>(character).Position;
			}

			return spriteComponent.Position;
		}
	}
}

using Encompass;
using GraceAttorney.Components;
using GraceAttorney.Messages;
using Microsoft.Xna.Framework;

namespace GraceAttorney.Engines
{
	[DefaultWritePriority(1)]
	[Receives(typeof(NewSpriteMessage))]
	[Writes(typeof(SpriteComponent), typeof(AnimatedSpriteComponent))]
	[Sends(typeof(StartMotionMessage), typeof(StartFadeMessage))]
	class SetUpSpriteEngine : Spawner<NewSpriteMessage>
	{
		protected override void Spawn(in NewSpriteMessage newSprite)
		{
			var targetPosition = Helpers.CalculatePosition(newSprite.Position, newSprite.Asset.FrameWidth, newSprite.Asset.FrameHeight);
			SetComponent(newSprite.Entity, new SpriteComponent
			(
				position: targetPosition,
				frameWidth: newSprite.Asset.FrameWidth,
				frameHeight: newSprite.Asset.FrameHeight,
				texture: newSprite.Asset.Sprite,
				layer: (int)newSprite.Layer
			));


			if (newSprite.Asset.Frames > 1)
				SetComponent(newSprite.Entity, new AnimatedSpriteComponent(0, newSprite.Asset.Frames));

			if (newSprite.EnterFrom == EnterExitDirection.Fade)
				SendMessage(new StartFadeMessage(newSprite.Entity, FadeDirection.FadeIn));
			else if (newSprite.EnterFrom != EnterExitDirection.NoAnimation)
				SendMessage(new StartMotionMessage(newSprite.Entity, targetPosition,
					Helpers.CalculateOffscreenPosition(newSprite.EnterFrom, targetPosition,
					spriteWidth: newSprite.Asset.FrameWidth,
					spriteHeight: newSprite.Asset.FrameHeight),
					removeAfterAnimating: false));
		}
	}
}

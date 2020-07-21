using Encompass;
using GraceAttorney.Components;
using GraceAttorney.Messages;
using Microsoft.Xna.Framework;

namespace GraceAttorney.Engines
{
	[DefaultWritePriority(1)]
	[Receives(typeof(NewSpriteMessage))]
	[Writes(typeof(SpriteComponent), typeof(AnimatedSpriteComponent))]
	[Sends(typeof(StartMotionMessage))]
	class SetUpSpriteEngine : Engine
	{
		public override void Update(double dt)
		{
			foreach (ref readonly var newSprite in ReadMessages<NewSpriteMessage>())
			{
				SetComponent(newSprite.Entity, new SpriteComponent
				{
					Layer = (int)newSprite.Layer,
					Position = newSprite.Position,
					FrameWidth = newSprite.Asset.FrameWidth,
					FrameHeight = newSprite.Asset.FrameHeight,
					Texture = newSprite.Asset.Sprite,
				}); 

				if (newSprite.Asset.Frames > 1)
					SetComponent(newSprite.Entity, new AnimatedSpriteComponent(0, newSprite.Asset.Frames));

				if (newSprite.EnterFrom != EnterExitDirection.NoAnimation)
				{
					SendMessage(new StartMotionMessage(newSprite.Entity, newSprite.EnterFrom, MotionDirection.In));
				}
			}
		}
	}
}

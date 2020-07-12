using System;
using System.Collections.Generic;
using System.Text;
using Encompass;
using GraceAttorney.Components;
using GraceAttorney.Messages;
using Microsoft.Xna.Framework;

namespace GraceAttorney.Engines
{
	[DefaultWritePriority(0)]
	[Receives(typeof(StartMotionMessage))]
	[Writes(typeof(MovingSpriteComponent), typeof(SpriteOffsetComponent), typeof(OpacityComponent))]
	class StartMotionEngine : Engine
	{
		public override void Update(double dt)
		{
			foreach (ref readonly var message in ReadMessages<StartMotionMessage>())
			{
				ref readonly var entity = ref message.Entity;

				if (message.Direction == EnterExitDirection.Fade)
				{
					if (message.MotionDirection == MotionDirection.In)
						SetComponent(entity, new OpacityComponent(direction: FadeDirection.FadeIn, opacity: 0, fadeRate: 1.0f));
					else
						SetComponent(entity, new OpacityComponent(direction: FadeDirection.FadeOut, opacity: 1.0f, fadeRate: 1.0f));
				}
				else
				{
					SetComponent(entity, new MovingSpriteComponent { Direction = message.MotionDirection, Velocity = .5f });
					SetComponent(entity, new SpriteOffsetComponent
					{
						// basically, the motion engine always moves the character in the direction they're already going
						// (I should probably change that and put the direction vector on the moving sprite component)
						// so if they're leaving, just nudge 'em a bit in the direction so they can get going.
						PositionPercentageOffset = GetDirectionVector(message.Direction) * 
									(message.MotionDirection == MotionDirection.In ? 1.0f : .01f)
					});
				}
			}

		}

		private static Vector2 GetDirectionVector(EnterExitDirection direction)
		{
			switch (direction)
			{
				case EnterExitDirection.Top:
					return -Vector2.UnitY;
				case EnterExitDirection.Bottom:
					return Vector2.UnitY;
				case EnterExitDirection.Right:
					return Vector2.UnitX;
				case EnterExitDirection.Left:
					return -Vector2.UnitX;
			}
			throw new ArgumentException("What, bud, did you invent a new direction or something");
		}
	}
}

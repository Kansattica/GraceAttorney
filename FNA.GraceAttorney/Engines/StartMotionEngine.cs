using System;
using System.Collections.Generic;
using System.Text;
using Encompass;
using FNA.GraceAttorney.Components;
using FNA.GraceAttorney.Messages;
using Microsoft.Xna.Framework;

namespace FNA.GraceAttorney.Engines
{
	[DefaultWritePriority(0)]
	[Receives(typeof(StartMotionMessage))]
	[Writes(typeof(MovingSpriteComponent), typeof(SpriteOffsetComponent), typeof(OpacityComponent))]
	class StartMotionEngine : Engine
	{
		public override void Update(double dt)
		{
			foreach (var message in ReadMessages<StartMotionMessage>())
			{
				var entity = message.Entity;

				if (message.EnterFrom == EntranceDirection.FadeIn)
				{
					SetComponent(entity, new OpacityComponent(direction: FadeDirection.FadeIn, opacity: 0, fadeRate: 1.0f));
				}
				else
				{
					SetComponent(entity, new MovingSpriteComponent { Direction = MotionDirection.In, Velocity = .5f });
					SetComponent(entity, new SpriteOffsetComponent { PositionPercentageOffset = GetDirectionVector(message.EnterFrom) });
				}
			}

		}

		private static Vector2 GetDirectionVector(EntranceDirection direction)
		{
			switch (direction)
			{
				case EntranceDirection.Top:
					return -Vector2.UnitY;
				case EntranceDirection.Bottom:
					return Vector2.UnitY;
				case EntranceDirection.Right:
					return Vector2.UnitX;
				case EntranceDirection.Left:
					return -Vector2.UnitX;
			}
			throw new ArgumentException("What, bud, did you invent a new direction or something");
		}
	}
}

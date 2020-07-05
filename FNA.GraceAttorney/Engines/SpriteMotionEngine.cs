using System;
using System.Collections.Generic;
using System.Text;
using Encompass;
using FNA.GraceAttorney.Components;
using Microsoft.Xna.Framework;

namespace FNA.GraceAttorney.Engines
{
	[DefaultWritePriority(2)]
	[Reads(typeof(MovingSpriteComponent), typeof(SpriteOffsetComponent))]
	[Writes(typeof(SpriteOffsetComponent), typeof(MovingSpriteComponent))]
	class SpriteMotionEngine : Engine
	{
		public override void Update(double dt)
		{
			foreach (ref readonly var entity in ReadEntities<MovingSpriteComponent>())
			{
				ref readonly var movingSprite = ref GetComponent<MovingSpriteComponent>(entity);
				// Offset is the percentage of the distance the sprite is away from where it will eventually end up
				// This engine's job is to move each one closer to zero based on its velocity

				ref readonly var offset = ref GetComponent<SpriteOffsetComponent>(entity);
				var newOffset = offset.PositionPercentageOffset +
								   VelocityVector(movingSprite.Velocity, dt, offset.PositionPercentageOffset, movingSprite.Direction);

				if (!ShouldStop(newOffset, movingSprite.Direction))
				{
					SetComponent(entity,
						new SpriteOffsetComponent { PositionPercentageOffset = newOffset });
				}
				else
				{
					RemoveComponent<MovingSpriteComponent>(entity);
					RemoveComponent<SpriteOffsetComponent>(entity);
				}
			}
		}

		private static Vector2 VelocityVector(float velocity, double dt, Vector2 directionVector, MotionDirection direction)
		{
			// basically, we want to return a vector in the opposite direction of the current direction, with a magnitude of 
			// velocity.
			// so if the direction is (1, 0) (representing a sprite all the way off the screen to the right)
			// we want to return a vector that's (-velocity, 0).
			// and if the direction is (0, -.4) (representing a sprite 40 percent of the way below where it should be)
			// then we want to return (0, velocity)
			// (well, velocity times dt so we're frame independent)
			directionVector.Normalize();

			// basically, if the sprite is leaving, we want the offset to get bigger, not smaller
			if (direction == MotionDirection.In)
				directionVector = -directionVector;

			return (float)(dt * velocity) * directionVector;
		}

		private static bool ShouldStop(in Vector2 offsetVector, MotionDirection direction)
        {
			if (direction == MotionDirection.In && offsetVector.Length() <= .006f)
				return true;
			if (direction == MotionDirection.Out && offsetVector.Length() >= 1)
				return true;
			return false;
		}
	}
}

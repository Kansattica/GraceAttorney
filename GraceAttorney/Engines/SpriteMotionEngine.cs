using System;
using System.Collections.Generic;
using System.Text;
using Encompass;
using GraceAttorney.Components;
using GraceAttorney.Messages;
using Microsoft.Xna.Framework;

namespace GraceAttorney.Engines
{
	[DefaultWritePriority(2)]
	[Reads(typeof(MovingSpriteComponent), typeof(PositionOverrideComponent))]
	[Writes(typeof(MovingSpriteComponent), typeof(PositionOverrideComponent))]
	[Sends(typeof(RemoveSpriteMessage))]
	class SpriteMotionEngine : Engine
	{
		public override void Update(double dt)
		{
			foreach (ref readonly var entity in ReadEntities<MovingSpriteComponent>())
			{
				ref readonly var movingSprite = ref GetComponent<MovingSpriteComponent>(entity);

				ref readonly var position = ref GetComponent<PositionOverrideComponent>(entity);

				var newPosition = position.Position + 
					ComputeMotionStep(position.Position, movingSprite.TargetPosition, (float)(movingSprite.Velocity * dt));

				if (ShouldStop(newPosition, movingSprite.TargetPosition))
				{
					RemoveComponent<MovingSpriteComponent>(entity);
					RemoveComponent<PositionOverrideComponent>(entity);
					if (movingSprite.RemoveAfterAnimating)
						SendMessage(new RemoveSpriteMessage(entity));
				}
				else
				{
					SetComponent(entity,
						new PositionOverrideComponent(newPosition));
				}
			}
		}

		private static bool ShouldStop(in Vector2 current, in Vector2 target)
		{
			return Vector2.Distance(current, target) <= 10;
		}

		private static Vector2 ComputeMotionStep(in Vector2 currentPosition, in Vector2 destinationPosition, float velocity)
		{
			var direction = destinationPosition - currentPosition;
			direction.Normalize();
			return direction * velocity;
		}

	}
}

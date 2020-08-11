using System;
using System.Collections.Generic;
using System.Text;
using Encompass;
using Microsoft.Xna.Framework;

namespace GraceAttorney.Components
{
	readonly struct MovingSpriteComponent : IComponent
	{
		public readonly Vector2 TargetPosition;
		public readonly float Velocity;
		public readonly bool RemoveAfterAnimating;

		public MovingSpriteComponent(Vector2 targetPosition, float velocity, bool removeAfterAnimating)
		{
			TargetPosition = targetPosition;
			Velocity = velocity;
			RemoveAfterAnimating = removeAfterAnimating;
		}
	}
}

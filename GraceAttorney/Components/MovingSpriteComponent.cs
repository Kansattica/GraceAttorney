using System;
using System.Collections.Generic;
using System.Text;
using Encompass;

namespace GraceAttorney.Components
{
	enum MotionDirection { In, Out }
	readonly struct MovingSpriteComponent : IComponent
	{
		public readonly MotionDirection Direction;
		public readonly float Velocity;

		public MovingSpriteComponent(MotionDirection direction, float velocity)
		{
			Direction = direction;
			Velocity = velocity;
		}
	}
}

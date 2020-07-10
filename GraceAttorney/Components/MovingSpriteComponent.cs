using System;
using System.Collections.Generic;
using System.Text;
using Encompass;
using Microsoft.Xna.Framework;

namespace GraceAttorney.Components
{
	enum MotionDirection { In, Out }
	struct MovingSpriteComponent : IComponent
	{
		public MotionDirection Direction;
		public float Velocity;
	}
}

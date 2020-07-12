using System;
using System.Collections.Generic;
using System.Text;
using Encompass;
using GraceAttorney.Components;

namespace GraceAttorney.Messages
{
	readonly struct StartMotionMessage : IMessage
	{
		public readonly Entity Entity;
		public readonly EnterExitDirection Direction;
		public readonly MotionDirection MotionDirection;
		public StartMotionMessage(in Entity entity, EnterExitDirection direction, MotionDirection motionDirection)
		{
			Entity = entity;
			Direction = direction;
			MotionDirection = motionDirection;
		}
	}
}

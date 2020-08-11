using System;
using System.Collections.Generic;
using System.Text;
using Encompass;
using GraceAttorney.Components;
using Microsoft.Xna.Framework;

namespace GraceAttorney.Messages
{
	readonly struct StartMotionMessage : IMessage
	{
		public readonly Entity Entity;
		public readonly Vector2 TargetPosition;
		public readonly Vector2 StartPosition;
		public readonly bool RemoveAfterAnimating;

		public StartMotionMessage(Entity entity, Vector2 targetPosition, Vector2 startPosition, bool removeAfterAnimating)
		{
			Entity = entity;
			TargetPosition = targetPosition;
			StartPosition = startPosition;
			RemoveAfterAnimating = removeAfterAnimating;
		}
	}
}

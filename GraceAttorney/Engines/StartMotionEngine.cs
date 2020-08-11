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
	[Writes(typeof(MovingSpriteComponent), typeof(PositionOverrideComponent))]
	class StartMotionEngine : Spawner<StartMotionMessage>
	{
		protected override void Spawn(in StartMotionMessage message)
		{
			ref readonly var entity = ref message.Entity;

			SetComponent(entity, new PositionOverrideComponent(message.StartPosition));
			SetComponent(entity, new MovingSpriteComponent(
				velocity: 500f,
				targetPosition: message.TargetPosition,
				removeAfterAnimating: message.RemoveAfterAnimating));
		}
	}
}

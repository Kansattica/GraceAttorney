using System;
using System.Collections.Generic;
using System.Text;
using Encompass;
using GraceAttorney.Components;
using GraceAttorney.Messages;

namespace GraceAttorney.Engines
{
	[Receives(typeof(ClearBackgroundMessage))]
	[Writes(typeof(OpacityComponent), 1)]
	[Reads(typeof(BackgroundComponent), typeof(OpacityComponent))]
	class ClearBackgroundEngine : Engine
	{
		public override void Update(double dt)
		{
			if (!SomeMessage<ClearBackgroundMessage>()) { return; }

			ref readonly var entity = ref ReadEntity<BackgroundComponent>();

			SetComponent(entity, new OpacityComponent(direction: FadeDirection.FadeOut, opacity: GetOpacity(entity), fadeRate: 1.0f));
		}
		private float GetOpacity(in Entity entity)
		{
			if (HasComponent<OpacityComponent>(entity))
			{
				ref readonly var opacityComponent = ref GetComponent<OpacityComponent>(entity);
				return opacityComponent.Opacity;
			}

			return 1.0f;
		}
	}

}

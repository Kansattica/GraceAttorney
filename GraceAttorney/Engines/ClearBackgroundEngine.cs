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

			ref readonly var currentOpacity = ref GetComponent<OpacityComponent>(entity).Opacity;

			SetComponent(entity, new OpacityComponent(direction: FadeDirection.FadeOut, opacity: currentOpacity, fadeRate: 1.0f));
		}
	}
}
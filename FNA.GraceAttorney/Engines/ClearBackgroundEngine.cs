using System;
using System.Collections.Generic;
using System.Text;
using Encompass;
using FNA.GraceAttorney.Components;
using FNA.GraceAttorney.Messages;

namespace FNA.GraceAttorney.Engines
{
	[Receives(typeof(ClearBackgroundMessage))]
	[Writes(typeof(OpacityComponent), 1)]
	[Reads(typeof(BackgroundComponent))]
	class ClearBackgroundEngine : Engine
	{
		public override void Update(double dt)
		{
			if (!SomeMessage<ClearBackgroundMessage>()) { return; }

			(var _, var entity) = ReadComponentIncludingEntity<BackgroundComponent>();

			SetComponent(entity, new OpacityComponent(direction: FadeDirection.FadeOut, opacity: 255, fadeRate: 1.0f));
		}
	}
}
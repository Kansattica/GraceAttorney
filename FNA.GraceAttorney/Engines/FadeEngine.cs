using System;
using System.Collections.Generic;
using System.Text;
using Encompass;
using FNA.GraceAttorney.Components;

namespace FNA.GraceAttorney.Engines
{
	[Reads(typeof(OpacityComponent))]
	[Writes(typeof(OpacityComponent), 10)]
	class FadeEngine : Engine
	{
		public override void Update(double dt)
		{
			foreach (ref readonly var entity in ReadEntities<OpacityComponent>())
			{
				ref readonly var opacity = ref GetComponent<OpacityComponent>(entity);
				switch (opacity.Direction)
				{
					case FadeDirection.FadeIn:
						SetComponent(entity, UpdateOpacity(opacity.Opacity + (dt * opacity.FadeRate), 1f, opacity.Direction, opacity.FadeRate));
						break;
					case FadeDirection.FadeOut:
						SetComponent(entity, UpdateOpacity(opacity.Opacity - (dt * opacity.FadeRate), 0f, opacity.Direction, opacity.FadeRate));
						break;
				}
			}
		}

		private OpacityComponent UpdateOpacity(double newValue, float target, FadeDirection direction, float fadeRate)
		{
			var newOpacity = (float)Math.Clamp(newValue, 0, 1.0);
			return new OpacityComponent(direction: newOpacity == target ? FadeDirection.None : direction, opacity: newOpacity, fadeRate: fadeRate);
		}
	}
}

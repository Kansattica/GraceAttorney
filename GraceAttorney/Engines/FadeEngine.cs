using System;
using System.Collections.Generic;
using System.Text;
using Encompass;
using GraceAttorney.Components;

namespace GraceAttorney.Engines
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
			var newOpacity = Clamp((float)newValue, 0, 1.0f);
			return new OpacityComponent(direction: newOpacity == target ? FadeDirection.None : direction, opacity: newOpacity, fadeRate: fadeRate);
		}

		private static float Clamp(float value, float low, float high)
		{
			if (value < low) { return low; }
			if (value > high) { return high; }
			return value;
		}
	}
}

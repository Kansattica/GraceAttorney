using System;
using System.Collections.Generic;
using System.Text;
using Encompass;
using FNA.GraceAttorney.Components;

namespace FNA.GraceAttorney.Engines
{
	[Reads(typeof(OpacityComponent))]
	[Writes(typeof(OpacityComponent), 1)]
	class FadeEngine : Engine
	{
		public override void Update(double dt)
		{
			foreach (var pair in ReadComponentsIncludingEntity<OpacityComponent>())
			{
				switch (pair.Item1.Direction)
				{
					case FadeDirection.FadeIn:
						SetComponent(pair.Item2, UpdateOpacity(pair.Item1.Opacity + (dt * pair.Item1.FadeRate), 1f, pair.Item1.Direction, pair.Item1.FadeRate));
						break;
					case FadeDirection.FadeOut:
						SetComponent(pair.Item2, UpdateOpacity(pair.Item1.Opacity - (dt * pair.Item1.FadeRate), 0f, pair.Item1.Direction, pair.Item1.FadeRate));
						break;
				}
			}
		}

		private OpacityComponent UpdateOpacity(double newValue, float target, FadeDirection direction, float fadeRate)
		{
			var newOpacity = Clamp((float)newValue, 0, 1f);
			return new OpacityComponent(direction: newOpacity == target ? FadeDirection.None : direction, opacity: newOpacity, fadeRate: fadeRate);
		}

		private static float Clamp(float value, float min, float max)
		{
			if (value < min) { return min; }
			if (value > max) { return max; }
			return value;
		}
	}
}

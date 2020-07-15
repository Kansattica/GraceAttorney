using System;
using System.Collections.Generic;
using System.Text;
using Encompass;
using GraceAttorney.Components;
using GraceAttorney.Messages;

namespace GraceAttorney.Engines
{
	[Reads(typeof(OpacityComponent))]
	[Writes(typeof(OpacityComponent), 10)]
	[Sends(typeof(RemoveSpriteMessage))]
	class FadeEngine : Engine
	{
		public override void Update(double dt)
		{
			foreach (ref readonly var entity in ReadEntities<OpacityComponent>())
			{
				ref readonly var opacity = ref GetComponent<OpacityComponent>(entity);
				var calculated = NewOpacity(dt, opacity);
				if (calculated.Direction == FadeDirection.None)
				{
					if (opacity.Direction == FadeDirection.FadeOut)
						SendMessage(new RemoveSpriteMessage(entity)); // if they fade all the way out, remove 'em
					else
						RemoveComponent<OpacityComponent>(entity); // if they fade all the way in, stop animating.
				}
				else
				{
					SetComponent(entity, calculated);
				}

			}
		}

		private OpacityComponent NewOpacity(double dt, in OpacityComponent opacity)
		{
			switch (opacity.Direction)
			{
				case FadeDirection.FadeIn:
					return UpdateOpacity(opacity.Opacity + (dt * opacity.FadeRate), 1f, opacity.Direction, opacity.FadeRate);
				case FadeDirection.FadeOut:
					return UpdateOpacity(opacity.Opacity - (dt * opacity.FadeRate), 0f, opacity.Direction, opacity.FadeRate);
				default:
					throw new NotImplementedException("Hm, this shouldn't happen in the fade engine.");
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

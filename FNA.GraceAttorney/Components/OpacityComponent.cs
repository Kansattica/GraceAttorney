using System;
using System.Collections.Generic;
using System.Text;
using Encompass;

namespace FNA.GraceAttorney.Components
{
	enum FadeDirection { FadeIn, FadeOut, None }
	readonly struct OpacityComponent : IComponent
	{
		public readonly float Opacity;
		public readonly float FadeRate;
		public readonly FadeDirection Direction;

		public OpacityComponent(float opacity, float fadeRate, FadeDirection direction)
		{
			Opacity = opacity;
			FadeRate = fadeRate;
			Direction = direction;
		}

	}
}

using System;
using System.Collections.Generic;
using System.Text;
using Encompass;

namespace FNA.GraceAttorney.Components
{
	enum FadeDirection { FadeIn, FadeOut, None }
	readonly struct OpacityComponent : IComponent
	{
		readonly public float Opacity;
		readonly public float FadeRate;
		readonly public FadeDirection Direction;

		public OpacityComponent(float opacity, float fadeRate, FadeDirection direction)
		{
			Opacity = opacity;
			FadeRate = fadeRate;
			Direction = direction;
		}

	}
}

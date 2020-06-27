using System;
using System.Collections.Generic;
using System.Text;
using Encompass;

namespace FNA.GraceAttorney.Components
{
	enum FadeDirection { FadeIn, FadeOut, None }
	struct OpacityComponent : IComponent
	{
		public float Opacity;
		public float FadeRate;
		public FadeDirection Direction;
	}
}

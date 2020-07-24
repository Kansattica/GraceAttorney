using System;
using System.Collections.Generic;
using System.Text;
using Encompass;
using Microsoft.Xna.Framework;

namespace GraceAttorney.Components
{
	readonly struct SpriteOffsetComponent : IComponent
	{
		public readonly Vector2 PositionPercentageOffset;

		public SpriteOffsetComponent(Vector2 positionPercentageOffset)
		{
			PositionPercentageOffset = positionPercentageOffset;
		}
	}
}

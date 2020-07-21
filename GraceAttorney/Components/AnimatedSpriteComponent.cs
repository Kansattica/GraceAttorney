using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Encompass;

namespace GraceAttorney.Components
{
	readonly struct AnimatedSpriteComponent : IComponent
	{
		public readonly double FrameProgress;
		public readonly int FrameCount;
		public AnimatedSpriteComponent(double frameProgress, int frameCount)
		{
			FrameProgress = frameProgress;
			FrameCount = frameCount;
		}
	}
}

using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Encompass;
using GraceAttorney.Components;

namespace GraceAttorney.Engines
{
	[Reads(typeof(AnimatedSpriteComponent))]
	[Writes(typeof(AnimatedSpriteComponent), 0)]
	class SpriteAnimationEngine : Engine
	{
		private const double AnimationFramesPerSecond = 8.333;
		public override void Update(double dt)
		{
			foreach (ref readonly var entity in ReadEntities<AnimatedSpriteComponent>())
			{
				ref readonly var sprite = ref GetComponent<AnimatedSpriteComponent>(entity);

				// should we guarantee that either zero or one frames pass each run?
				var newFrame = sprite.FrameProgress + (AnimationFramesPerSecond * dt);

 				// basically, if the new frame would be a frame that doesn't exist, wrap around to zero again.
				newFrame %= sprite.FrameCount;

				SetComponent(entity, new AnimatedSpriteComponent(newFrame, sprite.FrameCount));
			}
		}
	}
}

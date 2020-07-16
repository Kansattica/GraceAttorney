using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Encompass;
using GraceAttorney.Components;

namespace GraceAttorney.Engines
{
	[Reads(typeof(SpriteComponent))]
	[Writes(typeof(SpriteComponent), 0)]
	class SpriteAnimationEngine : Engine
	{
		private const int AnimationFramesPerSecond = 10;
		public override void Update(double dt)
		{
			foreach (ref readonly var entity in ReadEntities<SpriteComponent>())
			{
				ref readonly var sprite = ref GetComponent<SpriteComponent>(entity);

				if (sprite.Frames.Length == 1) { continue; }

				// should we guarantee that either zero or one frames pass each run?
				var newFrame = sprite.CurrentFrame + (AnimationFramesPerSecond * dt);

				// basically, if the new frame would be a frame that doesn't exist, wrap around to zero again.
				newFrame %= sprite.Frames.Length;

				SetComponent(entity,
				  new SpriteComponent { CurrentFrame = newFrame, Frames = sprite.Frames, Layer = sprite.Layer, Position = sprite.Position });
			}
		}
	}
}

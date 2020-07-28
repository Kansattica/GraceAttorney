using System;
using System.Collections.Generic;
using Encompass;
using GraceAttorney.Components;
using GraceAttorney.DependencyInjection;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GraceAttorney.Renderers
{
	class SpriteRenderer : OrderedRenderer<SpriteComponent>
	{
		private readonly SpriteBatch _spriteBatch;

		public SpriteRenderer(SpriteBatch spriteBatch)
		{
			_spriteBatch = spriteBatch;
		}

		public override void Render(Entity entity, in SpriteComponent drawComponent)
		{
			// look at later:
			// the DestinationRectangle thing might make more sense than the scale factor? I'll have to mess with it.
			// I want to let the sprites be different sizes relative to each other, so the rectangle thing is probably a non-starter.
			_spriteBatch.Draw(drawComponent.Texture,
				Helpers.CalculatePosition(drawComponent.Position, drawComponent.FrameWidth, drawComponent.FrameHeight) + CalculateOffset(entity),
				GetFrameBounds(entity, drawComponent), OpacityColor(entity));
		}

		private Rectangle GetFrameBounds(in Entity entity, in SpriteComponent spriteComponent)
		{
			int xOffset = 0;
			if (HasComponent<AnimatedSpriteComponent>(entity))
			{
				ref readonly var animation = ref GetComponent<AnimatedSpriteComponent>(entity);
				int currentFrame = (int)animation.FrameProgress;
				xOffset = spriteComponent.FrameWidth * currentFrame;
			}
			return new Rectangle(xOffset, 0, spriteComponent.FrameWidth, spriteComponent.FrameHeight);
		}

		private Color OpacityColor(in Entity entity)
		{
			if (HasComponent<OpacityComponent>(entity))
			{
				ref readonly var opacity = ref GetComponent<OpacityComponent>(entity);
				return new Color(1f, 1f, 1f, opacity.Opacity);
			}
			return Color.White;
		}

		private Vector2 CalculateOffset(in Entity entity)
		{
			if (!HasComponent<SpriteOffsetComponent>(entity))
				return Vector2.Zero;

			ref readonly var offsetComponent = ref GetComponent<SpriteOffsetComponent>(entity);
			var offset = offsetComponent.PositionPercentageOffset;
			return new Vector2(Common.Constants.BackgroundWidthInPixels * offset.X, Common.Constants.BackgroundHeightInPixels * offset.Y);
		}
	}
}

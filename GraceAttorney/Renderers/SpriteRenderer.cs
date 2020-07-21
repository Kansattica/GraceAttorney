using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using Encompass;
using GraceAttorney.Components;
using GraceAttorney.DependencyInjection;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GraceAttorney.Renderers
{
	[Reads(typeof(SpriteComponent), typeof(OpacityComponent))]
	class SpriteRenderer : OrderedRenderer<SpriteComponent>
	{
		private const float ShowThisMuch = .90f;
		private const float SideCharacterXOffset = .30f;

		private readonly SpriteBatch _spriteBatch;
		private readonly ScaleFactor _scaleFactor;
		private readonly UpdatedSize _viewport;

		public SpriteRenderer(SpriteBatch spriteBatch, ScaleFactor scaleFactor, UpdatedSize viewport)
		{
			_spriteBatch = spriteBatch;
			_scaleFactor = scaleFactor;
			_viewport = viewport;
		}

		public override void Render(Entity entity, in SpriteComponent drawComponent)
		{

				// look at later:
				// the DestinationRectangle thing might make more sense than the scale factor? I'll have to mess with it.
				// I want to let the sprites be different sizes relative to each other, so the rectangle thing is probably a non-starter.
				_spriteBatch.Draw(drawComponent.Texture,
					CalculatePosition(drawComponent.Position, drawComponent.FrameWidth, drawComponent.FrameHeight) + CalculateOffset(entity),
					GetFrameBounds(entity, drawComponent), OpacityColor(entity),
					0, Vector2.Zero, _scaleFactor.Factor, SpriteEffects.None, 0);
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

		private float CalculateTopOfHeadYPosition(int spriteHeight)
		{ 
			return _viewport.Height - (_scaleFactor.Factor * spriteHeight * ShowThisMuch);
		}

		Vector2 CalculatePosition(DrawLocation location, int spriteFrameWidth, int spriteFrameHeight)
		{
			var originToCenterTheSpriteAlongTheXAxis = (_viewport.Width - (_scaleFactor.Factor * spriteFrameWidth)) / 2;
			return location switch
			{
				DrawLocation.Background => new Vector2(originToCenterTheSpriteAlongTheXAxis, 0),// center the background so that it displays okay even if the window has been maximized to something non-16:9.
				DrawLocation.Center => new Vector2(originToCenterTheSpriteAlongTheXAxis, CalculateTopOfHeadYPosition(spriteFrameHeight)),// this actually has to get calculated in the sprite renderer, otherwise the sprite moves around when you resize the window
				DrawLocation.Left => new Vector2(originToCenterTheSpriteAlongTheXAxis - (SideCharacterXOffset * _viewport.Width),
					CalculateTopOfHeadYPosition(spriteFrameHeight)),
				DrawLocation.Right => new Vector2(originToCenterTheSpriteAlongTheXAxis + (SideCharacterXOffset * _viewport.Width),
					CalculateTopOfHeadYPosition(spriteFrameHeight)),
				_ => throw new NotImplementedException("You fell out of the switch."),
			};
		}

		private Vector2 CalculateOffset(in Entity entity)
		{
			if (!HasComponent<SpriteOffsetComponent>(entity))
				return Vector2.Zero;

			ref readonly var offsetComponent = ref GetComponent<SpriteOffsetComponent>(entity);
			var offset = offsetComponent.PositionPercentageOffset;
			return new Vector2(_viewport.Width * offset.X, _viewport.Height * offset.Y);
		}
	}
}

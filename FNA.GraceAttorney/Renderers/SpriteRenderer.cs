using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using Encompass;
using FNA.GraceAttorney.Components;
using FNA.GraceAttorney.DependencyInjection;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace FNA.GraceAttorney.Renderers
{
	[Reads(typeof(SpriteComponent), typeof(OpacityComponent))]
	class SpriteRenderer : OrderedRenderer<SpriteComponent>
	{
		private const float ShowThisMuch = .90f;
		private const float SideCharacterXOffset = .30f;

		private readonly SpriteBatch _spriteBatch;
		private readonly ScaleFactor _scaleFactor;
		private UpdatedSize _viewport;

		public SpriteRenderer(SpriteBatch spriteBatch, ScaleFactor scaleFactor, UpdatedSize viewport)
		{
			_spriteBatch = spriteBatch;
			_scaleFactor = scaleFactor;
			_viewport = viewport;
		}

		public override void Render(Entity entity, in SpriteComponent drawComponent)
		{
			if (drawComponent.Sprite != null)
			{
				// look at later:
				// the DestinationRectangle thing might make more sense than the scale factor? I'll have to mess with it.
				_spriteBatch.Draw(drawComponent.Sprite,
					CalculatePosition(drawComponent.Position, drawComponent.Sprite) + CalculateOffset(entity),
					null,
					HasComponent<OpacityComponent>(entity) ? OpacityColor(entity) : Color.White,
					0, Vector2.Zero, _scaleFactor.Factor, SpriteEffects.None, 0);
			}
		}

		private Color OpacityColor(in Entity entity)
		{
			ref readonly var opacity = ref GetComponent<OpacityComponent>(entity);
			return new Color(1f, 1f, 1f, opacity.Opacity);
		}

		private float CalculateTopOfHeadYPosition(int spriteHeight)
		{ 
			return _viewport.Height - (_scaleFactor.Factor * spriteHeight * ShowThisMuch);
		}
		Vector2 CalculatePosition(DrawLocation location, Texture2D sprite)
		{
			var originToCenterTheSpriteAlongTheXAxis = (_viewport.Width - (_scaleFactor.Factor * sprite.Width)) / 2;
			switch (location)
			{
				case DrawLocation.Background:
					// center the background so that it displays okay even if the window has been maximized to something non-16:9.
					return new Vector2(originToCenterTheSpriteAlongTheXAxis, 0);
				case DrawLocation.Centered:
					// this actually has to get calculated in the sprite renderer, otherwise the sprite moves around when you resize the window
					return new Vector2(originToCenterTheSpriteAlongTheXAxis, CalculateTopOfHeadYPosition(sprite.Height));
				case DrawLocation.Left:
					return new Vector2(originToCenterTheSpriteAlongTheXAxis + (SideCharacterXOffset * _viewport.Width)
						, CalculateTopOfHeadYPosition(sprite.Height));
				case DrawLocation.Right:
					return new Vector2(originToCenterTheSpriteAlongTheXAxis - (SideCharacterXOffset * _viewport.Width)
						, CalculateTopOfHeadYPosition(sprite.Height));

			}
			throw new NotImplementedException("You fell out of the switch.");
		}

		private Vector2 CalculateOffset(in Entity entity)
		{
			if (!HasComponent<SpriteOffsetComponent>(entity))
				return Vector2.Zero;
			var offset = GetComponent<SpriteOffsetComponent>(entity).PositionPercentageOffset;
			return new Vector2(_viewport.Width * offset.X, _viewport.Height * offset.Y);
		}
	}
}

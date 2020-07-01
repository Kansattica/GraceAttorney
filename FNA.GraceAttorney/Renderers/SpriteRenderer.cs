using System;
using System.Collections.Generic;
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

		private readonly SpriteBatch _spriteBatch;
		private readonly ScaleFactor _scaleFactor;
		private UpdatedSize _viewport;

		public SpriteRenderer(SpriteBatch spriteBatch, ScaleFactor scaleFactor, UpdatedSize viewport)
		{
			_spriteBatch = spriteBatch;
			_scaleFactor = scaleFactor;
			_viewport = viewport;
		}

		public override void Render(Entity entity, SpriteComponent drawComponent)
		{
			if (drawComponent.Sprite != null)
			{
				// look at later:
				// the DestinationRectangle thing might make more sense than the scale factor? I'll have to mess with it.
				_spriteBatch.Draw(drawComponent.Sprite, CalculatePosition(drawComponent.Position, drawComponent.Sprite),
					null,
					HasComponent<OpacityComponent>(entity) ? new Color(1f, 1f, 1f, GetComponent<OpacityComponent>(entity).Opacity) : Color.White,
					0, Vector2.Zero, _scaleFactor.Factor, SpriteEffects.None, 0);
			}
		}


		Vector2 CalculatePosition(DrawLocation location, Texture2D sprite)
		{
			switch (location)
			{
				case DrawLocation.Background:
					return Vector2.Zero;
				case DrawLocation.Centered:
					// this actually has to get calculated in the sprite renderer, otherwise the sprite moves around when you resize the window
					float topOfHead = _viewport.Height - (_scaleFactor.Factor * sprite.Height * ShowThisMuch);
					float spriteOrigin = (_viewport.Width - (_scaleFactor.Factor * sprite.Width)) / 2;
					return new Vector2(spriteOrigin, topOfHead);
			}
			throw new NotImplementedException("You fell out of the switch.");
		}
	}
}

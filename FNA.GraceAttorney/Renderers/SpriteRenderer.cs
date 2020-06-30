using System;
using System.Collections.Generic;
using System.Text;
using Encompass;
using FNA.GraceAttorney.Components;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace FNA.GraceAttorney.Renderers
{
	[Reads(typeof(SpriteComponent), typeof(OpacityComponent))]
	class SpriteRenderer : OrderedRenderer<SpriteComponent>
	{
		public override void Render(Entity entity, SpriteComponent drawComponent)
		{
			if (drawComponent.Sprite != null)
			{
				GraceAttorneyGame.Game.SpriteBatch.Draw(drawComponent.Sprite, CalculatePosition(drawComponent.Position, drawComponent.Sprite),
					null,
					HasComponent<OpacityComponent>(entity) ? new Color(1f, 1f, 1f, GetComponent<OpacityComponent>(entity).Opacity) : Color.White,
					0, Vector2.Zero, GraceAttorneyGame.Game.ScaleFactor, SpriteEffects.None, 0);
			}
		}

		private const float ShowThisMuch = .90f;
		Vector2 CalculatePosition(DrawLocation location, Texture2D sprite)
		{
			switch (location)
			{
				case DrawLocation.Background:
					return Vector2.Zero;
				case DrawLocation.Centered:
					// this actually has to get calculated in the sprite renderer, otherwise the sprite moves around when you resize the window
					float topOfHead = GraceAttorneyGame.Game.GraphicsDevice.Viewport.Height - (GraceAttorneyGame.Game.ScaleFactor * sprite.Height * ShowThisMuch);
					float spriteOrigin = (GraceAttorneyGame.Game.GraphicsDevice.Viewport.Width - (GraceAttorneyGame.Game.ScaleFactor * sprite.Width)) / 2;
					return new Vector2(spriteOrigin, topOfHead);
			}
			throw new NotImplementedException("You fell out of the switch.");
		}
	}
}

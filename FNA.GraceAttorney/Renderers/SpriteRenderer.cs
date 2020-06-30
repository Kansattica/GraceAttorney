using System;
using System.Collections.Generic;
using System.Text;
using Encompass;
using FNA.GraceAttorney.Components;
using Microsoft.Xna.Framework;

namespace FNA.GraceAttorney.Renderers
{
	[Reads(typeof(SpriteComponent), typeof(OpacityComponent))]
	class SpriteRenderer : OrderedRenderer<SpriteComponent>
	{
		public override void Render(Entity entity, SpriteComponent drawComponent)
		{
			if (drawComponent.Sprite != null)
			{
				GraceAttorneyGame.Game.SpriteBatch.Draw(drawComponent.Sprite, drawComponent.Position, null,
					HasComponent<OpacityComponent>(entity) ? new Color(1f, 1f, 1f, GetComponent<OpacityComponent>(entity).Opacity) : Color.White,
					0, Vector2.Zero, GraceAttorneyGame.Game.ScaleFactor, Microsoft.Xna.Framework.Graphics.SpriteEffects.None, 0);
			}
		}
	}
}

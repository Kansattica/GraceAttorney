using System;
using System.Collections.Generic;
using System.Text;
using Encompass;
using FNA.GraceAttorney.Components;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace FNA.GraceAttorney.Renderers
{
	[Reads(typeof(BackgroundComponent))]
	class BackgroundRenderer : OrderedRenderer<BackgroundComponent>
	{
		public override void Render(Entity entity, BackgroundComponent drawComponent)
		{
			var bg = ReadComponent<BackgroundComponent>();
			if (bg.Background != null)
				GraceAttorneyGame.Game.SpriteBatch.Draw(bg.Background, Vector2.Zero, Color.White);
		}
	}
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Encompass;
using GraceAttorney.DependencyInjection;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GraceAttorney.Renderers
{
	class SpriteTargetRenderer : GeneralRenderer
	{
		private readonly GraphicsDevice _graphics;
		private readonly RenderTarget2D _renderTarget;
		private readonly SpriteBatch _spriteBatch;

		public SpriteTargetRenderer(GraphicsDevice graphics, RenderTarget2D renderTarget, SpriteBatch spriteBatch)
		{
			_graphics = graphics;
			_renderTarget = renderTarget;
			_spriteBatch = spriteBatch;
		}

		public override void Render()
		{
			_graphics.SetRenderTarget(_renderTarget);
			_graphics.Clear(Color.Black);
			_spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.NonPremultiplied);
		}
	}
}

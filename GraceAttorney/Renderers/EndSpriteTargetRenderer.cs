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
	class EndSpriteTargetRenderer : GeneralRenderer
	{
		private readonly GraphicsDevice _graphics;
		private readonly SpriteBatch _spriteBatch;
		private readonly RenderTarget2D _renderTarget;
		private readonly UpdatedSize _screenSize;

		public EndSpriteTargetRenderer(GraphicsDevice graphics, UpdatedSize screenSize, RenderTarget2D renderTarget, SpriteBatch spriteBatch)
		{
			_graphics = graphics;
			_spriteBatch = spriteBatch;
			_renderTarget = renderTarget;
			_screenSize = screenSize;
		}

		public override void Render()
		{
			_spriteBatch.End();
			_graphics.SetRenderTarget(null);
			_graphics.Clear(Color.Black);
			_spriteBatch.Begin(SpriteSortMode.Deferred, null);

			var spriteScaleFactor = (float)_screenSize.Width / Common.Constants.BackgroundWidthInPixels;

			_spriteBatch.Draw(_renderTarget,
				new Vector2(_screenSize.Width - (Common.Constants.BackgroundWidthInPixels * spriteScaleFactor), 0),
				null, Color.White, 0, Vector2.Zero, spriteScaleFactor, SpriteEffects.None, 0);
		}
	}
}

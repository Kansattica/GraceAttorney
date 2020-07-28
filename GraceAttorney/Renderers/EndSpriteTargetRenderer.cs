using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Encompass;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GraceAttorney.Renderers
{
	class EndSpriteTargetRenderer : GeneralRenderer
	{
		private readonly GraphicsDevice _graphics;
		private readonly SpriteBatch _spriteBatch;
		private readonly RenderTarget2D _renderTarget;
		private readonly GraphicsDeviceManager _gdm;

		public EndSpriteTargetRenderer(GraphicsDevice graphics, GraphicsDeviceManager gdm, RenderTarget2D renderTarget, SpriteBatch spriteBatch)
		{
			_graphics = graphics;
			_spriteBatch = spriteBatch;
			_renderTarget = renderTarget;
			_gdm = gdm;
		}

		public override void Render()
		{
			_spriteBatch.End();
			_graphics.SetRenderTarget(null);
			_graphics.Clear(Color.Black);
			_spriteBatch.Begin(SpriteSortMode.Deferred, null);

			var spriteScaleFactor = (float)_gdm.PreferredBackBufferWidth / Common.Constants.BackgroundWidthInPixels;

			_spriteBatch.Draw(_renderTarget, Vector2.Zero,
				null, Color.White, 0, Vector2.Zero, spriteScaleFactor, SpriteEffects.None, 0);
		}
	}
}

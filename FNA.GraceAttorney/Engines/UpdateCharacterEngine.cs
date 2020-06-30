using System;
using System.Collections.Generic;
using System.Text;
using Encompass;
using FNA.GraceAttorney.Components;
using FNA.GraceAttorney.Messages;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace FNA.GraceAttorney.Engines
{
	[DefaultWritePriority(3)]
	[Writes(typeof(CharacterComponent), typeof(SpriteComponent), typeof(OpacityComponent))]
	[Reads(typeof(CharacterComponent), typeof(SpriteComponent))]
	[Receives(typeof(NewCharacterMessage))]
	class UpdateCharacterEngine : Engine
	{
		private const float CharacterStartsAtThisPercentDownTheScreen = .05f;
		public override void Update(double dt)
		{
			if (!SomeMessage<NewCharacterMessage>()) { return; }

			var entity = ReadEntity<CharacterComponent>();

			var sprite = GetComponent<SpriteComponent>(entity);

			var message = ReadMessage<NewCharacterMessage>();


			if (sprite.Sprite == null || sprite.Sprite.Name != message.AssetName)
				sprite.Sprite = GraceAttorneyGame.Game.Content.Load<Texture2D>(message.AssetName);

			float topOfHead = GraceAttorneyGame.Game.GraphicsDevice.Viewport.Height * CharacterStartsAtThisPercentDownTheScreen;
			float spriteOrigin =  (GraceAttorneyGame.Game.GraphicsDevice.Viewport.Width - (GraceAttorneyGame.Game.ScaleFactor * sprite.Sprite.Width)) / 2;

			sprite.Position = new Vector2(spriteOrigin, topOfHead);
			sprite.Layer = 1;

			SetComponent(entity, new OpacityComponent(direction: FadeDirection.FadeIn, opacity: 0, fadeRate: 1.0f));
			SetComponent(entity, sprite);
		}
	}
}

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
	[DefaultWritePriority(0)]
	[Writes(typeof(BackgroundComponent), typeof(SpriteComponent), typeof(OpacityComponent))]
	[Reads(typeof(BackgroundComponent), typeof(SpriteComponent))]
	[Receives(typeof(NewBackgroundMessage))]
	class SetBackgroundEngine : Engine
	{
		public override void Update(double dt)
		{
			if (!SomeMessage<NewBackgroundMessage>()) { return; }

			var entity = ReadEntity<BackgroundComponent>();

			var sprite = GetComponent<SpriteComponent>(entity);

			var message = ReadMessage<NewBackgroundMessage>();

			if (sprite.Sprite == null || sprite.Sprite.Name != message.AssetName)
				sprite.Sprite = GraceAttorneyGame.Game.Content.Load<Texture2D>(message.AssetName);
			sprite.Position = Vector2.Zero;

			SetComponent(entity, new OpacityComponent(direction: FadeDirection.FadeIn, opacity: 0, fadeRate: 1.0f));
			SetComponent(entity, sprite);

		}
	}
}

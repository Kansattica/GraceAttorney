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
	[Receives(typeof(NewBackgroundMessage), typeof(ClearBackgroundMessage))]
	class SetBackgroundEngine : Engine
	{
		public override void Update(double dt)
		{
			bool setBG = SomeMessage<NewBackgroundMessage>(), clearBG = SomeMessage<ClearBackgroundMessage>();

			if (!setBG && !clearBG) { return; }

			(var _, var entity) = ReadComponentIncludingEntity<BackgroundComponent>();

			var sprite = GetComponent<SpriteComponent>(entity);

			if (SomeMessage<NewBackgroundMessage>())
			{
				var message = ReadMessage<NewBackgroundMessage>();

				if (sprite.Sprite == null || sprite.Sprite.Name != message.AssetName)
					sprite.Sprite = GraceAttorneyGame.Game.Content.Load<Texture2D>(message.AssetName);
				sprite.Position = Vector2.Zero;
				SetComponent(entity, new OpacityComponent { Direction = FadeDirection.FadeIn, Opacity = 0, FadeRate = 1.0f });
			}
			else if (SomeMessage<ClearBackgroundMessage>())
			{
				SetComponent(entity, new OpacityComponent { Direction = FadeDirection.FadeOut, Opacity = 255, FadeRate = 1.0f });
			}

			SetComponent(entity, sprite);
		}
	}
}

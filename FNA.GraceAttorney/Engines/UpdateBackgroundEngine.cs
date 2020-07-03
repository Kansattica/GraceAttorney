using System;
using System.Collections.Generic;
using System.Text;
using Encompass;
using FNA.GraceAttorney.Components;
using FNA.GraceAttorney.Messages;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace FNA.GraceAttorney.Engines
{
	[DefaultWritePriority(0)]
	[Writes(typeof(BackgroundComponent), typeof(SpriteComponent))]
	[Reads(typeof(BackgroundComponent), typeof(SpriteComponent))]
	[Sends(typeof(StartMotionMessage))]
	[Receives(typeof(NewBackgroundMessage))]
	class UpdateBackgroundEngine : Engine
	{
		private readonly ContentManager _content;

		public UpdateBackgroundEngine(ContentManager content)
		{
			_content = content;
		}

		public override void Update(double dt)
		{
			if (!SomeMessage<NewBackgroundMessage>()) { return; }

			var entity = ReadEntity<BackgroundComponent>();

			var sprite = GetComponent<SpriteComponent>(entity);

			var message = ReadMessage<NewBackgroundMessage>();

			if (sprite.Sprite == null || sprite.Sprite.Name != message.AssetName)
				sprite.Sprite = _content.Load<Texture2D>(message.AssetName);
			sprite.Position = DrawLocation.Background;
			sprite.Layer = 0;

			SetComponent(entity, sprite);
			SendMessage(new StartMotionMessage { EnterFrom = EntranceDirection.FadeIn, Entity = entity });
		}
	}
}

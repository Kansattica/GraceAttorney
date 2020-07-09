using System;
using System.Collections.Generic;
using System.Text;
using Encompass;
using GraceAttorney.Components;
using GraceAttorney.Messages;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace GraceAttorney.Engines
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
			sprite.Layer = (int)SpriteLayers.Background;

			SetComponent(entity, sprite);
			SendMessage(new StartMotionMessage(entity, EntranceDirection.FadeIn));
		}
	}
}

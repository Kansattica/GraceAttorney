using System;
using System.Collections.Generic;
using System.Text;
using System.Transactions;
using Encompass;
using FNA.GraceAttorney.Components;
using FNA.GraceAttorney.Messages;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace FNA.GraceAttorney.Engines
{
	[DefaultWritePriority(3)]
	[Writes(typeof(CharacterComponent), typeof(SpriteComponent))]
	[Reads(typeof(CharacterComponent), typeof(SpriteComponent))]
	[Receives(typeof(NewCharacterMessage))]
	[Sends(typeof(StartMotionMessage))]
	class UpdateCharacterEngine : Engine
	{
		private readonly ContentManager _content;

		public UpdateCharacterEngine(ContentManager content)
		{
			_content = content;
		}

		public override void Update(double dt)
		{
			if (!SomeMessage<NewCharacterMessage>()) { return; }

			var entity = ReadEntity<CharacterComponent>();

			var sprite = GetComponent<SpriteComponent>(entity);

			var message = ReadMessage<NewCharacterMessage>();

			if (sprite.Sprite == null || sprite.Sprite.Name != message.AssetName)
				sprite.Sprite = _content.Load<Texture2D>(message.AssetName);

			sprite.Position = DrawLocation.Centered;
			sprite.Layer = 1;

			SendMessage(new StartMotionMessage { Entity = entity, EnterFrom = message.EnterFrom });

			SetComponent(entity, sprite);
		}

	}
}

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
	[Writes(typeof(SpriteComponent))]
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

			var entity = CreateEntity();

			ref readonly var message = ref ReadMessage<NewCharacterMessage>();

			AddComponent(entity, new SpriteComponent
			{
				Sprite = _content.Load<Texture2D>(message.AssetName),
				Position = DrawLocation.Centered,
				Layer = 1
			});

			SendMessage(new StartMotionMessage(entity, message.EnterFrom));
		}

	}
}

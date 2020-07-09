using System;
using System.Collections.Generic;
using System.Text;
using System.Transactions;
using Encompass;
using GraceAttorney.Components;
using GraceAttorney.Messages;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace GraceAttorney.Engines
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
			foreach (ref readonly var message in ReadMessages<NewCharacterMessage>())
			{
				var entity = CreateEntity();

				AddComponent(entity, new SpriteComponent
				{
					Sprite = _content.Load<Texture2D>(message.AssetName),
					Position = message.DrawLocation,
					Layer = (int)SpriteLayers.CharacterSprites
				});

				SendMessage(new StartMotionMessage(entity, message.EnterFrom));
			}
		}
	}
}
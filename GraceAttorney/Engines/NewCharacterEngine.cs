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
	[Writes(typeof(CharacterComponent))]
	[Receives(typeof(CharacterEnterMessage))]
	[Sends(typeof(NewSpriteMessage))]
	class NewCharacterEngine : Spawner<CharacterEnterMessage>
	{
		protected override void Spawn(CharacterEnterMessage message)
		{
			var entity = CreateEntity();

			AddComponent(entity, new CharacterComponent(message.CharacterName));

			SendMessage(new NewSpriteMessage(message.CharacterName, message.DrawLocation, SpriteLayers.CharacterSprites,
							message.EnterFrom, entity));
		}
	}
}

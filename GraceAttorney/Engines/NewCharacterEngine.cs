using System.IO;
using Encompass;
using GraceAttorney.Components;
using GraceAttorney.Messages;

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

			SendMessage(new NewSpriteMessage(
					Path.Combine(Constants.CharacterSpriteDirectory, message.CharacterName, message.Pose),
						message.DrawLocation, SpriteLayers.CharacterSprites, message.EnterFrom, entity));
		}
	}
}

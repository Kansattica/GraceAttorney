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
		private readonly ContentLoader _content;
		public NewCharacterEngine(ContentLoader content)
		{
			_content = content;
		}
		protected override void Spawn(in CharacterEnterMessage message)
		{
			var entity = CreateEntity();

			AddComponent(entity, new CharacterComponent(message.CharacterName));

			SendMessage(new NewSpriteMessage(
					_content.GetSpritePose(message.CharacterName, message.Pose),
						message.DrawLocation, SpriteLayers.CharacterSprites, message.EnterFrom, entity));
		}
	}
}

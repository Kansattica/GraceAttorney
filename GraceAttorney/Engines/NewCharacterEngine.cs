using System.IO;
using Encompass;
using GraceAttorney.Components;
using GraceAttorney.Messages;

namespace GraceAttorney.Engines
{
	[Writes(typeof(CharacterComponent),0)]
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

			SendMessage(new NewSpriteMessage(
					_content.GetSpritePose(message.CharacterName, message.Pose),
						message.DrawLocation, DrawLayers.CharacterSprites, message.EnterFrom, entity));

			AddComponent(entity, new CharacterComponent(message.CharacterName));
		}
	}
}

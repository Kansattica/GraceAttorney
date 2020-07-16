using System.IO;
using Encompass;
using GraceAttorney.Components;
using GraceAttorney.Messages;

namespace GraceAttorney.Engines
{
	[Writes(typeof(BackgroundComponent))]
	[Reads(typeof(BackgroundComponent))]
	[Sends(typeof(RemoveSpriteMessage), typeof(NewSpriteMessage))]
	[Receives(typeof(NewBackgroundMessage))]
	class NewBackgroundEngine : Spawner<NewBackgroundMessage>
	{
		protected override void Spawn(NewBackgroundMessage message)
		{
			if (SomeComponent<BackgroundComponent>())
			{
				ref readonly var entity = ref ReadEntity<BackgroundComponent>();
				RemoveComponent<BackgroundComponent>(entity);
				SendMessage(new RemoveSpriteMessage(entity));
			}

			var newBackground = CreateEntity();

			SetComponent(newBackground, new BackgroundComponent());

			SendMessage(new NewSpriteMessage(Path.Combine(Constants.BackgroundSpriteDirectory, message.AssetName),
							DrawLocation.Background, SpriteLayers.Background, EnterExitDirection.Fade, newBackground));
		}
	}
}

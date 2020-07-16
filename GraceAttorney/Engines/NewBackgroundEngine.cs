using System;
using System.CodeDom;
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

			SendMessage(new NewSpriteMessage(message.AssetName,
							DrawLocation.Background, SpriteLayers.Background, EnterExitDirection.Fade, newBackground));
		}
	}
}

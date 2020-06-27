using System;
using System.Collections.Generic;
using System.Text;
using Encompass;
using FNA.GraceAttorney.Components;
using FNA.GraceAttorney.Messages;
using Microsoft.Xna.Framework.Graphics;

namespace FNA.GraceAttorney.Engines
{
	[Writes(typeof(BackgroundComponent))]
	[Reads(typeof(BackgroundComponent))]
	[Receives(typeof(NewBackgroundMessage))]
	class SetBackgroundEngine : Engine
	{
		public override void Update(double dt)
		{
			if (SomeMessage<NewBackgroundMessage>())
			{
				(var background, var entity) = ReadComponentIncludingEntity<BackgroundComponent>();
				if (background.Background != null) { background.Background.Dispose(); }
				var message = ReadMessage<NewBackgroundMessage>();
				background.Background = GraceAttorneyGame.Game.Content.Load<Texture2D>(message.AssetName);
				SetComponent(entity, background);

			}
		}
	}
}

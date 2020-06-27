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
	[Receives(typeof(NewBackgroundMessage), typeof(ClearBackgroundMessage))]
	class SetBackgroundEngine : Engine
	{
		public override void Update(double dt)
		{
			bool setBG = SomeMessage<NewBackgroundMessage>(), clearBG = SomeMessage<ClearBackgroundMessage>();

			if (!setBG && !clearBG) { return; }

			(var background, var entity) = ReadComponentIncludingEntity<BackgroundComponent>();

			if (background.Background != null) { background.Background.Dispose(); }

			if (SomeMessage<NewBackgroundMessage>())
			{
				var message = ReadMessage<NewBackgroundMessage>();
				background.Background = GraceAttorneyGame.Game.Content.Load<Texture2D>(message.AssetName);
			}
			else if (SomeMessage<ClearBackgroundMessage>())
			{
				background.Background = null;
			}

			SetComponent(entity, background);
		}
	}
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Encompass;
using GraceAttorney.Components;

namespace GraceAttorney.Messages
{
	readonly struct StartFadeMessage : IMessage
	{
		public readonly Entity Sprite;
		public readonly FadeDirection Direction;

		public StartFadeMessage(Entity sprite, FadeDirection direction)
		{
			Sprite = sprite;
			Direction = direction;
		}
	}
}

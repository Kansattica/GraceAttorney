using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Encompass;

namespace GraceAttorney.Messages
{
	readonly struct RemoveSpriteMessage : IMessage 
	{
		public readonly Entity Sprite;
		public RemoveSpriteMessage(Entity sprite)
		{
			Sprite = sprite;
		}
	}
}

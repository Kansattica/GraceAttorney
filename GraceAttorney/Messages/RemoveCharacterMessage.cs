using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Encompass;

namespace GraceAttorney.Messages
{
	readonly struct RemoveCharacterMessage : IMessage 
	{
		public readonly Entity Character;
		public RemoveCharacterMessage(Entity character)
		{
			Character = character;
		}
	}
}

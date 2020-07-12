using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Encompass;
using GraceAttorney.Components;

namespace GraceAttorney.Messages
{
	readonly struct CharacterExitByPositionMessage : IMessage
	{
		readonly public DrawLocation Location;
		readonly public EnterExitDirection Direction;
		public CharacterExitByPositionMessage(DrawLocation location, EnterExitDirection direction)
		{
			Location = location;
			Direction = direction;
		}
	}
}

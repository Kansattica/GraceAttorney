using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Encompass;
using GraceAttorney.Components;

namespace GraceAttorney.Messages
{
	readonly struct CharacterExitByNameMessage : IMessage
	{
		public readonly string Name;
		public readonly EnterExitDirection Direction;

		public CharacterExitByNameMessage(string name, EnterExitDirection direction)
		{
			Name = name;
			Direction = direction;
		}
	}
}

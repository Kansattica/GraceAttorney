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
		readonly string Name;
		readonly EnterExitDirection Direction;
	}
}

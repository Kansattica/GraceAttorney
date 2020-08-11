using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Encompass;

namespace GraceAttorney.Components
{
	readonly struct CharacterComponent : IComponent
	{
		public readonly string Name;
		public readonly DrawLocation Location;

		public CharacterComponent(string name, DrawLocation location)
		{
			Name = name;
			Location = location;
		}
	}
}

using System;
using System.Collections.Generic;
using System.Text;
using Encompass;

namespace GraceAttorney.Components
{
	readonly struct AnimatedTextComponent : IComponent
	{
		public readonly int CharactersPerSecond;
		public readonly double CharactersVisible;

		public AnimatedTextComponent(int charactersPerSecond, double charactersVisible)
		{
			CharactersPerSecond = charactersPerSecond;
			CharactersVisible = charactersVisible;
		}
	}
}

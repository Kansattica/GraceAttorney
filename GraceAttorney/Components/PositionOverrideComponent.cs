using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Encompass;
using Microsoft.Xna.Framework;

namespace GraceAttorney.Components
{
	readonly struct PositionOverrideComponent : IComponent
	{
		public readonly Vector2 Position;

		public PositionOverrideComponent(in Vector2 position)
		{
			Position = position;
		}
	}
}

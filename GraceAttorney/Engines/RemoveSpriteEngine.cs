using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Encompass;
using GraceAttorney.Components;
using GraceAttorney.Messages;

namespace GraceAttorney.Engines
{
	[Receives(typeof(RemoveSpriteMessage))]
	[Writes(typeof(SpriteComponent), 4)]
	class RemoveSpriteEngine : Engine
	{
		public override void Update(double dt)
		{
			foreach (ref readonly var message in ReadMessages<RemoveSpriteMessage>())
			{
				ref readonly var character = ref message.Sprite;

				RemoveComponent<SpriteComponent>(character);
			}
		}
	}
}

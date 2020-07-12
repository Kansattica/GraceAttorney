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
	[Receives(typeof(RemoveCharacterMessage))]
	[Writes(typeof(SpriteComponent), 4)]
	class RemoveCharacterEngine : Engine
	{
		public override void Update(double dt)
		{
			foreach (ref readonly var message in ReadMessages<RemoveCharacterMessage>())
			{
				ref readonly var character = ref message.Character;

				RemoveComponent<SpriteComponent>(character);
			}
		}
	}
}

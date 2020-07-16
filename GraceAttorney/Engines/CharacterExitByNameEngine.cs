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
	[Receives(typeof(CharacterExitByNameMessage))]
	[Reads(typeof(CharacterComponent))]
	[Sends(typeof(CharacterExitMessage))]
	class CharacterExitByNameEngine : Engine
	{
		public override void Update(double dt)
		{
			foreach (ref readonly var message in ReadMessages<CharacterExitByNameMessage>())
			{
				// this is slow, but remember that there's only, like, three of these on screen at most.
				// we can even probably stop after we find one if it becomes a problem.
				foreach (ref readonly var entity in ReadEntities<CharacterComponent>())
				{
					ref readonly var character = ref GetComponent<CharacterComponent>(entity);
					if (character.Name == message.Name)
					{
						SendMessage(new CharacterExitMessage(entity, message.Direction));
					}
				}
			}
		}
	}
}

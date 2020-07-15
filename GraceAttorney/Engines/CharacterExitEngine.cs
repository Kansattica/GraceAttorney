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
	[Receives(typeof(CharacterExitMessage))]
	[Sends(typeof(StartMotionMessage), typeof(RemoveSpriteMessage))]
	class CharacterExitEngine : Engine
	{
		public override void Update(double dt)
		{
			foreach (ref readonly var message in ReadMessages<CharacterExitMessage>())
			{
				if (message.ExitTo == EnterExitDirection.NoAnimation)
					SendMessage(new RemoveSpriteMessage(message.Character));
				else
					SendMessage(new StartMotionMessage(message.Character, message.ExitTo, MotionDirection.Out));
			}
		}
	}
}

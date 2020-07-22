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
	[Reads(typeof(CharacterComponent))]
	[Sends(typeof(RemoveSpriteMessage))]
	[Receives(typeof(ClearAllCharactersMessage))]
	class ClearAllCharactersEngine : Spawner<ClearAllCharactersMessage>
	{
		protected override void Spawn(in ClearAllCharactersMessage message)
		{
			foreach (ref readonly var entity in ReadEntities<CharacterComponent>())
			{
				SendMessage(new RemoveSpriteMessage(entity));
			}
		}
	}
}

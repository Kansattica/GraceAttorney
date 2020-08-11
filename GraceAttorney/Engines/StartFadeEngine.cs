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
	[Writes(typeof(OpacityComponent), 0)]
	class StartFadeEngine : Spawner<StartFadeMessage>
	{
		private const float FadeRate = 1.0f;
		protected override void Spawn(in StartFadeMessage message)
		{
			SetComponent(message.Sprite, new OpacityComponent(
				message.Direction == FadeDirection.FadeIn ? 0 : 1.0f,
				FadeRate, message.Direction));
		}
	}
}

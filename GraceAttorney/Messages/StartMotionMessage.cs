using System;
using System.Collections.Generic;
using System.Text;
using Encompass;

namespace GraceAttorney.Messages
{
	readonly struct StartMotionMessage : IMessage
	{

		public readonly Entity Entity;
		public readonly EntranceDirection EnterFrom;
		public StartMotionMessage(in Entity entity, EntranceDirection enterFrom)
		{
			Entity = entity;
			EnterFrom = enterFrom;
		}
	}
}

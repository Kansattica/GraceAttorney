using System;
using System.Collections.Generic;
using System.Text;
using Encompass;

namespace FNA.GraceAttorney.Messages
{
	struct StartMotionMessage : IMessage, IHasEntity
	{
		public Entity Entity { get; set; }
		public EntranceDirection EnterFrom { get; set; }
	}
}

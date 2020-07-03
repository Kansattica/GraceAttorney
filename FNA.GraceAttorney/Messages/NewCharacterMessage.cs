using System;
using System.Collections.Generic;
using System.Text;
using Encompass;

namespace FNA.GraceAttorney.Messages
{
	enum EntranceDirection { Top, Bottom, Left, Right, FadeIn }
	readonly struct NewCharacterMessage : IMessage
	{
		public readonly string AssetName;
		public readonly EntranceDirection EnterFrom;
		public NewCharacterMessage(string assetName, EntranceDirection enterFrom = EntranceDirection.FadeIn)
		{
			AssetName = assetName;
			EnterFrom = enterFrom;
		}
	}
}

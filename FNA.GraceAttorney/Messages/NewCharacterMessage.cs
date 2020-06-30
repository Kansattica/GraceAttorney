using System;
using System.Collections.Generic;
using System.Text;
using Encompass;

namespace FNA.GraceAttorney.Messages
{
	readonly struct NewCharacterMessage : IMessage
	{
		public readonly string AssetName;
		public NewCharacterMessage(string assetName)
		{
			AssetName = assetName;
		}
	}
}

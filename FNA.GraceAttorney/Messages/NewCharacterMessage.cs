using System;
using System.Collections.Generic;
using System.Text;
using Encompass;
using FNA.GraceAttorney.Components;

namespace FNA.GraceAttorney.Messages
{
	enum EntranceDirection { Top, Bottom, Left, Right, FadeIn }
	readonly struct NewCharacterMessage : IMessage
	{
		public readonly string AssetName;
		public readonly EntranceDirection EnterFrom;
		public readonly DrawLocation DrawLocation;
		public NewCharacterMessage(string assetName, EntranceDirection enterFrom = EntranceDirection.FadeIn, DrawLocation drawLocation = DrawLocation.Centered)
		{
			AssetName = assetName;
			EnterFrom = enterFrom;
			DrawLocation = drawLocation;
		}
	}
}

using System;
using System.Collections.Generic;
using System.Text;
using Encompass;
using GraceAttorney.Components;

namespace GraceAttorney.Messages
{
	enum EnterExitDirection { Top, Bottom, Left, Right, Fade }
	readonly struct CharacterEnterMessage : IMessage
	{
		public readonly string CharacterName;
		public readonly EnterExitDirection EnterFrom;
		public readonly DrawLocation DrawLocation;
		public CharacterEnterMessage(string assetName, EnterExitDirection enterFrom = EnterExitDirection.Fade, DrawLocation drawLocation = DrawLocation.Center)
		{
			CharacterName = assetName;
			EnterFrom = enterFrom;
			DrawLocation = drawLocation;
		}
	}
}

using System;
using System.Collections.Generic;
using System.Text;
using Encompass;
using GraceAttorney.Components;

namespace GraceAttorney.Messages
{
	enum EnterExitDirection { Top, Bottom, Left, Right, Fade, NoAnimation }
	readonly struct CharacterEnterMessage : IMessage
	{
		public readonly string CharacterName;
		public readonly string Pose;
		public readonly EnterExitDirection EnterFrom;
		public readonly DrawLocation DrawLocation;
		public CharacterEnterMessage(string characterName, string pose, EnterExitDirection enterFrom = EnterExitDirection.Fade, DrawLocation drawLocation = DrawLocation.Center)
		{
			CharacterName = characterName;
			Pose = pose;
			EnterFrom = enterFrom;
			DrawLocation = drawLocation;
		}
	}
}

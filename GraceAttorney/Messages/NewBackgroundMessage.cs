using System;
using System.Collections.Generic;
using System.Text;
using Encompass;

namespace GraceAttorney.Messages
{
	readonly struct NewBackgroundMessage : IMessage
	{
		public readonly string AssetName;
		public NewBackgroundMessage(string assetName)
		{
			AssetName = assetName;
		}
	}
}

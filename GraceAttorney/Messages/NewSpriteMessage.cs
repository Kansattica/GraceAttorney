using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Encompass;
using GraceAttorney.Components;

namespace GraceAttorney.Messages
{
	readonly struct NewSpriteMessage : IMessage
	{
		public readonly string AssetPath;
		public readonly DrawLocation Position;
		public readonly SpriteLayers Layer;
		public readonly EnterExitDirection EnterFrom;
		public readonly Entity Entity;

		public NewSpriteMessage(string assetName, DrawLocation position, SpriteLayers layer, EnterExitDirection enterFrom, in Entity entity)
		{
			AssetPath = assetName;
			Position = position;
			Layer = layer;
			EnterFrom = enterFrom;
			Entity = entity;
		}
	}
}

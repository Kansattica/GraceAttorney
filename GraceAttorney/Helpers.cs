using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GraceAttorney.Components;
using GraceAttorney.Messages;
using Microsoft.Xna.Framework;

namespace GraceAttorney
{
	static class Helpers
	{
		private const float SideCharacterXOffset = .30f;

		public static Vector2 CalculatePosition(DrawLocation location, int spriteFrameWidth, int spriteFrameHeight)
		{
			var originToCenterTheSpriteAlongTheXAxis = (Common.Constants.BackgroundWidthInPixels - spriteFrameWidth) / 2;
			return location switch
			{
				// center the background so that it displays okay even if the window has been maximized to something non-16:9.
				// this might be unnecessary, but, you know, doesn't hurt				
				DrawLocation.Background => new Vector2(originToCenterTheSpriteAlongTheXAxis, 0),
				DrawLocation.Center => new Vector2(originToCenterTheSpriteAlongTheXAxis, CalculateTopOfHeadYPosition(spriteFrameHeight)),
				DrawLocation.Left => new Vector2(originToCenterTheSpriteAlongTheXAxis - (SideCharacterXOffset * Common.Constants.BackgroundWidthInPixels),
					CalculateTopOfHeadYPosition(spriteFrameHeight)),
				DrawLocation.Right => new Vector2(originToCenterTheSpriteAlongTheXAxis + (SideCharacterXOffset * Common.Constants.BackgroundWidthInPixels),
					CalculateTopOfHeadYPosition(spriteFrameHeight)),
				_ => throw new NotImplementedException("You fell out of the switch."),
			};
		}

		private static float CalculateTopOfHeadYPosition(int spriteHeight)
		{
			return Common.Constants.BackgroundHeightInPixels - spriteHeight;
		}

		public static Vector2 CalculateOffscreenPosition(EnterExitDirection direction, in Vector2 currentPosition, int spriteWidth, int spriteHeight)
		{
			switch (direction)
			{
				case EnterExitDirection.Top:
					return new Vector2(currentPosition.X, -spriteHeight); // wait until they're off the screen by their height
				case EnterExitDirection.Bottom:
					return new Vector2(currentPosition.X, Common.Constants.BackgroundHeightInPixels);
				case EnterExitDirection.Left:
					return new Vector2(-spriteWidth, currentPosition.Y);
				case EnterExitDirection.Right:
					return new Vector2(Common.Constants.BackgroundWidthInPixels, currentPosition.Y);
			}
			throw new ArgumentException("Hey, buddy, you gotta pass in a cardinal direction here.");
		}
	}
}

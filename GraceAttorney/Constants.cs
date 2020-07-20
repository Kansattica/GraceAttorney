using System;
using System.Collections.Generic;
using System.Text;

namespace GraceAttorney
{
	// note that these are in render order: background, then sprites, then dialogue, and so on.
	enum SpriteLayers { Background, CharacterSprites, DialogueBox, VeryTop}
	static class Constants
	{
		public const int BackgroundHeightInPixels = 1080;

		public const int CharactersPerLine = 40;
		public const int ExpectedLineCount = 3;

		// Note that this only applies to left-justified "normal" dialogue.
		// centered text doesn't wrap, so use CharactersPerLine for it.
		public const int CharactersPerDialogueBox = CharactersPerLine * ExpectedLineCount;


		public const string CharacterSpriteDirectory = "Characters";
		public const string BackgroundSpriteDirectory = "Backgrounds";
	}
}

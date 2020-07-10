using System;
using System.Collections.Generic;
using System.Text;

namespace GraceAttorney
{
	// note that these are in render order: background, then sprites, then dialogue, and so on.
	enum SpriteLayers { Background, CharacterSprites, DialogueBox, VeryTop}
	static class Constants
	{
		public const int CharactersPerLine = 40;
		public const int ExpectedLineCount = 3;
		public const int CharactersPerDialogueBox = CharactersPerLine * ExpectedLineCount;
	}
}

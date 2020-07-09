using System;
using System.Collections.Generic;
using System.Text;

namespace FNA.GraceAttorney
{
	enum SpriteLayers { Background, CharacterSprites, DialogueBox, VeryTop}
	static class Constants
	{
		public const int CharactersPerLine = 40;
		public const int ExpectedLineCount = 3;
		public const int CharactersPerDialogueBox = CharactersPerLine * ExpectedLineCount;

	}
}

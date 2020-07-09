using System;
using System.IO;
using SpriteFontPlus;

namespace Fonts.GraceAttorney
{
    public class GameFonts
    {
		//currently, thank you George Douros's Unicode Fonts for Ancient Scripts
		public static DynamicSpriteFont Dialogue { get; private set; } = DynamicSpriteFont.FromTtf(File.ReadAllBytes(Path.Combine("Content", "Fonts", "Aroania.ttf")), 36);
		public static DynamicSpriteFont NameTag { get; private set; } = DynamicSpriteFont.FromTtf(File.ReadAllBytes(Path.Combine("Content", "Fonts", "AroaniaBold.ttf")), 32);
    }
}

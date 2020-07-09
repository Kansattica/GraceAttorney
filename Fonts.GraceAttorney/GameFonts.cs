using System;
using System.IO;
using SpriteFontPlus;

namespace Fonts.GraceAttorney
{
    public class GameFonts
    {
		public static DynamicSpriteFont Dialogue { get; private set; } = DynamicSpriteFont.FromTtf(File.ReadAllBytes(Path.Combine("Content", "Fonts", "Aroania.ttf")), 46);
		public static DynamicSpriteFont NameTag { get; private set; } = DynamicSpriteFont.FromTtf(File.ReadAllBytes(Path.Combine("Content", "Fonts", "AroaniaBold.ttf")), 32);
    }
}

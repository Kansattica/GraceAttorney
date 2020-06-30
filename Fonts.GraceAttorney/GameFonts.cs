using System;
using System.IO;
using SpriteFontPlus;

namespace Fonts.GraceAttorney
{
    public class GameFonts
    {
		public static DynamicSpriteFont Dialogue { get; private set; } = DynamicSpriteFont.FromTtf(File.ReadAllBytes(@"C:\\Windows\\Fonts\arial.ttf"), 24);
		public static DynamicSpriteFont NameTag { get; private set; } = DynamicSpriteFont.FromTtf(File.ReadAllBytes(@"C:\\Windows\\Fonts\OpenSans-Bold.ttf"), 24);
    }
}

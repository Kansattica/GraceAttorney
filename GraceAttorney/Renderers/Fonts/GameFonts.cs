using System;
using System.IO;
using SpriteFontPlus;

namespace GraceAttorney.Renderers.Fonts
{
    public class GameFonts
    {
		// using Luciole (https://www.luciole-vision.com/luciole-en.html) for now
		public static DynamicSpriteFont Dialogue { get; private set; } = DynamicSpriteFont.FromTtf(File.ReadAllBytes(Path.Combine("Content", "Fonts", "Dialogue.ttf")), 24);

		public static DynamicSpriteFont NameTag { get; private set; } = DynamicSpriteFont.FromTtf(File.ReadAllBytes(Path.Combine("Content", "Fonts", "NameTag.ttf")), 24);
    }
}

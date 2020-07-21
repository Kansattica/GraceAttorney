using System;
using System.IO;
using SpriteFontPlus;

namespace GraceAttorney.Renderers.Fonts
{
    public class GameFonts
    {
		//currently, thank you George Douros's Unicode Fonts for Ancient Scripts
		// (Aroania for Dialogue, AroaniaBold for name tag)
		public static DynamicSpriteFont Dialogue { get; private set; } = DynamicSpriteFont.FromTtf(File.ReadAllBytes(Path.Combine("Content", "Fonts", "Dialogue.ttf")), 35);
		public static DynamicSpriteFont NameTag { get; private set; } = DynamicSpriteFont.FromTtf(File.ReadAllBytes(Path.Combine("Content", "Fonts", "NameTag.ttf")), 28);
    }
}

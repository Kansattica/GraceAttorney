using Encompass;
using Microsoft.Xna.Framework;

namespace GraceAttorney.Components
{
	enum NameTagLocation { Left, Center, Right };
	enum JustifyText { Left, Center };
	readonly struct DialogueComponent : IComponent, IDrawableComponent
	{
		public readonly bool Display;
		public readonly string Speaker;
		public readonly string Dialogue;
		public readonly NameTagLocation NameTagLocation;
		public readonly JustifyText Justification;
		public readonly Color TextColor;

		public int Layer { get => (int)DrawLayers.DialogueBox; }

		public DialogueComponent(bool display, string speaker, string dialogue, NameTagLocation nameTagLocation, JustifyText justification, Color textColor)
		{
			Display = display;
			Speaker = speaker;
			Dialogue = dialogue;
			NameTagLocation = nameTagLocation;
			Justification = justification;
			TextColor = textColor;
		}
	}
}

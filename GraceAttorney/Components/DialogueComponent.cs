using Encompass;
using Microsoft.Xna.Framework;

namespace GraceAttorney.Components
{
	enum NameTagLocation { Left, Center, Right };
	enum JustifyText { Left, Center };
	struct DialogueComponent : IComponent, IDrawableComponent
	{
		public bool Display;
		public string Speaker;
		public string Dialogue;
		public NameTagLocation NameTagLocation;
		public JustifyText Justification;
		public Color TextColor;

		public int Layer { get => (int)SpriteLayers.DialogueBox; }
	}
}

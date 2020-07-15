using System;
using System.Collections.Generic;
using System.Text;
using Encompass;

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

		public int Layer { get; set; }
	}
}

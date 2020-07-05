using System;
using System.Collections.Generic;
using System.Text;
using Encompass;

namespace FNA.GraceAttorney.Components
{
	enum NameTagLocation { Left, Center, Right };
	struct DialogueComponent : IComponent, IDrawableComponent
	{
		public bool Display;
		public string Speaker;
		public string Dialogue;
		public NameTagLocation NameTagLocation;

		public int Layer { get; set; }
	}
}

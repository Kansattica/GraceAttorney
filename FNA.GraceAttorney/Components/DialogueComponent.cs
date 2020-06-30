using System;
using System.Collections.Generic;
using System.Text;
using Encompass;

namespace FNA.GraceAttorney.Components
{
	struct DialogueComponent : IComponent, IDrawableComponent
	{
		public string Speaker;
		public string Dialogue;
		public bool ShowBox;

		public int Layer { get; set; }
	}
}

using System;
using System.Collections.Generic;
using System.Text;
using Encompass;
using Microsoft.Xna.Framework.Graphics;

namespace FNA.GraceAttorney.Components
{
	struct BackgroundComponent : IDrawableComponent, IComponent
	{
		public Texture2D Background;

		public int Layer { get; set; }
	}
}

using System;
using System.Collections.Generic;
using System.Text;
using Encompass;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GraceAttorney.Components
{
	enum DrawLocation { Background, Center, Left, Right }
	struct SpriteComponent : IDrawableComponent, IComponent
	{
		public DrawLocation Position;
		public Texture2D Sprite;
		public int Layer { get; set; }
	}
}

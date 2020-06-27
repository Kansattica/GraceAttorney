using System;
using System.Collections.Generic;
using System.Text;
using Encompass;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace FNA.GraceAttorney.Components
{
	struct SpriteComponent : IDrawableComponent, IComponent
	{
		public Vector2 Position;
		public Texture2D Sprite;
		public int Layer { get; set; }
	}
}

using System;
using System.Collections.Generic;
using System.Collections.Immutable;
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
		public ImmutableArray<Texture2D> Frames;
		public double CurrentFrame;
		public int Layer { get; set; }
	}
}

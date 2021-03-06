using Encompass;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GraceAttorney.Components
{
	enum DrawLocation { Background, Center, Left, Right }
	readonly struct SpriteComponent : IDrawableComponent, IComponent
	{
		public readonly Vector2 Position;
		public readonly Texture2D Texture;
		public readonly int FrameWidth;
		public readonly int FrameHeight;

		public int Layer { get; }

		public SpriteComponent(Vector2 position, Texture2D texture, int frameWidth, int frameHeight, int layer)
		{
			Position = position;
			Texture = texture;
			FrameWidth = frameWidth;
			FrameHeight = frameHeight;
			Layer = layer;
		}
	}
}

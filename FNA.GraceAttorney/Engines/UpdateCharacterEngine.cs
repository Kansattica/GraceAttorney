using System;
using System.Collections.Generic;
using System.Text;
using System.Transactions;
using Encompass;
using FNA.GraceAttorney.Components;
using FNA.GraceAttorney.Messages;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace FNA.GraceAttorney.Engines
{
	[DefaultWritePriority(3)]
	[Writes(typeof(CharacterComponent), typeof(SpriteComponent), typeof(OpacityComponent), typeof(MovingSpriteComponent), typeof(SpriteOffsetComponent))]
	[Reads(typeof(CharacterComponent), typeof(SpriteComponent))]
	[Receives(typeof(NewCharacterMessage))]
	class UpdateCharacterEngine : Engine
	{
		private readonly ContentManager _content;

		public UpdateCharacterEngine(ContentManager content)
		{
			_content = content;
		}

		public override void Update(double dt)
		{
			if (!SomeMessage<NewCharacterMessage>()) { return; }

			var entity = ReadEntity<CharacterComponent>();

			var sprite = GetComponent<SpriteComponent>(entity);

			var message = ReadMessage<NewCharacterMessage>();

			if (sprite.Sprite == null || sprite.Sprite.Name != message.AssetName)
				sprite.Sprite = _content.Load<Texture2D>(message.AssetName);

			sprite.Position = DrawLocation.Centered;
			sprite.Layer = 1;

			if (message.EnterFrom == EntranceDirection.FadeIn)
			{
				SetComponent(entity, new OpacityComponent(direction: FadeDirection.FadeIn, opacity: 0, fadeRate: 1.0f));
			}
			else
			{
				SetComponent(entity, new MovingSpriteComponent { Direction = MotionDirection.In, Velocity = .5f });
				SetComponent(entity, new SpriteOffsetComponent { PositionPercentageOffset = GetDirectionVector(message.EnterFrom) });
			}

			SetComponent(entity, sprite);
		}

		private static Vector2 GetDirectionVector(EntranceDirection direction)
		{
			switch (direction)
			{
				case EntranceDirection.Top:
					return -Vector2.UnitY;
				case EntranceDirection.Bottom:
					return Vector2.UnitY;
				case EntranceDirection.Right:
					return Vector2.UnitX;
				case EntranceDirection.Left:
					return -Vector2.UnitX;
			}
			throw new ArgumentException("What, bud, did you invent a new direction or something");
		}
	}
}

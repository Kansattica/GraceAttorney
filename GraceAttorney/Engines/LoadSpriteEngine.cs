using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Encompass;
using GraceAttorney.Components;
using GraceAttorney.Messages;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace GraceAttorney.Engines
{
	[Receives(typeof(NewSpriteMessage))]
	[Writes(typeof(SpriteComponent), 0)]
	[Sends(typeof(StartMotionMessage))]
	class LoadSpriteEngine : Engine
	{
		private readonly ContentManager _content;

		public LoadSpriteEngine(ContentManager content)
		{
			_content = content;
		}

		public override void Update(double dt)
		{
			foreach (ref readonly var newSprite in ReadMessages<NewSpriteMessage>())
			{
				SetComponent(newSprite.Entity, new SpriteComponent
				{
					Layer = (int)newSprite.Layer,
					Position = newSprite.Position,
					Sprite = _content.Load<Texture2D>(newSprite.AssetName)
				});

				if (newSprite.EnterFrom != EnterExitDirection.NoAnimation)
				{
					SendMessage(new StartMotionMessage(newSprite.Entity, newSprite.EnterFrom, MotionDirection.In));
				}
			}
		}
	}
}

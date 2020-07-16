using Encompass;
using GraceAttorney.Components;
using GraceAttorney.Messages;

namespace GraceAttorney.Engines
{
	[Receives(typeof(NewSpriteMessage))]
	[Writes(typeof(SpriteComponent), 1)]
	[Sends(typeof(StartMotionMessage))]
	class LoadSpriteEngine : Engine
	{
		private readonly OnDemandContentLoader _content;

		public LoadSpriteEngine(OnDemandContentLoader content)
		{
			_content = content;
		}

		public override void Update(double dt)
		{
			foreach (ref readonly var newSprite in ReadMessages<NewSpriteMessage>())
			{
				var sprite = new SpriteComponent
				{
					Layer = (int)newSprite.Layer,
					Position = newSprite.Position,
					Frames = _content.GetSpriteByPath(newSprite.AssetPath),
					CurrentFrame = 0
				};

				SetComponent(newSprite.Entity, sprite); 

				if (newSprite.EnterFrom != EnterExitDirection.NoAnimation)
				{
					SendMessage(new StartMotionMessage(newSprite.Entity, newSprite.EnterFrom, MotionDirection.In));
				}
			}
		}
	}
}

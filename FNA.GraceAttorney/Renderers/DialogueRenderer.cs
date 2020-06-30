using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using Encompass;
using FNA.GraceAttorney.Components;
using Fonts.GraceAttorney;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace FNA.GraceAttorney.Renderers
{
	class DialogueRenderer : OrderedRenderer<DialogueComponent>
	{
		private const float TextBoxScreenPercent = .40f;
		private readonly Color TextBoxColor = new Color(0, 0, 0, .9f);
		private readonly Color BorderColor = Color.DarkViolet;
		private const int BorderWidthInPixels = 1;

		private const float SpeakerBoxXOffsetPercent = .005f;
		private const float SpeakerBoxWidthPercent = 1.30f;
		private const float SpeakerBoxHeightPercent = 1.10f;

		public override void Render(Entity entity, DialogueComponent drawComponent)
		{
			if (!drawComponent.ShowBox) { return; }

			int screenHeight = GraceAttorneyGame.Game.GraphicsDevice.Viewport.Height;
			int screenWidth = GraceAttorneyGame.Game.GraphicsDevice.Viewport.Width;

			int textBoxStartsAt = (int)(screenHeight * (1 - TextBoxScreenPercent));
			int textBoxHeight = (int)(screenHeight * TextBoxScreenPercent) + 1; //+1 so there's no gap due to rounding error

			GraceAttorneyGame.Game.SpriteBatch.Draw(null,
				new Rectangle(0, textBoxStartsAt, screenWidth, textBoxHeight),
				TextBoxColor
				);

			DrawBorder(screenWidth, textBoxStartsAt, textBoxHeight, screenHeight);

			DrawDialogue(drawComponent, screenHeight, screenWidth, textBoxStartsAt);

			var speakerSize = GameFonts.NameTag.MeasureString(drawComponent.Speaker);

			int speakerBoxWidth = (int)(speakerSize.X * SpeakerBoxWidthPercent);
			int speakerBoxHeight = (int)(speakerSize.Y * SpeakerBoxHeightPercent);

			int nameTagStartsAtX = (int)(screenWidth * SpeakerBoxXOffsetPercent);

			GraceAttorneyGame.Game.SpriteBatch.Draw(null,
				new Rectangle(nameTagStartsAtX, textBoxStartsAt - speakerBoxHeight, speakerBoxWidth, speakerBoxHeight),
				TextBoxColor);

			GameFonts.NameTag.DrawString(GraceAttorneyGame
				 .Game.SpriteBatch, drawComponent.Speaker,
				new Vector2(nameTagStartsAtX + (speakerBoxWidth - speakerSize.X)/2, textBoxStartsAt - speakerBoxHeight), Color.White);

			DrawBorder(speakerBoxWidth, textBoxStartsAt - speakerBoxHeight, speakerBoxHeight, textBoxStartsAt, nameTagStartsAtX, drawBottom: false);
		}

		private static void DrawDialogue(DialogueComponent drawComponent, int screenHeight, int screenWidth, int textBoxStartsAt)
		{
			int xDialogueOffset = (int)(screenWidth * .01);
			int yDialogueOffset = (int)(screenHeight * .02);

			GameFonts.Dialogue.DrawString(GraceAttorneyGame
				 .Game.SpriteBatch, drawComponent.Dialogue,
				new Vector2(xDialogueOffset, textBoxStartsAt + yDialogueOffset), Color.White);
		}

		private void DrawBorder(int boxWidth, int topOfBox, int textBoxHeight, int BottomOfBox, int xOffset = 0, bool drawBottom = true)
		{
			var colorTexture = new Texture2D(GraceAttorneyGame.Game.GraphicsDevice, 1, 1);
			colorTexture.SetData(new[] { Color.White });

			// top border
			GraceAttorneyGame.Game.SpriteBatch.Draw(colorTexture,
				new Rectangle(xOffset, topOfBox, boxWidth, BorderWidthInPixels),
				BorderColor
				);

			// left side
			GraceAttorneyGame.Game.SpriteBatch.Draw(colorTexture,
				new Rectangle(xOffset, topOfBox, BorderWidthInPixels, textBoxHeight),
				BorderColor
				);

			if (drawBottom)
			{
				// bottom side
				GraceAttorneyGame.Game.SpriteBatch.Draw(colorTexture,
					new Rectangle(xOffset, BottomOfBox - BorderWidthInPixels, boxWidth, BorderWidthInPixels),
					BorderColor
					);
			}

			// right side
			GraceAttorneyGame.Game.SpriteBatch.Draw(colorTexture,
				new Rectangle(boxWidth - BorderWidthInPixels + xOffset, topOfBox, BorderWidthInPixels, textBoxHeight),
				BorderColor
				);
		}
	}
}

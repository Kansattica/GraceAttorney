using System;
using System.ComponentModel;
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
		private readonly Color TextBoxColor = new Color(0, 0, 0, .9f);
		private readonly Color OuterBorderColor = Color.Gray;
		private readonly Color InnerBorderColor = Color.DarkGray;

		private const float DialogueBoxVerticalScreenPercentage = .40f;
		private const float DialogueBoxHorizontalScreenPercentage = .80f;

		private const int BorderWidthInPixels = 2;

		private const float NameTagXOffsetPercent = 1.1f;
		private const float NameTagWidthPercent = 1.30f;
		private const float NameTagHeightPercent = 1.15f;

		public override void Render(Entity entity, DialogueComponent drawComponent)
		{
			if (!drawComponent.ShowBox) { return; }

			Rectangle dialogueBoxRect = CalculateDialogueBoxDimensions();

			GraceAttorneyGame.Game.SpriteBatch.Draw(null, dialogueBoxRect, TextBoxColor);

			DrawDialogueBoxBorder(dialogueBoxRect);

			DrawDialogue(new StringBuilder(drawComponent.Dialogue), dialogueBoxRect.X, dialogueBoxRect.Y);

			DrawNameTag(drawComponent, dialogueBoxRect);
		}

		private static Rectangle CalculateDialogueBoxDimensions()
		{
			int screenHeight = GraceAttorneyGame.Game.GraphicsDevice.Viewport.Height;
			int screenWidth = GraceAttorneyGame.Game.GraphicsDevice.Viewport.Width;

			int textBoxStartsAt = (int)(screenHeight * (1 - DialogueBoxVerticalScreenPercentage)) + 2 * BorderWidthInPixels;
			int textBoxHeight = (int)(screenHeight * DialogueBoxVerticalScreenPercentage) - 2 * BorderWidthInPixels;

			int dialogueBoxWidth = (int)(screenWidth * DialogueBoxHorizontalScreenPercentage);
			int dialogueBoxOffsetFromScreenSide = (screenWidth - dialogueBoxWidth) / 2;

			return new Rectangle(dialogueBoxOffsetFromScreenSide, textBoxStartsAt, dialogueBoxWidth, textBoxHeight);
		}

		// "in" passes by immutable reference
		// https://docs.microsoft.com/en-us/dotnet/csharp/language-reference/keywords/in-parameter-modifier
		private void DrawNameTag(in DialogueComponent drawComponent, in Rectangle dialogueBoxRect)
		{
			var speakerSize = GameFonts.NameTag.MeasureString(drawComponent.Speaker);

			int nameTagHeight = (int)(speakerSize.Y * NameTagHeightPercent);

			var nameTagRect = new Rectangle((int)(dialogueBoxRect.X * NameTagXOffsetPercent), dialogueBoxRect.Y - nameTagHeight,
				 (int)(speakerSize.X * NameTagWidthPercent), nameTagHeight);

			GraceAttorneyGame.Game.SpriteBatch.Draw(null, nameTagRect, TextBoxColor);

			GameFonts.NameTag.DrawString(GraceAttorneyGame
				 .Game.SpriteBatch, drawComponent.Speaker,
				new Vector2(nameTagRect.X + (nameTagRect.Width - speakerSize.X) / 2, dialogueBoxRect.Y - nameTagRect.Height), Color.White);

			DrawBorder(nameTagRect, OuterBorderColor, drawBottom: false);
		}

		private void DrawDialogueBoxBorder(Rectangle borderRect)
		{
			DrawBorder(borderRect, OuterBorderColor);

			DrawBorder(new Rectangle(borderRect.X + BorderWidthInPixels, borderRect.Y + BorderWidthInPixels,
				borderRect.Width - BorderWidthInPixels * 2, borderRect.Height - BorderWidthInPixels * 2)
				, InnerBorderColor);
		}

		private static void DrawDialogue(StringBuilder dialogue, int dialogueBoxX, int dialogueBoxY)
		{
			int xDialogueOffset = dialogueBoxX + 2 * BorderWidthInPixels + 10;
			int yDialogueOffset = dialogueBoxY + 2 * BorderWidthInPixels + 5;

			GameFonts.Dialogue.DrawString(GraceAttorneyGame
				 .Game.SpriteBatch, dialogue,
				new Vector2(xDialogueOffset, yDialogueOffset), Color.White);
		}

		private void DrawBorder(in Rectangle bounds, in Color color, bool drawBottom = true)
		{
			// top border
			GraceAttorneyGame.Game.SpriteBatch.Draw(_colorTexture,
				new Rectangle(bounds.X, bounds.Y, bounds.Width, BorderWidthInPixels),
				color
				);

			// left side
			GraceAttorneyGame.Game.SpriteBatch.Draw(_colorTexture,
				new Rectangle(bounds.X, bounds.Y, BorderWidthInPixels, bounds.Height),
				color
				);

			if (drawBottom)
			{
				// bottom side
				GraceAttorneyGame.Game.SpriteBatch.Draw(_colorTexture,
					new Rectangle(bounds.X, bounds.Y + bounds.Height - BorderWidthInPixels, bounds.Width, BorderWidthInPixels),
					color
					);
			}

			// right side
			GraceAttorneyGame.Game.SpriteBatch.Draw(_colorTexture,
				new Rectangle(bounds.X + bounds.Width - BorderWidthInPixels, bounds.Y, BorderWidthInPixels, bounds.Height),
				color
				);
		}

		private static readonly Texture2D _colorTexture = MakeTexture();
		private static Texture2D MakeTexture()
		{
			var colorTexture = new Texture2D(GraceAttorneyGame.Game.GraphicsDevice, 1, 1);
			colorTexture.SetData(new[] { Color.White });
			return colorTexture;
		}
	}
}

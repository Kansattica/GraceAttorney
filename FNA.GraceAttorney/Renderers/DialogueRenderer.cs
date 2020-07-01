using System;
using System.ComponentModel;
using System.Text;
using Encompass;
using FNA.GraceAttorney.Components;
using FNA.GraceAttorney.DependencyInjection;
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
		private const float DialogueBoxHorizontalScreenPercentage = .85f;

		private const int BorderWidthInPixels = 2;

		private const float NameTagXOffsetPercent = 1.1f;
		private const float NameTagWidthPercent = 1.30f;
		private const float NameTagHeightPercent = 1.15f;

		private readonly SpriteBatch _spriteBatch;
		private readonly UpdatedSize _viewport;
		private readonly Texture2D _colorTexture;
		public DialogueRenderer(SpriteBatch spriteBatch, UpdatedSize viewport, Texture2D colorTexture)
		{
			_spriteBatch = spriteBatch;
			_viewport = viewport;
			_colorTexture = colorTexture;
		}

		public override void Render(Entity entity, DialogueComponent drawComponent)
		{
			if (!drawComponent.ShowBox) { return; }
			
			Rectangle dialogueBoxRect = CalculateDialogueBoxDimensions(_viewport.Width, _viewport.Height);

			_spriteBatch.Draw(null, dialogueBoxRect, TextBoxColor);

			DrawDialogueBoxBorder(dialogueBoxRect);

			DrawDialogue(drawComponent.Dialogue, dialogueBoxRect.X, dialogueBoxRect.Y, dialogueBoxRect.Width);

			if (drawComponent.Speaker == null) { return; }

			DrawNameTag(drawComponent.Speaker, dialogueBoxRect);
		}

		private static Rectangle CalculateDialogueBoxDimensions(int screenWidth, int screenHeight)
        {
			int textBoxStartsAt = (int)(screenHeight * (1 - DialogueBoxVerticalScreenPercentage)) + 2 * BorderWidthInPixels;
			int textBoxHeight = (int)(screenHeight * DialogueBoxVerticalScreenPercentage) - 2 * BorderWidthInPixels;

			int dialogueBoxWidth = (int)(screenWidth * DialogueBoxHorizontalScreenPercentage);
			int dialogueBoxOffsetFromScreenSide = (screenWidth - dialogueBoxWidth) / 2;

			return new Rectangle(dialogueBoxOffsetFromScreenSide, textBoxStartsAt, dialogueBoxWidth, textBoxHeight);
		}

		// "in" passes by immutable reference
		// https://docs.microsoft.com/en-us/dotnet/csharp/language-reference/keywords/in-parameter-modifier
		private void DrawNameTag(string name, in Rectangle dialogueBoxRect)
		{
			var speakerSize = GameFonts.NameTag.MeasureString(name);

			int nameTagHeight = (int)(speakerSize.Y * NameTagHeightPercent);

			var nameTagRect = new Rectangle((int)(dialogueBoxRect.X * NameTagXOffsetPercent), dialogueBoxRect.Y - nameTagHeight,
				 (int)(speakerSize.X * NameTagWidthPercent), nameTagHeight);

			_spriteBatch.Draw(null, nameTagRect, TextBoxColor);

			GameFonts.NameTag.DrawString(_spriteBatch, name,
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

		private void DrawDialogue(string dialogue, int dialogueBoxX, int dialogueBoxY, int dialogueBoxWidth)
		{
			int dialoguePadding = BorderWidthInPixels + (int)(dialogueBoxWidth * .01);
			int xDialogueOffset = dialogueBoxX + dialoguePadding;
			int yDialogueOffset = dialogueBoxY + BorderWidthInPixels + 5;

			dialogueBoxWidth -= (dialoguePadding * 2);

			var toWrite = new StringBuilder();

			foreach (var line in dialogue.Split('\n'))
			{
				foreach (var word in line.Split(' ', StringSplitOptions.RemoveEmptyEntries))
				{
					toWrite.Append(word);
					if (GameFonts.Dialogue.MeasureString(toWrite).X > dialogueBoxWidth)
					{
						// length-1 is the last index in the string
						int beforeLastWordIdx = toWrite.Length - 1 - word.Length;

						// Only hyphenate long words
						if (word.Length <= 7)
						{
							toWrite[beforeLastWordIdx] = '\n';
							toWrite.Append(' ');
							continue;
						}

						int tryBreakingWordAtThisIdx = HyphenationGuess(word);
						bool keepTrying = true;
						do
						{
							// +1 because beforeLastWordIdx is a space
							// plus another one because we want to insert after the line break.
							int insertLineBreakAt = beforeLastWordIdx + 2 + tryBreakingWordAtThisIdx;

							bool insertedHyphen = false;
							if (toWrite[insertLineBreakAt - 1] != '-')
							{
								insertedHyphen = true;
								toWrite.Insert(insertLineBreakAt, "-\n");
							}
							else
							{
								toWrite.Insert(insertLineBreakAt, '\n');
							}	

							keepTrying = (GameFonts.Dialogue.MeasureString(toWrite).X > dialogueBoxWidth);
							if (keepTrying)
							{
								toWrite.Remove(insertLineBreakAt, insertedHyphen ? 2 : 1);
								tryBreakingWordAtThisIdx--;
							}

						} while (keepTrying);
					}
					toWrite.Append(' ');
				}

				toWrite.Append('\n');
			}

			GameFonts.Dialogue.DrawString(_spriteBatch, toWrite,
				new Vector2(xDialogueOffset, yDialogueOffset), Color.White);
		}

		private static int HyphenationGuess(string word)
		{
			var hyphenIdx = word.IndexOf('-');
			if (hyphenIdx == -1)
			{
				return word.Length / 2;
			}
			return hyphenIdx;
		}

		private void DrawBorder(in Rectangle bounds, in Color color, bool drawBottom = true)
		{
			// top border
			_spriteBatch.Draw(_colorTexture,
				new Rectangle(bounds.X, bounds.Y, bounds.Width, BorderWidthInPixels),
				color
				);

			// left side
			_spriteBatch.Draw(_colorTexture,
				new Rectangle(bounds.X, bounds.Y, BorderWidthInPixels, bounds.Height),
				color
				);

			if (drawBottom)
			{
				// bottom side
				_spriteBatch.Draw(_colorTexture,
					new Rectangle(bounds.X, bounds.Y + bounds.Height - BorderWidthInPixels, bounds.Width, BorderWidthInPixels),
					color
					);
			}

			// right side
			_spriteBatch.Draw(_colorTexture,
				new Rectangle(bounds.X + bounds.Width - BorderWidthInPixels, bounds.Y, BorderWidthInPixels, bounds.Height),
				color
				);
		}
	}
}

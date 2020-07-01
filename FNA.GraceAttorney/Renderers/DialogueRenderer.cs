using System;
using System.ComponentModel;
using System.Linq;
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

			DrawDialogue(TruncateForAnimation(entity, drawComponent.Dialogue), dialogueBoxRect.X, dialogueBoxRect.Y, dialogueBoxRect.Width);

			if (drawComponent.Speaker == null) { return; }

			DrawNameTag(drawComponent.Speaker, dialogueBoxRect);
		}

		private string TruncateForAnimation(Entity entity, string dialogue)
		{
			if (!HasComponent<AnimatedTextComponent>(entity))
				return dialogue;
			return dialogue.Substring(0, (int)GetComponent<AnimatedTextComponent>(entity).CharactersVisible);
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

		private void DrawBorder(in Rectangle bounds, in Color color, bool drawBottom = true)
		{
			// top border
			_spriteBatch.Draw(_colorTexture,
				new Rectangle(bounds.X, bounds.Y, bounds.Width, BorderWidthInPixels),
				color);

			// left side
			_spriteBatch.Draw(_colorTexture,
				new Rectangle(bounds.X, bounds.Y, BorderWidthInPixels, bounds.Height),
				color);

			if (drawBottom)
			{
				// bottom side
				_spriteBatch.Draw(_colorTexture,
					new Rectangle(bounds.X, bounds.Y + bounds.Height - BorderWidthInPixels, bounds.Width, BorderWidthInPixels),
					color);
			}

			// right side
			_spriteBatch.Draw(_colorTexture,
				new Rectangle(bounds.X + bounds.Width - BorderWidthInPixels, bounds.Y, BorderWidthInPixels, bounds.Height),
				color);
		}

		private void DrawDialogue(string dialogue, int dialogueBoxX, int dialogueBoxY, int dialogueBoxWidth)
		{
			int dialoguePadding = BorderWidthInPixels + (int)(dialogueBoxWidth * .01);

			int xDialogueOffset = dialogueBoxX + dialoguePadding;
			int yDialogueOffset = dialogueBoxY + BorderWidthInPixels + 5;

			GameFonts.Dialogue.DrawString(_spriteBatch, HyphenateAndWrapString(dialogue, dialogueBoxWidth - (dialoguePadding * 2)),
				new Vector2(xDialogueOffset, yDialogueOffset), Color.White);
		}

		// I'm sorry that this is so hairy, but this does a reasonable job breaking up longer words
		// and handles the screen resizing pretty gracefully.
		private static StringBuilder HyphenateAndWrapString(string dialogue, int dialogueBoxWidth)
		{
			var toWrite = new StringBuilder();

			foreach (var line in dialogue.Split('\n'))
			{
				var words = line.Split(' ', StringSplitOptions.RemoveEmptyEntries);
				for (var idx = 0; idx < words.Length; idx++)
				{
					var word = words[idx];
					toWrite.Append(word);

					// if the string is currently too long to fit in the dialogue box, hoo boy.
					if (GameFonts.Dialogue.MeasureString(toWrite).X > dialogueBoxWidth)
					{
						// length-1 is the last index in the string
						// toWrite[beforeLastWordIdx] should always be a space
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
							// plus another one because we want to insert the newline after whatever HyphenationGuess picked.
							// (hyphens go on the earlier of the two lines, 
							// and HyphenationGuess will try to break on a hyphen if there's one in the word)
							int insertLineBreakAt = beforeLastWordIdx + 2 + tryBreakingWordAtThisIdx;

							// if there's already a hyphen in the word, don't add another one
							// this -1 is because, if there's a hyphen, insertLineBreakAt is the index after it, because
							// that's where we want to insert the line break.
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
								// if we have to try again, because the string is too long,
								// undo what we just did.
								toWrite.Remove(insertLineBreakAt, insertedHyphen ? 2 : 1);

								// and try breaking the word earlier.
								tryBreakingWordAtThisIdx--;
							}

						} while (keepTrying);
					}

					// basically, if you append a space to the last word in the line, it leads a bug where,
					// for certain pathological dialogue box sizes, the space makes the string wider than the box,
					// which means that the rest of the function will put each word on its own line.
					if (idx != words.Length - 1)
						toWrite.Append(' ');
				}

				toWrite.Append('\n');
			}

			return toWrite;
		}

		private static int HyphenationGuess(string word)
		{
			var hyphenIdx = word.IndexOf('-');
			if (hyphenIdx == -1)
			{
				return TryPickVowel(word, word.Length / 2) + 1;
			}
			return hyphenIdx;
		}

		// words tend to look better when hyphenated along a vowel. I don't know if this really helps
		// and I might take it out later.
		// if nothing else, it helps keep the hyphenation a little more stable across screen resizes.
		private static readonly char[] _vowels = new[] { 'a', 'e', 'i', 'o', 'u', 'y' };
		private static bool IsVowel(char c)
		{
			foreach (char vowel in _vowels)
			{
				if (c == vowel) { return true; }
			}
			return false;
		}
		private static int TryPickVowel(string word, int idx)
		{
			if (IsVowel(word[idx])) { return idx; }
			if (IsVowel(word[idx-1])) { return idx-1; }
			if (IsVowel(word[idx+1])) { return idx+1; }
			return idx;
		}
	}
}

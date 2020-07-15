using System;
using System.Text;
using Encompass;
using GraceAttorney.Components;
using GraceAttorney.DependencyInjection;
using GraceAttorney.Renderers.Fonts;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GraceAttorney.Renderers
{
	class DialogueRenderer : OrderedRenderer<DialogueComponent>
	{
		private readonly Color TextBoxColor = new Color(0, 0, 0, .9f);
		private readonly Color OuterBorderColor = Color.Gray;
		private readonly Color InnerBorderColor = Color.DarkGray;

		private const float DialogueBoxVerticalScreenPercentage = .30f;
		private const float DialogueBoxHorizontalScreenPercentage = .80f;

		private const float DialogueBoxVerticalOffsetFromGroundPercentage = .05f;

		private const float HorizontalDialoguePadding = .02f;
		private const float VerticalDialoguePadding = .1f;

		private const int BorderWidthInPixels = 3;

		private const float LeftNameTagXOffsetPercent = 1.1f;
		private const float RightNameTagXOffsetPercent = .98f;
		private const float NameTagIsThisPercentWiderThanTheText = 1.30f;
		private const float NameTagIsThisPercentTallerThanTheText = 1.8f;

		private readonly SpriteBatch _spriteBatch;
		private readonly UpdatedSize _viewport;
		private readonly Texture2D _colorTexture;
		public DialogueRenderer(SpriteBatch spriteBatch, UpdatedSize viewport, Texture2D colorTexture)
		{
			_spriteBatch = spriteBatch;
			_viewport = viewport;
			_colorTexture = colorTexture;
		}

		public override void Render(Entity entity, in DialogueComponent drawComponent)
		{
			if (!drawComponent.Display) { return; }

			Rectangle dialogueBoxRect = CalculateDialogueBoxDimensions(_viewport.Width, _viewport.Height);

			_spriteBatch.Draw(null, dialogueBoxRect, TextBoxColor);

			DrawDialogueBoxBorder(dialogueBoxRect);

			if (drawComponent.Justification == JustifyText.Left)
				DrawDialogue(drawComponent.Dialogue, dialogueBoxRect, LengthToTruncateTo(entity), drawComponent.TextColor);
			else if (drawComponent.Justification == JustifyText.Center)
				DrawCenteredDialogue(drawComponent.Dialogue, dialogueBoxRect, LengthToTruncateTo(entity), drawComponent.TextColor);

			if (drawComponent.Speaker == null) { return; }

			DrawNameTag(drawComponent.Speaker, dialogueBoxRect, drawComponent.NameTagLocation);
		}

		private int LengthToTruncateTo(in Entity entity)
		{
			if (!HasComponent<AnimatedTextComponent>(entity))
				return -1;
			return (int)GetComponent<AnimatedTextComponent>(entity).CharactersVisible;
		}

		private static Rectangle CalculateDialogueBoxDimensions(int screenWidth, int screenHeight)
		{
			int textBoxStartsAt = (int)(screenHeight * (1 - DialogueBoxVerticalScreenPercentage)) + 2 * BorderWidthInPixels;
			int textBoxHeight = (int)(screenHeight * DialogueBoxVerticalScreenPercentage - screenHeight * DialogueBoxVerticalOffsetFromGroundPercentage) - 2 * BorderWidthInPixels;

			int dialogueBoxWidth = (int)(screenWidth * DialogueBoxHorizontalScreenPercentage);
			int dialogueBoxOffsetFromScreenSide = (screenWidth - dialogueBoxWidth) / 2;

			return new Rectangle(dialogueBoxOffsetFromScreenSide, textBoxStartsAt, dialogueBoxWidth, textBoxHeight);
		}

		// "in" passes by immutable reference
		// https://docs.microsoft.com/en-us/dotnet/csharp/language-reference/keywords/in-parameter-modifier
		private void DrawNameTag(string name, in Rectangle dialogueBoxRect, NameTagLocation location)
		{
			var speakerSize = GameFonts.NameTag.MeasureString(name);

			// this should probably be based on the width of the screen so long names don't get a weirdly wide name tag.
			int nameTagWidth = (int)(speakerSize.X * NameTagIsThisPercentWiderThanTheText);
			int nameTagHeight = (int)(speakerSize.Y * NameTagIsThisPercentTallerThanTheText);

			var nameTagRect = new Rectangle(NameTagXPosition(location, dialogueBoxRect.X, dialogueBoxRect.Width, nameTagWidth),
				dialogueBoxRect.Y - nameTagHeight,
				nameTagWidth, nameTagHeight);

			_spriteBatch.Draw(null, nameTagRect, TextBoxColor);

			GameFonts.NameTag.DrawString(_spriteBatch, name,
				new Vector2(nameTagRect.X + (nameTagRect.Width - speakerSize.X) / 2, nameTagRect.Y + (nameTagRect.Height - speakerSize.Y) / 2), Color.White);

			DrawBorder(nameTagRect, OuterBorderColor, drawBottom: false);
		}

		private int NameTagXPosition(NameTagLocation location, int dialogueBoxStartX, int dialogueBoxWidth, int nameTagWidth)
		{
			switch (location)
			{
				case NameTagLocation.Center:
					return (_viewport.Width - nameTagWidth) / 2;
				case NameTagLocation.Right:
					return (int)(dialogueBoxStartX + (RightNameTagXOffsetPercent * dialogueBoxWidth) - nameTagWidth);
				case NameTagLocation.Left:
				default:
					return (int)(dialogueBoxStartX * LeftNameTagXOffsetPercent);
			}
		}

		private void DrawDialogueBoxBorder(in Rectangle borderRect)
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

		private static readonly string MaxLineWidth = new string('W', Constants.CharactersPerLine);
		private const float LineWidthLeeway = .05f;
		private float CalculateTextFit(int dialogueBoxWidth)
		{
			var textHeight = GameFonts.Dialogue.MeasureString(MaxLineWidth).X;
			return dialogueBoxWidth / textHeight;
		}

		private void SetFontSize(int dialogueBoxWidth)
		{
			// basically, if the line isn't long enough to fit 40 or so wide characters comfortably, reduce the font size
			// if the line is too long, increase the font size.

			for (var lineWidthFit = CalculateTextFit(dialogueBoxWidth); !(1 < lineWidthFit && lineWidthFit < (1 + LineWidthLeeway)); lineWidthFit = CalculateTextFit(dialogueBoxWidth))
			{
				// if the text is too big, make the font smaller
				if (lineWidthFit < 1) GameFonts.Dialogue.Size--;
				else GameFonts.Dialogue.Size++;
			}
		}

		private void DrawDialogue(string dialogue, in Rectangle dialogueBox, int truncateTo, in Color color)
		{
			CalculateTextBounds(dialogueBox, out var xDialogueOffset, out var yDialogueOffset, out var actualDialogueWidth);

			SetFontSize(actualDialogueWidth);

			var toDisplay = HyphenateAndWrapString(dialogue, actualDialogueWidth);

			if (truncateTo != -1 && truncateTo < toDisplay.Length)
			{
				toDisplay.Remove(truncateTo, toDisplay.Length - truncateTo);
			}

			GameFonts.Dialogue.DrawString(_spriteBatch, toDisplay, new Vector2(xDialogueOffset, yDialogueOffset), color);
			_toWrite.Clear();
		}

		private const float PaddingBetweenCenteredLines = 1.4f;
		private void DrawCenteredDialogue(string dialogue, in Rectangle dialogueBox, int truncateTo, in Color color)
		{
			CalculateTextBounds(dialogueBox, out var xDialogueOffset, out var yDialogueOffset, out var actualDialogueWidth);

			SetFontSize(actualDialogueWidth);

			if (truncateTo != -1 && truncateTo < dialogue.Length)
			{
				dialogue = dialogue.Substring(0, truncateTo);
			}

			float runningYOffset = yDialogueOffset;
			foreach (var line in dialogue.Split('\n'))
			{
				var lineSize = GameFonts.Dialogue.MeasureString(line);
				int centeredX = (int)((actualDialogueWidth - lineSize.X) / 2 + xDialogueOffset);
				GameFonts.Dialogue.DrawString(_spriteBatch, line, new Vector2(centeredX, runningYOffset), color);
				runningYOffset += (lineSize.Y * PaddingBetweenCenteredLines);
			}
		}

		private static void CalculateTextBounds(Rectangle dialogueBox, out int xDialogueOffset, out int yDialogueOffset, out int actualDialogueWidth)
		{
			int dialoguePadding = BorderWidthInPixels + (int)(dialogueBox.Width * HorizontalDialoguePadding);

			xDialogueOffset = dialogueBox.X + dialoguePadding;
			yDialogueOffset = dialogueBox.Y + BorderWidthInPixels + (int)(dialogueBox.Height * VerticalDialoguePadding);
			actualDialogueWidth = dialogueBox.Width - (dialoguePadding * 2);
		}

		// I'm sorry that this is so hairy, but this does a reasonable job breaking up longer words
		// and handles the screen resizing pretty gracefully.

		private static readonly StringBuilder _toWrite = new StringBuilder(); // don't leak a stringbuilder every frame
		private static readonly char[] _space = new char[] { ' ' }; // .net framework 4.6.1 insists
		private static StringBuilder HyphenateAndWrapString(string dialogue, int dialogueBoxWidth)
		{
			foreach (var line in dialogue.Split('\n'))
			{
				var words = line.Split(_space, StringSplitOptions.RemoveEmptyEntries);
				for (var idx = 0; idx < words.Length; idx++)
				{
					var word = words[idx];
					_toWrite.Append(word);

					// if the string is currently too long to fit in the dialogue box, hoo boy.
					if (GameFonts.Dialogue.MeasureString(_toWrite).X > dialogueBoxWidth)
					{
						// length-1 is the last index in the string
						// toWrite[beforeLastWordIdx] should always be a space
						int beforeLastWordIdx = _toWrite.Length - 1 - word.Length;

						// Only hyphenate long words
						if (word.Length <= 7)
						{
							_toWrite[beforeLastWordIdx] = '\n';
							_toWrite.Append(' ');
							continue;
						}

						int tryBreakingWordAtThisIdx = HyphenationGuess(word);
						bool keepTrying;
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
							if (_toWrite[insertLineBreakAt - 1] != '-')
							{
								insertedHyphen = true;
								_toWrite.Insert(insertLineBreakAt, "-\n");
							}
							else
							{
								_toWrite.Insert(insertLineBreakAt, '\n');
							}

							keepTrying = (GameFonts.Dialogue.MeasureString(_toWrite).X > dialogueBoxWidth);
							if (keepTrying)
							{
								// if we have to try again, because the string is too long,
								// undo what we just did.
								_toWrite.Remove(insertLineBreakAt, insertedHyphen ? 2 : 1);

								// and try breaking the word earlier.
								tryBreakingWordAtThisIdx--;
							}

						} while (keepTrying);
					}

					// basically, if you append a space to the last word in the line, it leads a bug where,
					// for certain pathological dialogue box sizes, the space makes the string wider than the box,
					// which means that the rest of the function will put each word on its own line.
					if (idx != words.Length - 1)
						_toWrite.Append(' ');
				}

				_toWrite.Append('\n');
			}

			return _toWrite;
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

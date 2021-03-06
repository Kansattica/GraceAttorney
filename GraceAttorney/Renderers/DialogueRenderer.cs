using System;
using System.Text;
using Encompass;
using GraceAttorney.Components;
using GraceAttorney.DependencyInjection;
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
		private const float DialogueBoxHorizontalScreenPercentage = .70f;

		private const float DialogueBoxVerticalOffsetFromGroundPercentage = .05f;

		private const float HorizontalDialoguePadding = .02f;
		private const float VerticalDialoguePadding = .1f;

		private const int BorderWidthInPixels = 3;

		private const float LeftNameTagXOffsetPercent = 1.1f;
		private const float RightNameTagXOffsetPercent = .98f;
		private const float PadNameTagWidthByThisPercentageOfTheDialogueBox = .03f;
		private const float NameTagIsThisPercentTallerThanTheText = 1.8f;

		private readonly SpriteBatch _spriteBatch;
		private readonly Texture2D _colorTexture;
		private readonly ContentLoader _content;
		private readonly UpdatedSize _screenSize;

		public DialogueRenderer(SpriteBatch spriteBatch, Texture2D colorTexture, UpdatedSize screenSize, ContentLoader content)
		{
			_spriteBatch = spriteBatch;
			_colorTexture = colorTexture;
			_content = content;
			_screenSize = screenSize;
		}

		public override void Render(Entity entity, in DialogueComponent drawComponent)
		{
			if (!drawComponent.Display) { return; }

			Rectangle dialogueBoxRect = CalculateDialogueBoxDimensions(_screenSize.Width, _screenSize.Height);

			_spriteBatch.Draw(_colorTexture, dialogueBoxRect, TextBoxColor);

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
			var speakerSize = _content.NameTag.MeasureString(name);

			// this should probably be based on the width of the screen so long names don't get a weirdly wide name tag.
			int nameTagWidth = (int)(speakerSize.X + (dialogueBoxRect.Width * PadNameTagWidthByThisPercentageOfTheDialogueBox));
			int nameTagHeight = (int)(speakerSize.Y * NameTagIsThisPercentTallerThanTheText);

			var nameTagRect = new Rectangle(NameTagXPosition(location, dialogueBoxRect.X, dialogueBoxRect.Width, nameTagWidth),
				dialogueBoxRect.Y - nameTagHeight,
				nameTagWidth, nameTagHeight);

			_spriteBatch.Draw(_colorTexture, nameTagRect, TextBoxColor);

			_content.NameTag.DrawString(_spriteBatch, name,
				new Vector2(nameTagRect.X + (nameTagRect.Width - speakerSize.X) / 2, nameTagRect.Y + BorderWidthInPixels + (nameTagRect.Height - speakerSize.Y) / 2), Color.White);

			DrawBorder(nameTagRect, OuterBorderColor, drawBottom: false);
		}

		private int NameTagXPosition(NameTagLocation location, int dialogueBoxStartX, int dialogueBoxWidth, int nameTagWidth)
		{
			switch (location)
			{
				case NameTagLocation.Center:
					return (_screenSize.Width - nameTagWidth) / 2;
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

		private void DrawBorder(in Rectangle bounds, Color color, bool drawBottom = true)
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

		private readonly string MaxLineWidth = new string('W', Constants.CharactersPerLine);
		private const float LineWidthLeeway = .05f;
		private const float LineSpacingAsDialogueBoxHeightPercentage = .04f;
		private float CalculateTextFit(int dialogueBoxWidth)
		{
			var textHeight = _content.Dialogue.MeasureString(MaxLineWidth).X;
			return dialogueBoxWidth / textHeight;
		}

		private void SetFontSize(int dialogueBoxWidth, int dialogueBoxHeight)
		{
			// basically, if the line isn't long enough to fit 40 or so wide characters comfortably, reduce the font size
			// if the line is too long, increase the font size.

			// do the same loop thing for the line spacing, maybe?
			_content.Dialogue.LineSpacing = dialogueBoxHeight * LineSpacingAsDialogueBoxHeightPercentage;
			for (var lineWidthFit = CalculateTextFit(dialogueBoxWidth); !(1 < lineWidthFit && lineWidthFit < (1 + LineWidthLeeway)); lineWidthFit = CalculateTextFit(dialogueBoxWidth))
			{
				// if the text is too big, make the font smaller
				if (lineWidthFit < 1) _content.Dialogue.Size--;
				else _content.Dialogue.Size++;
			}

			_content.NameTag.Size = _content.Dialogue.Size;
		}

		private string lastDialogue = null;
		private int lastDialogueWidth = -2;
		private bool ShouldRehyphenate(string dialogue, int dialogueWidth)
		{
			return (dialogueWidth != lastDialogueWidth) || (lastDialogue != dialogue);
		}

		private void DrawDialogue(string dialogue, in Rectangle dialogueBox, int truncateTo, Color color)
		{
			CalculateTextBounds(dialogueBox, out var xDialogueOffset, out var yDialogueOffset, out var actualDialogueWidth);

			// rehyphenating the string every frame used to be the most expensive operation
			// so only rehyphenate if needed

			if (ShouldRehyphenate(dialogue, actualDialogueWidth))
			{
				_toDisplay.Clear();

				SetFontSize(actualDialogueWidth, dialogueBox.Height);

				HyphenateAndWrapString(dialogue, actualDialogueWidth);

				lastDialogue = dialogue;
				lastDialogueWidth = actualDialogueWidth;
			}

			// this allocates on every frame where the text is truncated, but, since we cache the _toDisplay string builder between frames,
			// this is (I believe) the most efficient way to get a substring out of a StringBuilder without modifying it

			if (truncateTo != -1 && truncateTo < _toDisplay.Length)
			{
				_content.Dialogue.DrawString(_spriteBatch, _toDisplay.ToString(0, truncateTo), new Vector2(xDialogueOffset, yDialogueOffset), color);
			}
			else
			{
				_content.Dialogue.DrawString(_spriteBatch, _toDisplay, new Vector2(xDialogueOffset, yDialogueOffset), color);
			}

		}

		private const float PaddingBetweenCenteredLines = 1.4f;
		private void DrawCenteredDialogue(string dialogue, in Rectangle dialogueBox, int truncateTo, Color color)
		{
			CalculateTextBounds(dialogueBox, out var xDialogueOffset, out var yDialogueOffset, out var actualDialogueWidth);

			SetFontSize(actualDialogueWidth, dialogueBox.Height);

			if (truncateTo != -1 && truncateTo < dialogue.Length)
			{
				dialogue = dialogue.Substring(0, truncateTo);
			}

			float runningYOffset = yDialogueOffset;
			foreach (var line in dialogue.Split('\n'))
			{
				var lineSize = _content.Dialogue.MeasureString(line);
				int centeredX = (int)((actualDialogueWidth - lineSize.X) / 2 + xDialogueOffset);
				_content.Dialogue.DrawString(_spriteBatch, line, new Vector2(centeredX, runningYOffset), color);
				runningYOffset += (lineSize.Y * PaddingBetweenCenteredLines);
			}
		}

		private static void CalculateTextBounds(in Rectangle dialogueBox, out int xDialogueOffset, out int yDialogueOffset, out int actualDialogueWidth)
		{
			int dialoguePadding = BorderWidthInPixels + (int)(dialogueBox.Width * HorizontalDialoguePadding);

			xDialogueOffset = dialogueBox.X + dialoguePadding;
			yDialogueOffset = dialogueBox.Y + BorderWidthInPixels + (int)(dialogueBox.Height * VerticalDialoguePadding);
			actualDialogueWidth = dialogueBox.Width - (dialoguePadding * 2);
		}

		// I'm sorry that this is so hairy, but this does a reasonable job breaking up longer words
		// and handles the screen resizing pretty gracefully.

		private static readonly StringBuilder _toDisplay = new StringBuilder(); // don't leak a stringbuilder every frame
		private static readonly char[] _space = new char[] { ' ' }; // .net framework 4.6.1 insists
		private void HyphenateAndWrapString(string dialogue, int dialogueBoxWidth)
		{
			foreach (var line in dialogue.Split('\n'))
			{
				var words = line.Split(_space, StringSplitOptions.RemoveEmptyEntries);
				for (var idx = 0; idx < words.Length; idx++)
				{
					var word = words[idx];
					_toDisplay.Append(word);

					// if the string is currently too long to fit in the dialogue box, hoo boy.
					if (_content.Dialogue.MeasureString(_toDisplay).X > dialogueBoxWidth)
					{
						// length-1 is the last index in the string
						// toWrite[beforeLastWordIdx] should always be a space
						int beforeLastWordIdx = _toDisplay.Length - 1 - word.Length;

						// Only hyphenate long words
						if (word.Length <= 7)
						{
							_toDisplay[beforeLastWordIdx] = '\n';
							_toDisplay.Append(' ');
							continue;
						}

						int tryBreakingWordAtThisIdx = HyphenationGuess(word);
						bool keepTrying;
						do
						{
							// if breaking the word would leave one or two letters alone on the starting line, 
							// just start a new line beforehand. 
							if (tryBreakingWordAtThisIdx <= 2)
							{
								_toDisplay[beforeLastWordIdx] = '\n';
								break;
							}

							// +1 because beforeLastWordIdx is a space
							// plus another one because we want to insert the newline after whatever HyphenationGuess picked.
							// (hyphens go on the earlier of the two lines, 
							// and HyphenationGuess will try to break on a hyphen if there's one in the word)
							int insertLineBreakAt = beforeLastWordIdx + 2 + tryBreakingWordAtThisIdx;

							// if there's already a hyphen in the word, don't add another one
							// this -1 is because, if there's a hyphen, insertLineBreakAt is the index after it, because
							// that's where we want to insert the line break.
							bool insertedHyphen = false;
							if (_toDisplay[insertLineBreakAt - 1] != '-')
							{
								insertedHyphen = true;
								_toDisplay.Insert(insertLineBreakAt, "-\n");
							}
							else
							{
								_toDisplay.Insert(insertLineBreakAt, '\n');
							}

							keepTrying = (_content.Dialogue.MeasureString(_toDisplay).X > dialogueBoxWidth);
							if (keepTrying)
							{
								// if we have to try again, because the string is too long,
								// undo what we just did.
								_toDisplay.Remove(insertLineBreakAt, insertedHyphen ? 2 : 1);

								// and try breaking the word earlier.
								tryBreakingWordAtThisIdx--;
							}

						} while (keepTrying);
					}

					// basically, if you append a space to the last word in the line, it leads a bug where,
					// for certain pathological dialogue box sizes, the space makes the string wider than the box,
					// which means that the rest of the function will put each word on its own line.
					if (idx != words.Length - 1)
						_toDisplay.Append(' ');
				}

				_toDisplay.Append('\n');
			}
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

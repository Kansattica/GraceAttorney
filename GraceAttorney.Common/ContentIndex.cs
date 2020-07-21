using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace GraceAttorney.Common
{
	public class ContentIndex
	{
		public List<ImageResource> Backgrounds { get; set; } = new List<ImageResource>();
		public Dictionary<string, List<ImageResource>> Characters { get; set; } = new Dictionary<string, List<ImageResource>>();

		private const string BackgroundsHeader = "===BACKGROUNDS===";
		private const string CharactersHeader = "===CHARACTERS===";
		private const char Newline = '\n';
		public void Write(Stream toWrite)
		{
			SerializeAndWriteLine(BackgroundsHeader, toWrite);
			foreach (var background in Backgrounds)
			{
				SerializeAndWriteLine(background.ToString(), toWrite);
			}

			SerializeAndWriteLine(CharactersHeader, toWrite);
			foreach (var character in Characters.Values.SelectMany(x => x))
			{
				SerializeAndWriteLine(character.ToString(), toWrite);
			}
		}

		private enum ReadMode { None, Character, Background }

		public static ContentIndex Read(string path)
		{
			var currentMode = ReadMode.None;

			var toReturn = new ContentIndex();
			foreach (var line in File.ReadAllText(path).Split(Newline))
			{
				if (line == BackgroundsHeader) { currentMode = ReadMode.Background; continue; }
				else if (line == CharactersHeader) { currentMode = ReadMode.Character; continue; }
				else if (string.IsNullOrWhiteSpace(line)) { continue; }
				switch (currentMode)
				{
					case ReadMode.None:
						break;
					case ReadMode.Background:
						toReturn.Backgrounds.Add(ImageResource.FromString(line));
						break;
					case ReadMode.Character:
						var resource = ImageResource.FromString(line);
						var characterName = Directory.GetParent(resource.FilePath).Name;
						if (toReturn.Characters.TryGetValue(characterName, out var resources))
						{
							resources.Add(resource);
						}
						else
						{
							toReturn.Characters.Add(characterName, new List<ImageResource> { resource });
						}
						break;
				}

			}

			return toReturn;

		}

		private static void SerializeAndWriteLine(string str, Stream stream)
		{
			var nextLine = Encoding.UTF8.GetBytes(str);
			stream.Write(nextLine, 0, nextLine.Length);
			stream.WriteByte((byte)Newline);
		}
	}

	public class ImageResource : IEquatable<ImageResource>
	{
		public string FilePath { get; set; }
		public string Name { get => Path.GetFileNameWithoutExtension(FilePath); }
		public int Frames { get; set; }
		public int FrameWidth { get; set; }
		public int FrameHeight { get; set; }

		public override string ToString()
		{
			return $"{FilePath};{Frames};{FrameWidth};{FrameHeight}";
		}

		public static ImageResource FromString(string str)
		{
			var split = str.Split(';');
			return new ImageResource
			{
				FilePath = split[0],
				Frames = int.Parse(split[1]),
				FrameWidth = int.Parse(split[2]),
				FrameHeight = int.Parse(split[3]),
			};
		}

		public override bool Equals(object obj)
		{
			return Equals(obj as ImageResource);
		}

		public bool Equals(ImageResource other)
		{
			return other != null &&
				   FilePath == other.FilePath;
		}

		public override int GetHashCode()
		{
			return 1230029444 + EqualityComparer<string>.Default.GetHashCode(FilePath);
		}

		public static bool operator ==(ImageResource left, ImageResource right)
		{
			return EqualityComparer<ImageResource>.Default.Equals(left, right);
		}

		public static bool operator !=(ImageResource left, ImageResource right)
		{
			return !(left == right);
		}
	}
}

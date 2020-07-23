using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;

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
			foreach (var character in Characters)
			{
				foreach (var pose in character.Value)
				{
					pose.Character = character.Key;
					SerializeAndWriteLine(pose.ToString(), toWrite);
				}
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
						if (toReturn.Characters.TryGetValue(resource.Character, out var resources))
						{
							resources.Add(resource);
						}
						else
						{
							toReturn.Characters.Add(resource.Character, new List<ImageResource> { resource });
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
		public string Character { get; set; } = "";
		public string Name { get => Path.GetFileNameWithoutExtension(FilePath); }
		public int Frames { get; set; }
		public int FrameWidth { get; set; }
		public int FrameHeight { get; set; }
		public int StartsAtX { get; set; } = 0;
		public int StartsAtY { get; set; } = 0;

		public override string ToString()
		{
			return $"{NormalizePathSeparators(FilePath)};{Character};{Frames};{FrameWidth};{FrameHeight};{StartsAtX};{StartsAtY}";
		}

		public static ImageResource FromString(string str)
		{
			var split = str.Split(';');
			return new ImageResource
			{
				FilePath = split[0],
				Character = split[1],
				Frames = int.Parse(split[2]),
				FrameWidth = int.Parse(split[3]),
				FrameHeight = int.Parse(split[4]),
				StartsAtX = int.Parse(split[5]),
				StartsAtY = int.Parse(split[6])
			};
		}

		// basically, you can use forward slashes to separate directories on any platform (including Windows!)
		// but backslashes are a Windowsism
		private static string NormalizePathSeparators(string path) => path.Replace('\\', '/');

		public override bool Equals(object obj)
		{
			return Equals(obj as ImageResource);
		}

		public bool Equals(ImageResource other)
		{
			return other != null &&
				   FilePath == other.FilePath &&
				   StartsAtX == other.StartsAtX &&
				   StartsAtY == other.StartsAtY;
		}

		public override int GetHashCode()
		{
			var hashCode = -1237577697;
			hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(FilePath);
			hashCode = hashCode * -1521134295 + StartsAtX.GetHashCode();
			hashCode = hashCode * -1521134295 + StartsAtY.GetHashCode();
			return hashCode;
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

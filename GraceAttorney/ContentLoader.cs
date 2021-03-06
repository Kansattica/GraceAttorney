using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using GraceAttorney.Common;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using SpriteFontPlus;

namespace GraceAttorney
{
	class ContentLoader 
	{
		private readonly Dictionary<string, CaseSprite> _backgrounds;
		private readonly Dictionary<string, Dictionary<string, CaseSprite>> _characters;

		public readonly DynamicSpriteFont Dialogue;
		public readonly DynamicSpriteFont NameTag;

		public ContentLoader(ContentManager content, string caseDirectory)
		{
			var index = ContentIndex.Read(Path.Combine(content.RootDirectory, caseDirectory, Common.Constants.IndexFileName));

			_backgrounds = index.Backgrounds.ToDictionary(x => x.Name, x => new CaseSprite(x, content));

			_characters = index.Characters
				.ToDictionary(x => x.Key, x => x.Value.ToDictionary(pose => pose.Name, pose => new CaseSprite(pose, content)));

			// probably put fonts in the index later
			Dialogue = DynamicSpriteFont.FromTtf(File.ReadAllBytes(Path.Combine(content.RootDirectory, "Fonts", "Dialogue.ttf")), 24);
			NameTag = DynamicSpriteFont.FromTtf(File.ReadAllBytes(Path.Combine(content.RootDirectory, "Fonts", "NameTag.ttf")), 24);
		}

		public bool BackgroundExists(string name)
		{
			return _backgrounds.ContainsKey(name);
		}

		public CaseSprite GetBackground(string name)
		{
			return _backgrounds[name];
		}

		public bool SpritePoseExists(string characterName, string pose)
		{
			if(_characters.TryGetValue(characterName, out var poses))
				return poses.ContainsKey(pose);
			return false;
		}

		public CaseSprite GetSpritePose(string characterName, string pose)
		{
			return _characters[characterName][pose];
		}
	}

	class CaseSprite : IEquatable<CaseSprite>
	{
		public Texture2D Sprite;
		public int Frames;
		public int FrameWidth;
		public int FrameHeight;

		private readonly string path;
		public CaseSprite(ImageResource resource, ContentManager content)
		{
			Sprite = content.Load<Texture2D>(resource.FilePath);
			Frames = resource.Frames;
			FrameWidth = resource.FrameWidth;
			FrameHeight = resource.FrameHeight;

			path = resource.FilePath;
		}

		public override bool Equals(object obj)
		{
			return Equals(obj as CaseSprite);
		}

		public bool Equals(CaseSprite other)
		{
			return other != null &&
				   path == other.path;
		}

		public override int GetHashCode()
		{
			return HashCode.Combine(path);
		}
	}
}

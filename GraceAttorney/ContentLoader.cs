using System;
using System.Collections.Immutable;
using System.IO;
using GraceAttorney.Common;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace GraceAttorney
{
	class ContentLoader : IDisposable
	{
		private readonly ContentManager _content;

		private readonly ImmutableDictionary<string, CaseSprite> _backgrounds;
		private readonly ImmutableDictionary<string, ImmutableDictionary<string, CaseSprite>> _characters;

		private bool disposedValue;

		public ContentLoader(ContentManager content, string caseDirectory)
		{
			_content = content;

			var index = ContentIndex.Read(Path.Combine(content.RootDirectory, caseDirectory, Common.Constants.IndexFileName));

			_backgrounds = index.Backgrounds.ToImmutableDictionary(x => x.Name, x => new CaseSprite(x, content));

			_characters = index.Characters
				.ToImmutableDictionary(x => x.Key, x => x.Value.ToImmutableDictionary(pose => pose.Name, pose => new CaseSprite(pose, content)));
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
			return _characters[characterName].ContainsKey(pose);
		}

		public CaseSprite GetSpritePose(string characterName, string pose)
		{
			return _characters[characterName][pose];
		}

		protected virtual void Dispose(bool disposing)
		{
			if (!disposedValue)
			{
				if (disposing)
				{
					// TODO: dispose managed state (managed objects)
					//foreach (var texture in _loadedSprites.Values)
					//	texture.Dispose();

					_content.Unload();
					_content.Dispose();
				}

				// TODO: free unmanaged resources (unmanaged objects) and override finalizer
				// TODO: set large fields to null
				disposedValue = true;
			}
		}

		// // TODO: override finalizer only if 'Dispose(bool disposing)' has code to free unmanaged resources
		// ~OnDemandContentLoader()
		// {
		//     // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
		//     Dispose(disposing: false);
		// }

		public void Dispose()
		{
			// Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
			Dispose(disposing: true);
			GC.SuppressFinalize(this);
		}
	}

	class CaseSprite
	{
		public Texture2D Sprite;
		public int Frames;
		public int FrameWidth;
		public int FrameHeight;

		public CaseSprite(ImageResource resource, ContentManager content)
		{
			Sprite = content.Load<Texture2D>(resource.FilePath);
			Frames = resource.Frames;
			FrameWidth = resource.FrameWidth;
			FrameHeight = resource.FrameHeight;
		}
	}
}

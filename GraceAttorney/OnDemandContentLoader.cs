using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace GraceAttorney
{
	class OnDemandContentLoader
	{
		private readonly ContentManager _content;

		private readonly Dictionary<string, Texture2D> _loadedSprites = new Dictionary<string, Texture2D>();

		public OnDemandContentLoader(ContentManager content)
		{
			_content = content;
		}

		public Texture2D GetSpriteByPath(string path)
		{
			return GetOrLoad(path);
		}

		private Texture2D GetOrLoad(string path)
		{
			if (!_loadedSprites.TryGetValue(path, out var texture))
			{
				texture = _content.Load<Texture2D>(path);
				_loadedSprites.Add(path, texture);
			}
			return texture;
		}
	}
}

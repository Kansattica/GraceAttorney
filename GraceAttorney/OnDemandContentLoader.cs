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
	class OnDemandContentLoader : IDisposable
	{
		private readonly ContentManager _content;

		private readonly Dictionary<string, Texture2D> _loadedSprites = new Dictionary<string, Texture2D>();
		private bool disposedValue;

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

		protected virtual void Dispose(bool disposing)
		{
			if (!disposedValue)
			{
				if (disposing)
				{
					// TODO: dispose managed state (managed objects)
					foreach (var texture in _loadedSprites.Values)
					{
						texture.Dispose();
					}
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
}

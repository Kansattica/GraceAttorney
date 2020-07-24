using System;

namespace GraceAttorney.Common
{
    public static class Constants
    {
		// I think I'd like the final product to be 4K
		// (3840x2160), but this is fine for development
		// also, it's possible to dynamically scale everything
		// off the size of the current background, but
		// I think that's going to cause more problems than it solves
		public const int BackgroundHeightInPixels = 1080;
		public const int BackgroundWidthInPixels = 1920;

		// dank said that this is true for FNA
		// the XNA limit is 4096 * 4096
		public const int MaximumTextureSize = 8192 * 8192;

		public const string IndexFileName = "index.txt";
    }
}

using System;

namespace GraceAttorney.Common
{
    public static class Constants
    {
		public const int BackgroundHeightInPixels = 1080;
		public const int BackgroundWidthInPixels = 1920;

		// dank said that this is true for FNA
		// the XNA limit is 4096 * 4096
		public const int MaximumTextureSize = 8192 * 8192;

		public const string IndexFileName = "index.txt";
    }
}

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using GraceAttorney.Common;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;

namespace GraceAttorney.AssetCompiler
{
	class AssetMover
	{
		private readonly string _contentRootDir;
		private readonly string _targetContentDir;
		public AssetMover(string from, string to)
		{
			if (!Directory.Exists(from)) { throw new ArgumentException($"{from} must be a directory."); }

			Directory.CreateDirectory(to);
			if (!Directory.Exists(to)) { throw new ArgumentException($"{to} must be a directory."); }

			_contentRootDir = from;
			_targetContentDir = to;

			if (Path.GetFileName(_targetContentDir) != "Content")
			{
				_targetContentDir = Path.Combine(_targetContentDir, "Content");
			}
		}

		private const string BackgroundsName = "Backgrounds";
		private const string CharactersName = "Characters";
		private const string FontsName = "Fonts";

		public void Compile()
		{
			Directory.CreateDirectory(_targetContentDir);
			foreach (var itemPath in Directory.EnumerateFileSystemEntries(_contentRootDir))
			{
				if (Path.GetFileName(itemPath) == FontsName)
					CopyDirIfNewerAndExists(itemPath, Path.Combine(_targetContentDir, FontsName));
				else
					ProcessCase(itemPath);
			}
		}

		private void ProcessCase(string caseDirectory)
		{
			//cases have Backgrounds and Characters
			//and Characters are the ones that need the most special treatment

			var targetCaseDir = Path.Combine(_targetContentDir, Path.GetFileName(caseDirectory));

			Console.WriteLine("Compiling assets from {0} to {1}", caseDirectory, targetCaseDir);

			var contentIndex = new ContentIndex()
			{
				Backgrounds = ProcessResourceDirectory(Path.Combine(caseDirectory, BackgroundsName), Path.Combine(targetCaseDir, BackgroundsName)).ToList()
			};

			var characterDir = Path.Combine(caseDirectory, CharactersName);
			var characterTargetDir = Path.Combine(targetCaseDir, CharactersName);
			foreach (var character in Directory.EnumerateFileSystemEntries(characterDir))
			{
				var characterName = Path.GetFileName(character);
				contentIndex.Characters.Add(characterName, ProcessResourceDirectory(character, Path.Combine(characterTargetDir, characterName)).ToList());
			}

			using (var indexFile = File.Open(Path.Combine(targetCaseDir, "index.txt"), FileMode.Create))
				contentIndex.Write(indexFile);
		}

		// Content/Case1/Backgrounds and Content/Case1/CharacterName
		private IEnumerable<ImageResource> ProcessResourceDirectory(string srcPath, string destPath)
		{
			Directory.CreateDirectory(destPath);
			foreach (var path in Directory.EnumerateFileSystemEntries(srcPath))
			{
				Console.WriteLine("Processing the path {0}", path);
				if (Directory.Exists(path))
					yield return ProcessAnimatedImage(path, destPath);
				else
					yield return ProcessSingleImage(path, destPath);
			}
		}

		private ImageResource ProcessSingleImage(string srcPath, string destPath)
		{
			destPath = Path.Combine(destPath, Path.GetFileName(srcPath));
			var resource = new ImageResource()
			{
				Frames = 1,
				// length + 1 because we want to remove the leftover slash 
				FilePath = destPath.Remove(0, _targetContentDir.Length + 1)
			};

			using (Image image = Image.Load(srcPath))
			{
				resource.FrameHeight = image.Height;
				resource.FrameWidth = image.Width;
			}

			CopyIfNewer(srcPath, destPath);

			return resource;
		}

		private ImageResource ProcessAnimatedImage(string srcPath, string destPath)
		{
			var frames = Directory.EnumerateFiles(srcPath).OrderBy(x => x).Select(x => Image.Load(x)).ToArray();

			var (Width, Height) = frames.Aggregate((Width: 0, Height: 0), (acc, img)
				=> (img.Width > acc.Width ? img.Width : acc.Width, img.Height > acc.Height ? img.Height : acc.Height));

			var newestFile = Directory.EnumerateFiles(srcPath).Max(File.GetLastWriteTimeUtc);

			var targetPath = Path.Combine(destPath, Path.GetFileName(srcPath) + ".png");

			Console.WriteLine("Checking animated sprite in directory {0} to {1}. It has {2} frames, and each frame is {3}x{4}.",
				srcPath, targetPath, frames.Length, Width, Height);

			if (!File.Exists(targetPath) || newestFile > File.GetLastWriteTimeUtc(targetPath))
			{
				Console.WriteLine("Change detected. Processing animated sprite in directory {0} to {1}. It has {2} frames, and each frame is {3}x{4}.",
					srcPath, targetPath, frames.Length, Width, Height);

				using (var targetImage = new Image<Rgba32>(Width * frames.Length, Height))
				{
					Point drawAt = Point.Empty;
					foreach (var frame in frames)
					{
						frame.Mutate(x => x.Pad(Width, Height));

						targetImage.Mutate(x => x.DrawImage(frame, drawAt, PixelColorBlendingMode.Normal, 1f));

						drawAt.X += Width;
						frame.Dispose();
					}

					using (var fs = File.OpenWrite(targetPath))
						targetImage.SaveAsPng(fs);
				}
			}

			return new ImageResource()
			{
				FilePath = targetPath.Remove(0, _targetContentDir.Length + 1),
				FrameHeight = Height,
				FrameWidth = Width,
				Frames = frames.Length
			};
		}

		private void CopyDirIfNewerAndExists(string from, string to)
		{
			Console.WriteLine("Copying directory {0} to {1}", from, to);
			if (!Directory.Exists(from)) { return; }

			Directory.CreateDirectory(to);

			foreach (var entry in Directory.EnumerateFileSystemEntries(from))
			{
				if (Directory.Exists(entry))
				{
					CopyDirIfNewerAndExists(entry, Path.Combine(to, Path.GetDirectoryName(entry)));
					continue;
				}

				CopyIfNewer(entry, Path.Combine(to, Path.GetFileName(entry)));
			}
		}

		private void CopyIfNewer(string from, string to)
		{
			if (ShouldCopy(from, to))
			{
				Console.WriteLine("Copying {0} to {1}.", from, to);
				File.Copy(from, to, true);
			}
			else
			{
				Console.WriteLine("Not copying {0} to {1} because {1} is up to date.", from, to);
			}
		}

		private static bool ShouldCopy(string from, string to)
		{
			return File.GetLastWriteTimeUtc(from) > File.GetLastWriteTimeUtc(to);
		}
	}
}

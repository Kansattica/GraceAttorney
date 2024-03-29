using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using GraceAttorney.Common;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats.Png;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;

namespace GraceAttorney.AssetCompiler
{
	class AssetMover
	{
		private readonly string _contentRootDir;
		private readonly string _targetContentDir;

		private readonly bool _quietMode;

		private bool _changeDetected = false;

		public AssetMover(string from, string to, bool quietMode = false)
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

			_quietMode = quietMode;

			Configuration.Default.ImageFormatsManager.SetEncoder(PngFormat.Instance, new PngEncoder
			{
				CompressionLevel = PngCompressionLevel.BestCompression,
				ChunkFilter = PngChunkFilter.ExcludeAll,
				FilterMethod = PngFilterMethod.Adaptive,
				IgnoreMetadata = true
			});
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

			if (!_quietMode)
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

			if (!_changeDetected) { return; }

			var indexPath = Path.Combine(targetCaseDir, Constants.IndexFileName);
			Console.WriteLine("Writing {0} because something changed.", indexPath);
			using (var indexFile = File.Open(indexPath, FileMode.Create))
				contentIndex.Write(indexFile);
		}

		// Content/Case1/Backgrounds and Content/Case1/CharacterName
		private IEnumerable<ImageResource> ProcessResourceDirectory(string srcPath, string destPath)
		{
			Directory.CreateDirectory(destPath);
			foreach (var path in Directory.EnumerateFileSystemEntries(srcPath))
			{
				if (!_quietMode)
					Console.WriteLine("Processing the path {0}", path);

				if (Directory.Exists(path))
					yield return AnimatedImageInDirectory(path, destPath);
				else if (path.EndsWith(".gif"))
					yield return AnimatedGif(path, destPath);
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

			WarnIfImageTooBig(srcPath, destPath);

			return resource;
		}

		private ImageResource AnimatedImageInDirectory(string srcPath, string destPath)
		{
			var frames = Directory.EnumerateFiles(srcPath).OrderBy(x => x).Select(x => Image.Load(x)).ToArray();
			var newestFile = Directory.EnumerateFiles(srcPath).Max(File.GetLastWriteTimeUtc);
			var targetPath = Path.Combine(destPath, Path.GetFileName(srcPath) + ".png");
			bool shouldWrite = !File.Exists(targetPath) || newestFile > File.GetLastWriteTimeUtc(targetPath);

			return ProcessAnimatedImage(frames, targetPath, shouldWrite);
		}

		private ImageResource AnimatedGif(string srcPath, string destPath)
		{
			var targetPath = Path.Combine(destPath, Path.GetFileNameWithoutExtension(srcPath) + ".png");
			using (Image inputGif = Image.Load(srcPath))
			{
				return ProcessAnimatedImage(Enumerable.Range(0, inputGif.Frames.Count).Select(x => inputGif.Frames.CloneFrame(x)).ToArray(),
					targetPath, ShouldCopy(srcPath, targetPath));
			}

		}

		private ImageResource ProcessAnimatedImage(Image[] frames, string targetPath, bool shouldWrite)
		{
			var (Width, Height) = frames.Aggregate((Width: 0, Height: 0), (acc, img)
				=> (img.Width > acc.Width ? img.Width : acc.Width, img.Height > acc.Height ? img.Height : acc.Height));

			if (shouldWrite)
			{
				_changeDetected = true;
				Console.WriteLine("Change detected. Saving animated sprite to {0}. It has {1} frames, and each frame is {2}x{3}.",
					targetPath, frames.Length, Width, Height);

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

					WarnIfImageTooBig(targetImage, targetPath);

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

		private void WarnIfImageTooBig(Image image, string targetPath)
		{
			var imageSize = image.Width * image.Height;
			if (imageSize > Constants.MaximumTextureSize)
				PrintWarning($"{targetPath} is {imageSize} pixels in total, which is larger than larger than FNA's {Constants.MaximumTextureSize}-pixel limit on texture sizes.");
		}

		private void WarnIfImageTooBig(string pathToCheck, string targetPath)
		{
			var metadata = Image.Identify(pathToCheck);
			var imageSize = metadata.Width * metadata.Height;
			if (imageSize > Constants.MaximumTextureSize)
				PrintWarning($"{targetPath} is {imageSize} pixels in total, which is larger than larger than FNA's {Constants.MaximumTextureSize}-pixel limit on texture sizes.");
			// add something to yell or fix it if the image isn't RGBA, since I think that's the only kind of PNG FNA likes
		}

		private void CopyDirIfNewerAndExists(string from, string to)
		{
			if (!_quietMode)
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
				_changeDetected = true;
				Console.WriteLine("Copying {0} to {1}.", from, to);
				File.Copy(from, to, true);
			}
			else
			{
				if (!_quietMode)
					Console.WriteLine("Not copying {0} to {1} because {1} is up to date.", from, to);
			}
		}

		private static bool ShouldCopy(string from, string to)
		{
			return File.GetLastWriteTimeUtc(from) > File.GetLastWriteTimeUtc(to);
		}

		private static void PrintWarning(string message)
		{
			Console.ForegroundColor = ConsoleColor.Yellow;
			Console.WriteLine("WARNING: {0}", message);
			Console.ResetColor();
		}
	}
}

using Microsoft.VisualStudio.TestTools.UnitTesting;
using GraceAttorney.Common;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.IO;
using System.Linq;

namespace GraceAttorney.Tests
{
    [TestClass]
    public class ContentIndexTests
    {
        [TestMethod]
        public void RoundTrip()
        {
			var index = new ContentIndex
			{
				Backgrounds = new List<ImageResource>
			{
				new ImageResource
				{
					FilePath = "Backgrounds/path.png",
					FrameHeight = 165,
					FrameWidth = 1235,
					Frames = 12
				},
				new ImageResource
				{
					FilePath = "Backgrounds/file.png",
					FrameHeight = 10,
					FrameWidth = 12,
					Frames = 1
				},
				new ImageResource
				{
					FilePath = "Backgrounds/background.png",
					FrameHeight = 1080,
					FrameWidth = 1920,
					Frames = 1
				}
			},

				Characters = new Dictionary<string, List<ImageResource>>
			{
				{ "Cool Guy", new List<ImageResource>
					{
						new ImageResource
						{
							FilePath = "Cool Guy/standing.png",
							FrameHeight = 100,
							FrameWidth = 200,
							Frames = 1,
							StartsAtX = 69,
							StartsAtY = 128
						},
						new ImageResource
						{
							FilePath = "Cool Guy/jumping.png",
							FrameHeight = 300,
							FrameWidth = 205,
							Frames = 5,
							StartsAtX = 0,
							StartsAtY = 165
						}
					}
				},
				{ "Cooler Guy", new List<ImageResource>
					{
						new ImageResource
						{
							FilePath = "Cooler Guy/notstanding.png",
							FrameHeight = 1001,
							FrameWidth = 2002,
							Frames = 1,
							StartsAtX = 2,
							StartsAtY = 22
						},
						new ImageResource
						{
							FilePath = "Cooler Guy/jumpingbetter.png",
							FrameHeight = 3004,
							FrameWidth = 2051,
							Frames = 62
						}
					}
				},
			}
			};

			var testPath = Path.GetTempFileName();
			using (var outfile = File.OpenWrite(testPath))
			{
				index.Write(outfile);
			}

			Assert.AreEqual("===BACKGROUNDS===\nBackgrounds/path.png;;12;1235;165;0;0\nBackgrounds/file.png;;1;12;10;0;0\nBackgrounds/background.png;;1;1920;1080;0;0\n===CHARACTERS===\nCool Guy/standing.png;Cool Guy;1;200;100;69;128\nCool Guy/jumping.png;Cool Guy;5;205;300;0;165\nCooler Guy/notstanding.png;Cooler Guy;1;2002;1001;2;22\nCooler Guy/jumpingbetter.png;Cooler Guy;62;2051;3004;0;0\n",
				File.ReadAllText(testPath));

			var actual = ContentIndex.Read(testPath);
			File.Delete(testPath);

			Assert.AreEqual(index.Backgrounds.Count, actual.Backgrounds.Count);
			foreach (var backgrounds in index.Backgrounds.Zip(actual.Backgrounds))
			{
				Assert.AreEqual(backgrounds.First, backgrounds.Second);
			}

			Assert.AreEqual(index.Characters.Count, actual.Characters.Count);
			foreach (var kvp in index.Characters)
			{
				var actualPoses = actual.Characters[kvp.Key];
				foreach (var pose in kvp.Value.Zip(actualPoses))
				{
					Assert.AreEqual(pose.First, pose.Second);
					Assert.AreEqual(Path.GetDirectoryName(pose.Second.FilePath), pose.Second.Character);
				}
			}
		}
		
		[TestMethod]
		public void NormalizesDirectorySeparators()
		{
			var index = new ContentIndex
			{
				Backgrounds = new List<ImageResource>()
				{
					new ImageResource
					{
						FilePath = @"With\Both/Kinds/Of\Slashes.png"
					}
				},

				Characters = new Dictionary<string, List<ImageResource>>
				{
					{ "Cool Friend", new List<ImageResource>
						{
							new ImageResource
							{
								FilePath = @"With\Just\Backslashes\And Spaces!.png"
							},
							new ImageResource
							{
								FilePath = @"With/Forward/Slashes/and aspace.jpeg"
							}
						}
					}
				}
			};


			var testPath = Path.GetTempFileName();
			using (var outfile = File.OpenWrite(testPath))
			{
				index.Write(outfile);
			}

			var newIndex = ContentIndex.Read(testPath);

			Assert.AreEqual("With/Both/Kinds/Of/Slashes.png", newIndex.Backgrounds.Single().FilePath);
			Assert.AreEqual("With/Just/Backslashes/And Spaces!.png", newIndex.Characters["Cool Friend"][0].FilePath);
			Assert.AreEqual("With/Forward/Slashes/and aspace.jpeg", newIndex.Characters["Cool Friend"][1].FilePath);
		}
	}
}

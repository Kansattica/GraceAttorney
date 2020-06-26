using System;
using System.IO;
using System.Transactions;
using Ink.Runtime;

namespace InkTests
{
	class Program
	{
		static void Main(string[] args)
		{
			var story = new Story(File.ReadAllText("Ink/gay.ink.json"));

			while(story.canContinue)
			{
				var nextLine = story.Continue().Split('*', StringSplitOptions.None);

				using (var c = new ConsoleColorer(story.currentTags.Contains("red") ? ConsoleColor.Red : new ConsoleColor?()))
				{
					for (var i = 0; i < nextLine.Length; i++)
					{
						using (var cc = new ConsoleColorer(i % 2 == 1 ? ConsoleColor.Blue : new ConsoleColor?(), c.Current))
						{
							Console.Write(nextLine[i]);
						}
					}
				}
			}
		}
	}

	class ConsoleColorer : IDisposable
	{
		public ConsoleColor Current { get; private set; }
		private readonly ConsoleColor _changeBack;

		public ConsoleColorer(ConsoleColor? changeTo, ConsoleColor changeBack = ConsoleColor.Gray)
		{
			if (changeTo.HasValue)
				Console.ForegroundColor = Current = changeTo.Value;
			else
				Current = Console.ForegroundColor;
			_changeBack = changeBack;
		}


		public void Dispose()
		{
			Console.ForegroundColor = _changeBack;
		}
	}
}

using System;
using System.IO;
using System.Linq;

namespace GraceAttorney.AssetCompiler
{
    class Program
    {
        static void Main(string[] args)
        {
			Console.WriteLine($"Compiling assets from {args[0]} to {args[1]}.");
			new AssetMover(args[0], args[1]).Compile();
        }
    }
}

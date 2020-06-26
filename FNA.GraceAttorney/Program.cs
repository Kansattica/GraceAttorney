using System;

namespace FNA.GraceAttorney
{
    class Program
    {
        static void Main(string[] args)
        {
			using (GraceAttorneyGame g = new GraceAttorneyGame())
			{
				g.Run();
			}
		}
	}
}

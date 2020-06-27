using System;

namespace FNA.GraceAttorney
{
    class Program
    {
        static void Main(string[] args)
        {
			Environment.SetEnvironmentVariable("FNA_KEYBOARD_USE_SCANCODES", "1");
			using (GraceAttorneyGame g = new GraceAttorneyGame())
			{
				g.Run();
			}
		}
	}
}

using System;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;

namespace GraceAttorney
{
    class Program
    {
		[DllImport("kernel32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
		[return: MarshalAs(UnmanagedType.Bool)]
		static extern bool SetDllDirectory(string lpPathName);

		static void Main(string[] args)
		{
#if NETCOREAPP
            DllMap.Initialise(false);
#endif
			// https://github.com/FNA-XNA/FNA/wiki/4:-FNA-and-Windows-API#64-bit-support
			if (Environment.OSVersion.Platform == PlatformID.Win32NT)
			{
				SetDllDirectory(Path.Combine(
					AppDomain.CurrentDomain.BaseDirectory,
					Environment.Is64BitProcess ? "x64" : "x86"
				));
			}

			// https://github.com/FNA-XNA/FNA/wiki/7:-FNA-Environment-Variables#fna_graphics_enable_highdpi
			// NOTE: from documentation: 
			//       Lastly, when packaging for macOS, be sure this is in your app bundle's Info.plist:
			//           <key>NSHighResolutionCapable</key>
			//           <string>True</string>
			Environment.SetEnvironmentVariable("FNA_GRAPHICS_ENABLE_HIGHDPI", "1");
			Environment.SetEnvironmentVariable("FNA_KEYBOARD_USE_SCANCODES", "1");

			using (GraceAttorneyGame game = new GraceAttorneyGame())
			{
				bool isHighDPI = Environment.GetEnvironmentVariable("FNA_GRAPHICS_ENABLE_HIGHDPI") == "1";
				if (isHighDPI)
					Debug.WriteLine("HiDPI Enabled");

				game.Run();
			}
		}
	}
}

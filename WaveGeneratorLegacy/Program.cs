using TartarosLogger;

namespace WaveGeneratorLegacy
{
	internal class Program
	{
		private static void Init()
		{
			// Set default console colors
			Console.BackgroundColor = ConsoleColor.Black;
			Console.ForegroundColor = ConsoleColor.White;
		}

		static void Main(string[] args)
		{
			Logger.WriteLogInFile = false;
			if (args.Length > 0)
			{
				if (args[0] == "--debug")
				{
					Logger.DebugEnabled = true;
					Logger.Debug("Debug is enabled");
				}
			}
			Logger.Info($"TartarosLogger Version:\t{Logger.Version}");

			Init();

			Logger.Info("Init finished");
			Thread.Sleep(2000);
			UserInterface.Start();
		}
	}
}

namespace WaveGeneratorLegacy
{
	class UserInterface
	{
		private static ClientSocket? client;

		private static string SetupIP()
		{
			string? input;
			Console.Title = "WaveGenerator - Input IP";
			Console.Write("\n\nWaveGenerator IP Address: 192.168.1.");
			input = Console.ReadLine();
			if (!string.IsNullOrEmpty(input))
			{
				if (input.Length < 3)
				{
					return "192.168.1." + input;
				}
			}
			return "192.168.1.2";
		}

		public static void Start()
		{
			// Setup IP Adress and port
			string? menuInput;
			string ipAddress = SetupIP();
			client = new(ipAddress, 5025);
			client.SendData("DISP:TEXT 'Select program!'");

			Console.Title = "WaveGenerator - MainMenu";
			Console.Clear();
			Console.WriteLine("Select option below\n");
			Console.WriteLine("> 85/11737\t[ 1 ]");
			Console.WriteLine("> XX/XXXXX\t[ 2 ]");
			Console.WriteLine("> XX/XXXXX\t[ 3 ]");
			Console.WriteLine("> XX/XXXXX\t[ 4 ]");
			Console.WriteLine("> XX/XXXXX\t[ 5 ]");

			Console.SetCursorPosition(0, Console.WindowHeight - 1);
			Console.Write(">");
			menuInput = Console.ReadLine();

			if (!string.IsNullOrEmpty(menuInput))
			{
				if (menuInput.Equals("1"))
				{
					OpenPreAmp();
					return;
				}
			}
			else
			{
				Console.WriteLine($"Option {menuInput} not found");
			}
		}

		private static void OpenPreAmp()
		{
			List<int> FreqValues = new List<int>
			{ 1, 10, 40, 80, 100};

			int selectedOption = 0;

			if (client != null)
			{
				client.SendData("FUNC SQU");
				client.SendData("FUNC:SQU:DCYC +50.0");
				client.SendData("FREQ +1000.0");

				client.SendData("SOUR:VOLT +2.0");
				client.SendData("VOLT:OFFS +0.0");

				client.SendData("OUTP 1");
			}

			Console.Title = "WaveGenerator - Pre-Amp";
			Console.Clear();
			Console.CursorVisible = false;

			while (true)
			{
				Console.Clear();
				Console.WriteLine("To exit program, press backspace!\n");
				for (int i = 0; i < FreqValues.Count; i++)
				{
					if (selectedOption == i)
					{
						Console.BackgroundColor = ConsoleColor.White;
						Console.ForegroundColor = ConsoleColor.Black;
						Console.WriteLine("Freq: " + FreqValues[i] + "kHz");
						Console.BackgroundColor = ConsoleColor.Black;
						Console.ForegroundColor = ConsoleColor.White;

						if (client != null)
						{
							client.SendData($"DISP:TEXT 'KLA PreAmp\nSelected frequency: {FreqValues[i]}kHz'");
							client.SendData($"FREQ +{FreqValues[i]}000");
						}
					}
					else if (selectedOption != i)
					{
						Console.WriteLine("Freq: " + FreqValues[i] + "kHz");
					}
				}
				ConsoleKey keyInput = Console.ReadKey().Key;
				if (keyInput == ConsoleKey.UpArrow)
				{
					selectedOption--;
					if (selectedOption < 0)
					{
						selectedOption = 0;
					}
				}
				if (keyInput == ConsoleKey.DownArrow)
				{
					selectedOption++;
					if (selectedOption > FreqValues.Count - 1)
					{
						selectedOption = FreqValues.Count - 1;
					}
				}
				if (keyInput == ConsoleKey.Backspace)
				{
					if (client != null)
					{
						Console.CursorVisible = true;
						client.SendData("DISP:TEXT:CLEAR");
						client.Disconnect();
					}
					Environment.Exit(0);
				}
			}
		}
	}
}

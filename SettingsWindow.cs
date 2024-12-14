using System;

class SettingsWindow
{
	public static SettingsWindow s;

	private SettingsWindow() { }

	public static void Initialize()
	{
		if (s == null) s = new();
	}

	public void Show()
	{
		bool validInput = true;
		do
		{
			Console.Clear();
			Console.WriteLine("Параметры игры");
			Console.WriteLine();
			Console.WriteLine($"1 - Размер окна навигации. Ширина: {NavigationWindow.s.windowWidth}. Высота: {NavigationWindow.s.windowHeight}.");
			Console.WriteLine();
			Console.WriteLine("0 - Назад");
			Console.WriteLine();

			string input = Console.ReadLine();
			if (input == "1")
			{
				Console.Write("Новая ширина окна: ");
				input = Console.ReadLine();
				int value;
				validInput = int.TryParse(input, out value);
				if (validInput)
				{
					NavigationWindow.s.windowWidth = value;
					Console.Write("Новая высота окна: ");
					input = Console.ReadLine();
					validInput = int.TryParse(input, out value);
					if (validInput)
					{
						NavigationWindow.s.windowHeight = value;
						NavigationWindow.s.UpdateDisplaySize();
						validInput = false;
					}
				}
			}
			else if (input == "0") NavigationWindow.s.Show();
			else validInput = false;
		}
		while (!validInput);
	}
}

using System;

class MainMenu
{
	public static MainMenu s;

	private MainMenu() { }

	public static void Initialize()
	{
		if (s == null) s = new();
	}

	public void Show()
	{
		Console.WriteLine("Просто бизнес");
		Console.WriteLine();
		Console.WriteLine("1 - Начать новую игру");
		Console.WriteLine("0 - Выход из игры");
		Console.WriteLine();

		bool validInput = false;
		while (!validInput)
		{
			string input = Console.ReadLine();
			if (input == "1")
			{
				Game.s.Start();
				validInput = true;
			}
			else if (input == "0") validInput = true;
		}
	}
}

using System;

class Program
{
	public static readonly Random rnd = new();
	static void Main()
	{
		Game.Initialize();
		MainMenu.Initialize();
		NavigationWindow.Initialize();
		SettingsWindow.Initialize();

		MainMenu.s.Show();
	}
}

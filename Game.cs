using System;
using System.Collections.Generic;

class Game
{
	public static Game s;

	public const double MAP_SIZE = 1e6; // Размер карты
	public List<StarSystem> starSystems = new(); // Звёздные системы
	public SpaceShip player; // Корабль игрока
	public double date; // Игровая дата
	static readonly Random rnd = Program.rnd;
	bool running = true;

	private Game() { }

	public static void Initialize()
	{
		if (s == null) s = new();
	}

	// Начало игры
	public void Start()
	{
		GenerateMap();
		SpawnPlayer();
		NavigationWindow.s.Show();
		GameLoop();
	}

	// Создание карты
	void GenerateMap()
	{
		for (int index = 0; index < 400; index++)
		{
			starSystems.Add(new StarSystem());
		}
	}

	// Размещение корабля игрока
	void SpawnPlayer()
	{
		player = new();
		player.position = (MAP_SIZE * rnd.NextDouble(), MAP_SIZE * rnd.NextDouble());
	}

	// Основной игровой цикл
	void GameLoop()
	{
		date = -NavigationWindow.s.zoom / 1024.0;
		while (running)
		{
			date += NavigationWindow.s.zoom / 1024.0;
			foreach(StarSystem starSystem in starSystems)
			{
				starSystem.Update();
			}
			if (NavigationWindow.s.active) NavigationWindow.s.Update();
		}
	}
}

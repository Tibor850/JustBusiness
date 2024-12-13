using System;
using System.Collections.Generic;

class Game
{
	public static Game s;

	public const double MAP_SIZE = 256.0;
	public List<StarSystem> starSystems = new();
	public SpaceShip player;
	public double date;
	static readonly Random rnd = Program.rnd;
	bool running = true;

	private Game() { }

	public static void Initialize()
	{
		if (s == null) s = new();
	}

	public void Start()
	{
		date = 0;
		GenerateMap();
		SpawnPlayer();
		NavigationWindow.s.Show();
		GameLoop();
	}

	void GenerateMap()
	{
		for (int index = 0; index < 400; index++)
		{
			starSystems.Add(new StarSystem());
		}
	}

	void SpawnPlayer()
	{
		player = new();
		player.position = (MAP_SIZE * rnd.NextDouble(), MAP_SIZE * rnd.NextDouble());
	}

	void GameLoop()
	{
		while (running)
		{
			date += 1 / NavigationWindow.s.Zoom;
			foreach(StarSystem starSystem in starSystems)
			{
				starSystem.Update();
			}
			if (NavigationWindow.s.active) NavigationWindow.s.Update();
		}
	}
}

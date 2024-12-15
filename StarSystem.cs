using System;
using System.Collections.Generic;

class StarSystem
{
	public string name; // Название звездной системы
	public (double x, double y) position; // Координаты звезды в мире
	public readonly List<Planet> planets; // Планеты системы
	public readonly double size; // Размер системы

	static readonly Random rnd = Program.rnd;
	const double MIN_DISTANCE = 1000.0; // Минимальное расстояние до ближайшей зведы
	const int MAX_PLANETS = 10; // Максимальное количество планет у звезды

	public StarSystem()
	{
		name = $"Система {Game.s.starSystems.Count + 1}";

		// Создаём планеты
		planets = new();
		for (int index = 0; index < rnd.Next(MAX_PLANETS + 1); index++)
		{
			Planet planet = new(this, index + 1);
			planets.Add(planet);
		}
		if (planets.Count > 0)
		{
			size = planets[^1].orbitRadius;
		}
		else size = 0;

		// Размещаем звезду в мире
		double minSqDistance;
		do
		{
			minSqDistance = Game.MAP_SIZE * Game.MAP_SIZE * 2;
			position = new()
			{
				x = Game.MAP_SIZE * rnd.NextDouble(),
				y = Game.MAP_SIZE * rnd.NextDouble()
			};
			foreach (StarSystem starSystem in Game.s.starSystems)
			{
				double sqDistance = SqDistance(position, starSystem.position);
				if (minSqDistance > sqDistance) minSqDistance = sqDistance;
			}
		}
		while (minSqDistance < MIN_DISTANCE * MIN_DISTANCE);
	}

	// Космопорты системы
	public List<Planet> SpacePorts
	{
		get
		{
			List<Planet> spacePorts = new();
			foreach(Planet planet in planets)
			{
				if (planet.hasSpacePort) spacePorts.Add(planet);
			}
			return spacePorts;
		}
	}

	public void Update()
	{
		foreach(Planet planet in planets)
		{
			planet.Update();
		}
	}

	static double SqDistance((double x, double y) point1, (double x, double y) point2)
	{
		return (point1.x - point2.x) * (point1.x - point2.x) + (point1.y - point2.y) * (point1.y - point2.y);
	}
}

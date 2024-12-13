using System;

class StarSystem
{
	static readonly Random rnd = Program.rnd;
	public string name;
	public (double x, double y) position;

	const double MIN_DISTANCE = 5.0;

	public StarSystem()
	{
		name = $"Система {Game.s.starSystems.Count + 1}";
		while (true)
		{
			double x = Game.MAP_SIZE * rnd.NextDouble();
			double y = Game.MAP_SIZE * rnd.NextDouble();

			double minSqDistance = Game.MAP_SIZE * Game.MAP_SIZE;
			foreach (StarSystem starSystem in Game.s.starSystems)
			{
				double sqDistance = SqDistance((x, y), starSystem.position);
				if (minSqDistance > sqDistance) minSqDistance = sqDistance;
			}

			if (minSqDistance > MIN_DISTANCE * MIN_DISTANCE)
			{
				position = (x, y);
				break;
			}
		}
	}

	static double SqDistance((double x, double y) point1, (double x, double y) point2)
	{
		return (point1.x - point2.x) * (point1.x - point2.x) + (point1.y - point2.y) * (point1.y - point2.y);
	}
}

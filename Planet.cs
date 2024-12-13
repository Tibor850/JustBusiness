using System;

class Planet
{
	public string name;
	public (double x, double y) position;

	static readonly Random rnd = Program.rnd;
	readonly StarSystem starSystem;
	readonly double phase;
	readonly double orbitRadius;
	readonly double period;

	public Planet(StarSystem starSystem, int number)
	{
		this.starSystem = starSystem;
		name = $"{starSystem.name}.{number}";
		phase = 2.0 * Math.PI * rnd.NextDouble();
		orbitRadius = number / 512.0;
		period = 50 / Math.PI * number;
	}

	public void Update()
	{
		position.x = starSystem.position.x + orbitRadius * Math.Cos(phase + Game.s.date / period);
		position.y = starSystem.position.y + orbitRadius * Math.Sin(phase + Game.s.date / period);
	}
}

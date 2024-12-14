using System;

class Planet
{
	public string name; // Название планеты
	public (double x, double y) position; // Координаты в мире

	static readonly Random rnd = Program.rnd;
	readonly StarSystem starSystem; // Родительская звездная система
	readonly double phase0; // Начальная фаза планеты на орбите (рад)
	readonly double orbitRadius; // Радиус орбиты
	readonly double period; // Период обращения вокруг звезды (сутки)

	public Planet(StarSystem starSystem, int number)
	{
		this.starSystem = starSystem;
		name = $"{starSystem.name}.{number}";
		phase0 = Math.Tau * rnd.NextDouble();
		orbitRadius = number * 10;
		period = 30.0 * number; // 30 дней для самой ближней планеты
	}

	public void Update()
	{
		// Текущая фаза планеты на орбите
		double phase = phase0 + Math.Tau * Game.s.date / period;
		position.x = starSystem.position.x + orbitRadius * Math.Cos(phase);
		position.y = starSystem.position.y + orbitRadius * Math.Sin(phase);
	}
}

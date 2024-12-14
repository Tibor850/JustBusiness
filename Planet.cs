using System;

class Planet
{
	public string name; // �������� �������
	public (double x, double y) position; // ���������� � ����

	static readonly Random rnd = Program.rnd;
	readonly StarSystem starSystem; // ������������ �������� �������
	readonly double phase0; // ��������� ���� ������� �� ������ (���)
	readonly double orbitRadius; // ������ ������
	readonly double period; // ������ ��������� ������ ������ (�����)

	public Planet(StarSystem starSystem, int number)
	{
		this.starSystem = starSystem;
		name = $"{starSystem.name}.{number}";
		phase0 = Math.Tau * rnd.NextDouble();
		orbitRadius = number * 10;
		period = 30.0 * number; // 30 ���� ��� ����� ������� �������
	}

	public void Update()
	{
		// ������� ���� ������� �� ������
		double phase = phase0 + Math.Tau * Game.s.date / period;
		position.x = starSystem.position.x + orbitRadius * Math.Cos(phase);
		position.y = starSystem.position.y + orbitRadius * Math.Sin(phase);
	}
}

using System;
using System.Text;

class NavigationWindow
{
	public static NavigationWindow s;
	public bool active = false;
	public int windowWidth = 100;
	public int windowHeight = 28;

	int zoom = 10;
	readonly double[] zoomValue = { 0.0009765625, 0.001953125, 0.00390625, 0.0078125, 0.015625, 0.03125, 0.0625, 0.125, 0.25, 0.5, 1.0, 2.0, 4.0, 8.0, 16.0, 32.0, 64.0, 128.0, 256.0, 512.0, 1024.0 };
	char[][] frameBuffer;
	StringBuilder display = new();

	public double Zoom
	{
		get
		{
			return zoomValue[zoom];
		}
	}

	private NavigationWindow()
	{
		UpdateDisplaySize();
	}

	public static void Initialize()
	{
		if (s == null) s = new();
	}

	public void Show()
	{
		active = true;
	}

	public void Update()
	{
		Console.Clear();
		RenderMap();

		UserInput();
	}

	public void UpdateDisplaySize()
	{
		frameBuffer = new char[windowHeight][];
		for (int line = 0; line < windowHeight; line++)
		{
			frameBuffer[line] = new char[windowWidth];
		}
		display = new StringBuilder(windowWidth * windowHeight);
	}

	void RenderMap()
	{
		ClearFrame();
		RenderStars();
		RenderShips();
		RenderWindow();
		Render();
	}

	void ClearFrame()
	{
		for (int y = 0; y < windowHeight; y++)
		{
			for (int x = 0; x < windowWidth; x++)
			{
				frameBuffer[y][x] = ' ';
			}
		}
		display.Clear();
	}

	void RenderStars()
	{
		foreach (StarSystem starSystem in Game.s.starSystems)
		{
			(int x, int y) windowPoint = WindowCoordinates(starSystem.position, Game.s.player.position);
			PutChar('*', windowPoint);
			if (Zoom > 0.5 && Zoom < 128.0) 
			{
				for (int index = 0; index < starSystem.name.Length; index++)
				{
					PutChar(starSystem.name[index], (windowPoint.x + 1 + index, windowPoint.y + 1));
				}
			}
			else if (Zoom >= 128.0)
			{
				for (int index = 0; index < starSystem.planets.Count; index++)
				{
					PutChar('o', WindowCoordinates(starSystem.planets[index].position, Game.s.player.position));
				}
			}
		}
	}

	void PutChar(char symbol, (int x, int y) point)
	{
		if (point.x >= 0 && point.x < windowWidth && point.y >= 0 && point.y < windowHeight)
		{
			frameBuffer[point.y][point.x] = symbol;
		}
	}

	void RenderShips()
	{
		frameBuffer[windowHeight / 2][windowWidth / 2] = 'A';
	}

	void RenderWindow()
	{
		for (int x = 1; x < windowWidth - 1; x++)
		{
			frameBuffer[0][x] = '═';
			frameBuffer[windowHeight - 1][x] = '═';
		}
		for (int y = 1; y < windowHeight - 1; y++)
		{
			frameBuffer[y][0] = '║';
			frameBuffer[y][windowWidth - 1] = '║';
		}
		frameBuffer[0][0] = '╔';
		frameBuffer[windowHeight - 1][0] = '╚';
		frameBuffer[0][windowWidth - 1] = '╗';
		frameBuffer[windowHeight - 1][windowWidth - 1] = '╝';
	}

	void Render()
	{
		for (int line = 0; line < windowHeight; line++)
		{
			display.Append(frameBuffer[line]);
			display.AppendLine();
		}

		Console.WriteLine(display.ToString());
	}

	void UserInput()
	{
		Console.WriteLine($"Перемещение   Масштаб   Дата: {Game.s.date:f2}");
		Console.WriteLine("    qwe         + o");
		Console.WriteLine("    asd         - p");
		Console.WriteLine("    zxc");
		Console.WriteLine($"             Шаг: {1 / Zoom:g2} сут.");
		Console.WriteLine("0 - Параметры");

		bool validInput = false;
		while (!validInput)
		{
			validInput = true;
			string input = Console.ReadLine();
			if (input == "q") Game.s.player.MoveUpLeft();
			else if (input == "w") Game.s.player.MoveUp();
			else if (input == "e") Game.s.player.MoveUpRight();
			else if (input == "a") Game.s.player.MoveLeft();
			else if (input == "s") validInput = true;
			else if (input == "d") Game.s.player.MoveRight();
			else if (input == "z") Game.s.player.MoveDownLeft();
			else if (input == "x") Game.s.player.MoveDown();
			else if (input == "c") Game.s.player.MoveDownRight();
			else if (input == "o")
			{
				ZoomIn();
				Update();
			}
			else if (input == "p")
			{
				ZoomOut();
				Update();
			}
			else if (input == "0")
			{
				active = false;
				SettingsWindow.s.Show();
			}
			else validInput = false;
		}
	}

	void ZoomOut()
	{
		zoom--;
		if (zoom < 0) zoom = 0;
	}

	void ZoomIn()
	{
		zoom++;
		if (zoom > zoomValue.Length - 1) zoom = zoomValue.Length - 1;
	}

	(int x, int y) WindowCoordinates((double x, double y) point, (double x, double y) center)
	{
		int displayX = (int)((point.x - center.x) * zoomValue[zoom]) + windowWidth / 2;
		int displayY = (int)((point.y - center.y) * zoomValue[zoom]) + windowHeight / 2;
		return (displayX, displayY);
	}

	static double SqDistance((double x, double y) point1, (double x, double y) point2)
	{
		return (point1.x - point2.x) * (point1.x - point2.x) + (point1.y - point2.y) * (point1.y - point2.y);
	}
}

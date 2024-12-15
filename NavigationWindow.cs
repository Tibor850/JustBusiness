using System;
using System.Collections.Generic;
using System.Text;

class NavigationWindow
{
	public static NavigationWindow s;
	public bool active = false; // Статус активности окна для обновления
	public int windowWidth = 100; // Размер окна навигационной карты
	public int windowHeight = 28;
	public int zoom = 1024; // Масштаб карты

	char[][] frameBuffer; // Буфер дисплея карты
	StringBuilder display = new(); // Дисплей карты

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

	// Изменение размера дисплея карты
	public void UpdateDisplaySize()
	{
		frameBuffer = new char[windowHeight][];
		for (int line = 0; line < windowHeight; line++)
		{
			frameBuffer[line] = new char[windowWidth];
		}
		display = new StringBuilder(windowWidth * windowHeight);
	}

	// Рендер карты
	void RenderMap()
	{
		ClearFrame();
		RenderStars();
		RenderShips();
		RenderWindowFrame();
		Render();
	}

	// Очистка буфера дисплея
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

	// Рендер звезд на дисплее
	void RenderStars()
	{
		StarSystem? nearestSystem = null; // Ближайшая к игроку система
		double minSqDistance = Game.MAP_SIZE * Game.MAP_SIZE * 2; // Наименьшее расстояние

		foreach (StarSystem starSystem in Game.s.starSystems)
		{
			// Координаты центра звезды на дисплее
			(int x, int y) centerPoint = WindowCoordinates(starSystem.position, Game.s.player.position);

			// Пропускаем звездные системы не попадающие в область дисплея
			if (centerPoint.x + starSystem.size / zoom < 0 ||
				centerPoint.x - starSystem.size / zoom > windowWidth ||
				centerPoint.y + starSystem.size / zoom < 0 ||
				centerPoint.y - starSystem.size / zoom > windowHeight) continue;

			// Отображаем название звезды
			if (zoom >= 128 && zoom <= 2048)
			{
				for (int index = 0; index < starSystem.name.Length; index++)
				{
					RenderChar(starSystem.name[index], (centerPoint.x + 1 + index, centerPoint.y + 1));
				}
			}

			// Отображаем планеты
			if (zoom <= 128)
			{
				foreach (Planet planet in starSystem.planets)
				{
					(int x, int y) planetPosition = WindowCoordinates(planet.position, Game.s.player.position);
					if (planet.hasSpacePort) RenderChar('0', planetPosition);
					else RenderChar('O', planetPosition);

					// Отображаем название планеты
					if (zoom <= 32)
					{
						for (int index = 0; index < planet.name.Length; index++)
						{
							RenderChar(planet.name[index], (planetPosition.x + 1 + index, planetPosition.y + 1));
						}
					}
				}
			}

			// Изображение звезды
			if (zoom == 1) RenderSprite(starSprite[1], centerPoint, centerSprite: true);
			else if (zoom == 2) RenderSprite(starSprite[0], centerPoint, centerSprite: true);
			else if (zoom <= 128) RenderChar('#', centerPoint);
			else RenderChar('*', centerPoint);

			// Определяем ближайшую звезду
			if (zoom <= 128)
			{
				double sqDistance = SqDistance(starSystem.position, Game.s.player.position);
				if (sqDistance < minSqDistance)
				{
					nearestSystem = starSystem;
					minSqDistance = sqDistance;
				}
			}
		}

		// Выводим информацию о ближайшей системе
		if (nearestSystem != null) ShowSystemInfo(nearestSystem);
	}

	// Панель информации о системе
	void ShowSystemInfo(StarSystem starSystem)
	{
		List<Planet> spacePorts = starSystem.SpacePorts;
		int panelLines = spacePorts.Count + 7;
		if (spacePorts.Count == 0) panelLines++;
		string[] panelSprite = new string[panelLines];
		panelSprite[0] = "╔";
		panelSprite[1] = $"║ {starSystem.name}";
		panelSprite[2] = "║";
		panelSprite[3] = $"║ Количество планет: {starSystem.planets.Count}";
		panelSprite[4] = "║";
		panelSprite[5] = "║ Обитаемые пленеты:";
		if (spacePorts.Count == 0) panelSprite[6] = "║ Нет";
		else
		{
			for (int index = 0; index < spacePorts.Count; index++)
			{
				panelSprite[6 + index] = $"║ {spacePorts[index].name}";
			}
		}
		panelSprite[^1] = "╚";

		// Находим самую длинную строку
		int maxLength = 0;
		foreach (string line in panelSprite)
		{
			if (line.Length > maxLength) maxLength = line.Length;
		}

		// Выравниваем правую сторону спрайта и добавляем рамку
		for (int lineIndex = 0; lineIndex < panelLines; lineIndex++)
		{
			string newline = panelSprite[lineIndex];
			for (int charIndex = newline.Length; charIndex < maxLength + 2; charIndex++)
			{
				// Первая строка
				if (lineIndex == 0)
				{
					if (charIndex < maxLength + 1) newline += '═';
					else newline += '╗';
				}
				// Последняя строка
				else if (lineIndex == panelLines - 1)
				{
					if (charIndex < maxLength + 1) newline += '═';
					else newline += '╝';
				}
				// Остальные строки
				else
				{
					if (charIndex < maxLength + 1) newline += ' ';
					else newline += '║';
				}
			}
			panelSprite[lineIndex] = newline;
		}
		RenderSprite(panelSprite, (1, 1), transparent: false);
	}

	// Рендер одного символа
	void RenderChar(char symbol, (int x, int y) point)
	{
		if (point.x >= 0 && point.x < windowWidth && point.y >= 0 && point.y < windowHeight)
		{
			frameBuffer[point.y][point.x] = symbol;
		}
	}

	// Рендер спрайта
	void RenderSprite(string[] sprite, (int x, int y) point, bool centerSprite = false, bool transparent = true)
	{
		// Сдвиг спрайта для центрирования
		(int x, int y) pointShift = (0, 0);
		if (centerSprite)
		{
			pointShift.x = -sprite[0].Length / 2;
			pointShift.y = -sprite.Length / 2;
		}

		// Перебираем все точки спрайта
		for (int spriteY = 0; spriteY < sprite.Length; spriteY++)
		{
			for (int spriteX = 0; spriteX < sprite[spriteY].Length; spriteX++)
			{
				// Если нужна прозрачность пропускаем пустые точки
				if (transparent && sprite[spriteY][spriteX] == ' ') continue;

				if (point.x + pointShift.x + spriteX >= 0 && point.x + pointShift.x + spriteX < windowWidth && point.y + pointShift.y + spriteY >= 0 && point.y + pointShift.y + spriteY < windowHeight)
				{
					frameBuffer[point.y + pointShift.y + spriteY][point.x + pointShift.x + spriteX] = sprite[spriteY][spriteX];
				}
			}
		}
	}

	// Рендер корабля игрока
	void RenderShips()
	{
		frameBuffer[windowHeight / 2][windowWidth / 2] = 'A';
	}

	// Отображение рамки карты
	void RenderWindowFrame()
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

	// Вывод буфера на экран
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
		Console.WriteLine($"             Шаг: {zoom / 1024.0:g2} сут.");
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

	void ZoomIn()
	{
		if (zoom <= 1) return;
		zoom /= 2;
	}

	void ZoomOut()
	{
		if (zoom >= 65536) return;
		zoom *= 2;
	}

	// Координаты на дисплее карты
	(int x, int y) WindowCoordinates((double x, double y) point, (double x, double y) center)
	{
		int displayX = (int)point.x / zoom - (int)center.x / zoom + windowWidth / 2;
		int displayY = (int)point.y / 2 / zoom - (int)center.y / 2 / zoom + windowHeight / 2;
		return (displayX, displayY);
	}

	static double SqDistance((double x, double y) point1, (double x, double y) point2)
	{
		return (point1.x - point2.x) * (point1.x - point2.x) + (point1.y - point2.y) * (point1.y - point2.y);
	}

	static readonly string[][] starSprite = 
	[
		[
			@" # ",
			@"###",
			@" # "
		],
		[
			@"   ###   ",
			@" ####### ",
			@"#########",
			@" ####### ",
			@"   ###   ",
		],
	];
}

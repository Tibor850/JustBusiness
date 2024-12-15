using System;

class SpaceShip
{
	public (double x, double y) position; // Координаты в мире
	const double SQR2 = 1.4142135623731; // Карень квадратный из 2
	
	public void MoveUpLeft()
	{
		position.x -= NavigationWindow.s.zoom / SQR2;
		position.y -= NavigationWindow.s.zoom / SQR2;
	}

	public void MoveUp()
	{
		position.y -= NavigationWindow.s.zoom;
	}

	public void MoveUpRight()
	{
		position.x += NavigationWindow.s.zoom / SQR2;
		position.y -= NavigationWindow.s.zoom / SQR2;
	}

	public void MoveLeft()
	{
		position.x -= NavigationWindow.s.zoom;
	}

	public void MoveRight()
	{
		position.x += NavigationWindow.s.zoom;
	}

	public void MoveDownLeft()
	{
		position.x -= NavigationWindow.s.zoom / SQR2;
		position.y += NavigationWindow.s.zoom / SQR2;
	}

	public void MoveDown()
	{
		position.y += NavigationWindow.s.zoom;
	}

	public void MoveDownRight()
	{
		position.x += NavigationWindow.s.zoom / SQR2;
		position.y += NavigationWindow.s.zoom / SQR2;
	}
}

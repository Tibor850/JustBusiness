using System;

class SpaceShip
{
	public (double x, double y) position;
	const double SQR2 = 1.4142135623731;

	public void MoveUpLeft()
	{
		position.x -= 1 / (SQR2 * NavigationWindow.s.Zoom);
		position.y -= 1 / (SQR2 * NavigationWindow.s.Zoom);
	}

	public void MoveUp()
	{
		position.y -= 1 / NavigationWindow.s.Zoom;
	}

	public void MoveUpRight()
	{
		position.x += 1 / (SQR2 * NavigationWindow.s.Zoom);
		position.y -= 1 / (SQR2 * NavigationWindow.s.Zoom);
	}

	public void MoveLeft()
	{
		position.x -= 1 / NavigationWindow.s.Zoom;
	}

	public void MoveRight()
	{
		position.x += 1 / NavigationWindow.s.Zoom;
	}

	public void MoveDownLeft()
	{
		position.x -= 1 / (SQR2 * NavigationWindow.s.Zoom);
		position.y += 1 / (SQR2 * NavigationWindow.s.Zoom);
	}

	public void MoveDown()
	{
		position.y += 1 / NavigationWindow.s.Zoom;
	}

	public void MoveDownRight()
	{
		position.x += 1 / (SQR2 * NavigationWindow.s.Zoom);
		position.y += 1 / (SQR2 * NavigationWindow.s.Zoom);
	}
}

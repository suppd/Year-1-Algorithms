using System.Drawing;

/**
 * This class represents (the data for) a Room, at this moment only a rectangle in the dungeon.
 */
class Room
{
	public Rectangle area;
	public int amountOfOverlaps = 0;
	public int id;
	private static int lastID = 0;

	public int doorCount = 0;

	public Room (Rectangle pArea)
	{
		area = pArea;

		id = lastID++;

	}



	public int ToInt()
	{
		return id;
	}
	
	//TODO: Implement a toString method for debugging?
	//Return information about the type of object and it's data
	//eg Room: (x, y, width, height)

}

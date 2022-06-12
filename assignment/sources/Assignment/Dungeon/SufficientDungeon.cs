using GXPEngine;
using System.Drawing;
using System;
using GXPEngine.OpenGL;
using System.Collections.Generic;


class SufficientDungeon : Dungeon
{

	/// <summary>
	/// improve code quality (remove nesting and new lines)
	/// remove the 50/50 random thing
	/// know what the double for loop does on placedoors
	/// </summary>
	Random random = new Random();
	public readonly Queue<Room> openList = new Queue<Room>();

	public SufficientDungeon(Size pSize) : base(pSize) { }

	protected override void generate(int pMinimumRoomSize)
	{
		Room root = new Room(new Rectangle(0, 0, size.Width, size.Height));
		openList.Clear();
		openList.Enqueue(root);
		

		while (openList.Count > 0)
        {
			Room currentRoom = openList.Dequeue();
			
			drawRoom(currentRoom, Pens.Red);
			//Console.ReadKey();
			bool canBeSplit = CanBeSplit(currentRoom, pMinimumRoomSize);
			if (canBeSplit)
            {
				List<Room> newRooms = Split(currentRoom, pMinimumRoomSize);
				
				foreach (Room room in newRooms)
                {
					openList.Enqueue(room);
				}
            }
			else
            {
				closedList.Add(currentRoom);
			}
			//Console.WriteLine("index" + closedList.Count);
			Console.WriteLine(CanBeSplit(root,pMinimumRoomSize));
			drawRooms(openList, Pens.Yellow);
			drawRooms(closedList, Pens.Green);
			//Console.ReadKey();
		}

		if (openList.Count <= 0)
		{
			//closedList.Sort(CompareArea);
			//closedList.RemoveAt(0);
			//closedList.RemoveAt(closedList.Count - 1);

			setupDoors();
            Console.WriteLine(doors.Count);
		}
	}

	public void setupDoors()
	{
		for (int i = 0; i < closedList.Count - 1; i++)
		{
			for (int j = i + 1; j < closedList.Count; j++)
			{
				Room room = closedList[i];
				Room room2 = closedList[j];				
				Door door;
				Rectangle overlaps = Rectangle.Intersect(room.area, room2.area);
				if (!overlaps.IsEmpty)
				{
					room.amountOfOverlaps++;
					room2.amountOfOverlaps++;
					if (overlaps.Width >= 2)
					{
						//Console.WriteLine("overlap horizontally detected " + overlaps.Width);
						door = new Door(new Point(overlaps.X + overlaps.Width / 2 + random.Next(-1, 1), overlaps.Y + (overlaps.Height / 2)), room, room2);
						//Console.WriteLine("door Added at:" + door.location);

						if (room.doorCount <= room.amountOfOverlaps || room2.doorCount <= room2.amountOfOverlaps)
						{
							doors.Add(door);
							room.doorCount++;
							room2.doorCount++;
						}
					}

					else if (overlaps.Height >= 4)
					{
						// Console.WriteLine("overlap vertically detected " + overlaps.Height);

						door = new Door(new Point(overlaps.X + overlaps.Width / 2, overlaps.Y + overlaps.Height / 2 + random.Next(-1, 1)), room, room2);
						// Console.WriteLine("door Added at:" + door.location);

						if (room.doorCount <= room.amountOfOverlaps || room2.doorCount <= room2.amountOfOverlaps)
						{
							doors.Add(door);
							room.doorCount++;
							room2.doorCount++;
						}
					}

				}
            }
		}
	}

	private bool CanBeSplit(Room room, int minimumroomsize)
	{
		if (room.area.Width >= minimumroomsize * 2 + 1 || room.area.Height >= minimumroomsize * 2 + 1)
		{
			return true;
		}
        else
        {
			return false;
        }
    }

	private List<Room> Split(Room room, int minimumRoomsize)
    {

		if (room.area.Width > minimumRoomsize * 2 + 1)
		{
			return splitHorizontal(room, minimumRoomsize);
		}
	

		else if (room.area.Height > minimumRoomsize * 2 + 1)
		{
			return splitVertical(room, minimumRoomsize);
		}
		
		else
		{
			return new List<Room>();
		}
		
		
	}

	private List<Room> splitVertical(Room room, int minimumRoomsize)
	{
		List<Room> result = new List<Room>();

		int maximumRoomsize = room.area.Height - minimumRoomsize;
		int cutoff = random.Next(minimumRoomsize, maximumRoomsize);

		Room roomA = new Room(new Rectangle(room.area.X, room.area.Y, room.area.Width, cutoff + 1));
		Room roomB = new Room(new Rectangle(room.area.X, room.area.Y + cutoff, room.area.Width, room.area.Height - cutoff));

		result.Add(roomA);
		result.Add(roomB);
 
		return result;
	}

    private List<Room> splitHorizontal(Room room, int minimumRoomsize)
    {
        List<Room> result = new List<Room>();
		
        int maximumRoomsize = room.area.Width - minimumRoomsize;
        int cutoff = random.Next(minimumRoomsize, maximumRoomsize);

        Room roomA = new Room(new Rectangle(room.area.X, room.area.Y, cutoff + 1, room.area.Height));
        Room roomB = new Room(new Rectangle(room.area.X + cutoff , room.area.Y, room.area.Width  - cutoff, room.area.Height));

        result.Add(roomA);
        result.Add(roomB);
 	
		return result;
    }
	private static int CompareArea(Room room1, Room room2)
    {
		int a1 = CalculateRoomArea(room1);
		int a2 = CalculateRoomArea(room2);

		if (a1 < a2)
        {
			return -1;
        }
        else
        {
			if (a1 > a2)
            {
				return 1;
            }
        }
		return 0;
    }

	private static int CalculateRoomArea(Room room)
    {
		return room.area.Width * room.area.Height;
    }
}


using GXPEngine;
using System.Drawing;
using System;
using GXPEngine.OpenGL;
using System.Collections.Generic;
using System.Collections;
/**
 * An example of a dungeon nodegraph implementation.
 * 
 * This implementation places only three nodes and only works with the SampleDungeon.
 * Your implementation has to do better :).
 * 
 * It is recommended to subclass this class instead of NodeGraph so that you already 
 * have access to the helper methods such as getRoomCenter etc.
 * 
 * TODO:
 * - Create a subclass of this class, and override the generate method, see the generate method below for an example.
 */
class HighLevelDungeonNodeGraph : SampleDungeonNodeGraph
{

    protected Dungeon dungeon;
    public Queue<Node> nodesQ = new Queue<Node>();
    public Dictionary<Door, Node> doorNodes = new Dictionary<Door, Node>();
    public Dictionary<Room, Node> roomNodes = new Dictionary<Room, Node>();

    public HighLevelDungeonNodeGraph(Dungeon pDungeon) : base(pDungeon)
    {
        dungeon = pDungeon;
    }


    protected override void generate()
    {  
        placeNodes();

        for (int i = 0; i < dungeon.closedList.Count; i++)
        {
           roomNodes.Add(dungeon.closedList[i], nodes[i]);
        }

        for (int i = 0; i < dungeon.doors.Count; i++)
        {
            doorNodes.Add(dungeon.doors[i], nodes[i + dungeon.closedList.Count]);
        }

        foreach (Node node in roomNodes.Values)
        {
            Console.WriteLine("nodes information " + " ID =  " + node.id + ", LOC =  " + node.location + " , room = " + node.roomID + " is door? " + node.isdoor);  
        }

        foreach (Node node in doorNodes.Values)
        {
            Console.WriteLine("door nodes information " + " ID =  " + node.id + ", LOC =  " + node.location + " , room = " + node.roomID + " is door? " + node.isdoor);
        }


        Console.WriteLine(doorNodes.Count);
        Console.WriteLine(roomNodes.Count);

        foreach (KeyValuePair<Door, Node> keypair in doorNodes)
        {
            Node nodeA = roomNodes[keypair.Key.roomA];
            Node nodeB = roomNodes[keypair.Key.roomB];

            if (keypair.Key.roomA != null)
            {
                AddConnection(keypair.Value, nodeA);
            }
            if (keypair.Key.roomA != null)
            {
                AddConnection(keypair.Value, nodeB);
            }
        }
    }

    public void placeNodes()
    {
        if (dungeon != null)
        {
            for (int i =0 ; i < dungeon.closedList.Count; i++)
            {
                nodes.Add(new Node(getRoomCenter(dungeon.closedList[i]),null, false));
            }
            for (int j = 0 ; j < dungeon.doors.Count; j++)
            {
                nodes.Add(new Node(getDoorCenter(dungeon.doors[j]), null, true));
            }
        }
    }
}
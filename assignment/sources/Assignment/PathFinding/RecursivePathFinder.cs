using GXPEngine;
using System;
using System.Collections.Generic;

/**
 * An example of a PathFinder implementation which completes the process by rolling a die 
 * and just returning the straight-as-the-crow-flies path if you roll a 6 ;). 
 */
class RecursivePathFinder : PathFinder	{

	public RecursivePathFinder(NodeGraph pGraph) : base(pGraph) {}

	protected override List<Node> generate(Node pFrom, Node pTo)
	{
		if (pFrom == pTo)
        {
			Console.WriteLine("Path not created cause start-end node are the same?!");
			return null;
        }
		else if (pFrom != pTo)
        {
            for (int i = 0; i < pFrom.connections.Count; i++) // only for direct connection
            {
                Console.WriteLine(pFrom.connections[i].id);
                if (pFrom.connections[i] == pTo)
                {
                    Console.WriteLine("path found");
                    return new List<Node>() { pFrom, pTo };
                }
                else
                {
                    for (int j = 0; j < pFrom.connections[i].connections.Count; j++)
                    {
                        if (pFrom.connections[i].connections[j] == pTo)
                        {
                            Console.WriteLine(pFrom.connections[i].connections[j].id);
                            return new List<Node>() { pFrom, pFrom.connections[i].connections[j], pTo };
                        }
                    }
                }
            }
            Console.WriteLine("There is A path but it couldnt be found..");
			return null;
			//return CheckNodesForGivenNode(pFrom,pTo);
        }
		else
		{
			Console.WriteLine("Too bad, no path found !!");
			return null;
		}
	}

	private List<Node> CheckNodesForGivenNode(Node FromNode, Node nodeToFind)
	{
		if (FromNode != null && nodeToFind != null)
		{ 
			foreach (Node node in FromNode.connections)
			{
				Console.WriteLine("Checking " + node.id);
				if (node == nodeToFind)
				{
					Console.WriteLine(node.id + "" + nodeToFind.id);
					return new List<Node>() { FromNode, nodeToFind };
				}
				else if (node != nodeToFind)
				{
					Console.WriteLine("continuing the Cycle...");
					foreach (Node node2 in node.connections)
                    {
						return CheckNodesForGivenNode(node2, nodeToFind);
					}
					
				}
			}
		}
		else
		{
			return null;
		}
		return null;
	}
}


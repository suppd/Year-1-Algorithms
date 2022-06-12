using GXPEngine;
using System;
using System.Collections.Generic;

/**
 * Very simple example of a nodegraphagent that walks directly to the node you clicked on,
 * ignoring walls, connections etc.
 */
class OnGraphWayPointAgent : NodeGraphAgent
{
	private Queue<Node> nodes = new Queue<Node>();
	//Current target to move towards
	private Node _target = null;
	private Node currentNode;
	private Node lastNode;
	public OnGraphWayPointAgent(NodeGraph pNodeGraph) : base(pNodeGraph)
	{
		SetOrigin(width / 2, height / 2);

		//position ourselves on a random node
		if (pNodeGraph.nodes.Count > 0)
		{
			jumpToNode(pNodeGraph.nodes[Utils.Random(0, pNodeGraph.nodes.Count)]);
		}

		//listen to nodeclicks
		pNodeGraph.OnNodeLeftClicked += onNodeClickHandler;
	}

	protected virtual void onNodeClickHandler(Node pNode)
	{
		//foreach (Node connectionNode in pNode.connections) 
		for (int i = 0; i < pNode.connections.Count; i++)
		{
			nodes.Enqueue(pNode);
			currentNode = nodes.Dequeue();
			if (currentNode.id == pNode.connections[i].id)
            {
				Console.WriteLine("Current Node Id " + currentNode.id + "Connection nodes Ids " +  pNode.connections[i].id);
				//Console.WriteLine("Targetnode is in connections");
				if (currentNode != null)
				{
					nodes.Enqueue(pNode);
					lastNode = nodes.Peek();
				}
			}
		}
		
	}

	protected override void Update()
	{

		
		//no target? Don't walk
		if (currentNode == null) return;
		//Move towards the target node, if we reached it, clear the target
		else if (moveTowardsNode(currentNode))
		{
			//Console.WriteLine(currentNode.id);
			//foreach (Node connectionNode in lastNode.connections)
   //         {
			//	if (currentNode != connectionNode)
   //             {
			//		//Console.WriteLine("Trying To Move Off Graph");
			//		//nodes.Clear();
   //             }
   //         }
			if (nodes == null)
			{
				nodes.Clear();
			}
			else if (nodes.Count != 0)
			{
				currentNode = nodes.Dequeue();
				
			}
		}
	}
}

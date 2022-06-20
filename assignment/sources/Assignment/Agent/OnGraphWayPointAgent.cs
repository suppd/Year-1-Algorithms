using GXPEngine;
using System;
using System.Collections.Generic;
using System.Linq;

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
			lastNode = jumpToNode(pNodeGraph.nodes[Utils.Random(0, pNodeGraph.nodes.Count)]);
			Console.WriteLine(lastNode.id);
		}

		//listen to nodeclicks
		pNodeGraph.OnNodeLeftClicked += onNodeClickHandler;
	}

	protected virtual void onNodeClickHandler(Node pNode)
	{
		if (lastNode.connections.Contains(pNode))
		{
			Console.WriteLine(pNode.id);
			nodes.Enqueue(pNode);
			currentNode = nodes.Dequeue();
			lastNode = currentNode;
		}
        else
        {
			return;
        }
		
		if (currentNode != null)
		{
			nodes.Enqueue(pNode);
			lastNode = nodes.Peek();
			for (int i = 0; i < currentNode.connections.Count; i++)
            {
				if (currentNode.connections.Contains(lastNode))
                {
					Console.WriteLine("Good!");
                }
                else
                {
					Console.WriteLine("Not Good!");
					nodes.Clear();
                }
            }
		}
	}

	protected override void Update()
	{


		//no target? Don't walk
		if (currentNode == null)
			return;
		//Move towards the target node, if we reached it, clear the target
		else if (moveTowardsNode(currentNode))
		{
			SetFrame(1);

		}
		else
		{
			
		}
	}
}

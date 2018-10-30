using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace IronSideStudio.CrazyTrafficJam.Pathfinding
{
	public class PathFinder
	{
		private static PathFinder instance;
		private GridNode.GridManager manager;
		private PathNode[,] nodes;

		public static PathFinder CreateInstance()
		{
			instance = new PathFinder();
			instance.manager = CoreManager.Instance.GetManager<GridNode.GridManager>();
			instance.nodes = new PathNode[instance.manager.SizeX, instance.manager.SizeZ];

			Vector3 posRaycast = Vector3.up;
			RaycastHit hit;

			for (int y = 0 ; y < instance.manager.SizeZ ; ++y)
			{
				posRaycast.z = y;
				for (int x = 0 ; x < instance.manager.SizeX ; ++x)
				{
					posRaycast.x = x;
					if (Physics.Raycast(posRaycast, Vector3.down, out hit, LayerMask.GetMask(Constante.Layer.GridNode)))
					{
						PathNode n = new PathNode(posRaycast);
						instance.nodes[x, y] = n;
					}

				}
			}

			return instance;
		}

		public static void DeleteInstance()
		{
			instance = null;
		}

		public static Vector3[] GetPath(Vector3 start, Vector3 end)
		{
			return instance.Pathfinding(start, end);
		}

		private Vector3[] Pathfinding(Vector3 start, Vector3 end)
		{
			RaycastHit hit;
			if (!Physics.Raycast(start + Vector3.up, Vector3.down, out hit, LayerMask.GetMask(Constante.Layer.GridNode)))
				return null;
			if (!Physics.Raycast(end + Vector3.up, Vector3.down, out hit, LayerMask.GetMask(Constante.Layer.GridNode)))
				return null;

			bool pathSuccess = false;
			PathNode startNode = new PathNode(start, null);
			PathNode targetNode = new PathNode(end, null);
			Heap<PathNode> openSet = new Heap<PathNode>(manager.SizeX * manager.SizeZ);
			HashSet<PathNode> closedSet = new HashSet<PathNode>();

			openSet.Push(startNode);

			while (openSet.Count > 0)
			{
				PathNode currentNode = openSet.Pop();
				closedSet.Add(currentNode);

				if (currentNode.Position == targetNode.Position)
				{
					pathSuccess = true;
					//Find path;
					break;
				}

				PathNode[] nodeNeighbours = GetNeighbours(currentNode);

				foreach (PathNode neighbour in nodeNeighbours)
				{
					if (closedSet.Contains(neighbour))
						continue;

					int movementCost = currentNode.MovementCost + neighbour.SpeedCost + neighbour.CarCost;
					if (movementCost < neighbour.MovementCost || !openSet.Contains(neighbour))
					{
						neighbour.SetMovementCost(movementCost);
						neighbour.SetDistance(GetDistance(neighbour, targetNode));
						neighbour.SetParent(currentNode);

						if (!openSet.Contains(neighbour))
							openSet.Push(neighbour);
						else
							openSet.UpdateItem(neighbour);
					}
				}
			}

			if (pathSuccess)
			{
				return RetracePath(startNode, targetNode);
			}
			return new Vector3[0];
		}

		private Vector3[] RetracePath(PathNode startNode, PathNode endNode)
		{
			List<Vector3> path = new List<Vector3>();
			PathNode currentNode = endNode;

			while (currentNode != startNode)
			{
				path.Add(currentNode.Position);
				currentNode = currentNode.Parent;
			}
			Vector3[] waypoints = path.ToArray();
			System.Array.Reverse(waypoints);
			return waypoints;
		}

		private PathNode[] GetNeighbours(PathNode node)
		{
			List<PathNode> neighbours = new List<PathNode>();

			int checkX = (int)node.Position.x - 1;
			int checkY = (int)node.Position.y;

			if (checkX >= 0)
				neighbours.Add(nodes[checkX, checkY]);

			checkX = (int)node.Position.x + 1;
			if (checkX < manager.SizeX)
				neighbours.Add(nodes[checkX, checkY]);

			checkX = (int)node.Position.x;
			checkY = (int)node.Position.y - 1;

			if (checkY >= 0)
				neighbours.Add(nodes[checkX, checkY]);

			checkY = (int)node.Position.y + 1;
			if (checkY < manager.SizeZ)
				neighbours.Add(nodes[checkX, checkY]);
			return neighbours.ToArray();
		}

		private int GetDistance(PathNode nodeA, PathNode nodeB)
		{

			int dstX = (int)Mathf.Abs(nodeA.Position.x - nodeB.Position.x);
			int dstY = (int)Mathf.Abs(nodeA.Position.y - nodeB.Position.y);

			if (dstX > dstY)
				return 14 * dstY + 10 * (dstX - dstY);
			return 14 * dstX + 10 * (dstY - dstX);
		}
	}
}
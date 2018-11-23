using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace IronSideStudio.CrazyTrafficJam.Pathfinding
{
	public class PathFinder
	{
		private static PathFinder instance;
		private Grid.Manager manager;
		private PathNode[,] nodes;

		public static PathFinder CreateInstance()
		{
			instance = new PathFinder();
			instance.manager = GameplayManager.Instance.GetManager<Grid.Manager>();
			instance.nodes = new PathNode[instance.manager.SizeX, instance.manager.SizeZ];

			Vector3 posNode = Vector3.up;

			for (int z = 0 ; z < instance.manager.SizeZ ; ++z)
			{
				posNode.z = z;
				for (int x = 0 ; x < instance.manager.SizeX ; ++x)
				{
					posNode.x = x;
					Grid.Node node = instance.manager.GetNode(posNode);
					if (node)
					{
						PathNode n = new PathNode(posNode, node);
						instance.nodes[x, z] = n;
					}
				}
			}

			return instance;
		}

		public static void DeleteInstance()
		{
			instance = null;
		}

		public static Grid.Node[] GetPath(Vector3 start, Vector3 end)
		{
			return instance.Pathfinding(start, end);
		}

		private Grid.Node[] Pathfinding(Vector3 start, Vector3 end)
		{
			RaycastHit hit;
			if (!Physics.Raycast(start + Vector3.up, Vector3.down, out hit, LayerMask.GetMask(Constante.Layer.GridNode)))
				return null;
			if (!Physics.Raycast(end + Vector3.up, Vector3.down, out hit, LayerMask.GetMask(Constante.Layer.GridNode)))
				return null;

			bool pathSuccess = false;
			PathNode startNode = nodes[(int)start.x, (int)start.z];
			PathNode targetNode = nodes[(int)end.x, (int)end.z];
			Heap<PathNode> openSet = new Heap<PathNode>(manager.SizeX * manager.SizeZ);
			HashSet<PathNode> closedSet = new HashSet<PathNode>();

			openSet.Push(startNode);

			while (openSet.Count > 0)
			{
				PathNode currentNode = openSet.Pop();
				closedSet.Add(currentNode);

				if (currentNode == targetNode)
				{
					pathSuccess = true;
					//Find path;
					break;
				}

				PathNode[] nodeNeighbours = GetNeighbours(currentNode);

				foreach (PathNode neighbour in nodeNeighbours)
				{
					if (closedSet.Contains(neighbour) ||
						neighbour.Type == Grid.ENodeType.None)
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
			return null;
		}

		private Grid.Node[] RetracePath(PathNode startNode, PathNode endNode)
		{
			List<Grid.Node> path = new List<Grid.Node>();
			PathNode currentNode = endNode;

			while (currentNode != startNode)
			{
				path.Add(currentNode.Node);
				currentNode = currentNode.Parent;
			}
			path.Add(startNode.Node);
			Grid.Node[] waypoints = path.ToArray();
			System.Array.Reverse(waypoints);
			return waypoints;
		}

		private PathNode[] GetNeighbours(PathNode node)
		{
			List<PathNode> neighbours = new List<PathNode>();

			int checkX = (int)node.Position.x - 1;
			int checkZ = (int)node.Position.z;

			if (checkX >= 0)
				neighbours.Add(nodes[checkX, checkZ]);

			checkX = (int)node.Position.x + 1;
			if (checkX < manager.SizeX)
				neighbours.Add(nodes[checkX, checkZ]);

			checkX = (int)node.Position.x;
			checkZ = (int)node.Position.z - 1;

			if (checkZ >= 0)
				neighbours.Add(nodes[checkX, checkZ]);

			checkZ = (int)node.Position.z + 1;
			if (checkZ < manager.SizeZ)
				neighbours.Add(nodes[checkX, checkZ]);
			return neighbours.ToArray();
		}

		private int GetDistance(PathNode nodeA, PathNode nodeB)
		{
			int dstX = (int)Mathf.Abs(nodeA.Position.x - nodeB.Position.x);
			int dstZ = (int)Mathf.Abs(nodeA.Position.z - nodeB.Position.z);

			if (dstX > dstZ)
				return 14 * dstZ + 10 * (dstX - dstZ);
			return 14 * dstX + 10 * (dstZ - dstX);
		}
	}
}
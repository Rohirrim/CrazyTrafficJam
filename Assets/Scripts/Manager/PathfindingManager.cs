using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System;

namespace IronSideStudio.CrazyTrafficJam
{
	public class PathfindingManager : Manager
	{
		public class Node : IComparable<Node>
		{
			public Node parent;

			public CrossPoint point;
			public List<Node> linkToNode;
			public int gCost;
			public int hCost;
			public int FCost { get { return gCost + hCost; } }

			public Node()
			{
				linkToNode = new List<Node>();
			}

			public int CompareTo(Node nodeToCompare)
			{
				int compare = FCost.CompareTo(nodeToCompare.FCost);
				if (compare == 0)
				{
					compare = hCost.CompareTo(nodeToCompare.hCost);
				}
				return -compare;
			}
		}

		private CrossPointManager crossManager;
		private List<Node> mapNodes;

		public override bool Init()
		{
			base.Init();
			crossManager = CoreManager.Instance.GetManager<CrossPointManager>();
			mapNodes = new List<Node>();
			return IsInit = true;
		}

		public void Load(SaveMap save)
		{
			mapNodes.Clear();

			for (int i = 0 ; i < save.crossPoints.Length ; ++i)
			{
				Node newNode = new Node();
				newNode.point = crossManager.GetCross(save.crossPoints[i].id);
				mapNodes.Add(newNode);
			}
			for (int i = 0 ; i < save.crossPoints.Length ; ++i)
			{
				Node currentNode = GetNode(crossManager.GetCross(save.crossPoints[i].id));

				for (int j = 0 ; j < save.crossPoints[i].links.Length ; ++j)
				{
					Node nextNode = GetNode(crossManager.GetCross(save.crossPoints[i].links[j]));
					currentNode.linkToNode.Add(nextNode);
				}
			}
		}

		private CrossPoint s;
		private CrossPoint e;

		private void Update()
		{
			if (Input.GetMouseButtonDown(1))
			{
				Vector2 worldPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
				RaycastHit2D hit = Physics2D.Raycast(worldPosition, Vector2.zero, 10f, LayerMask.GetMask(Constante.Layer.CrossPoint));
				if (hit.transform)
				{
					s = hit.transform.GetComponent<CrossPoint>();
				}
			}
			if (Input.GetMouseButtonUp(1))
			{
				Vector2 worldPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
				RaycastHit2D hit = Physics2D.Raycast(worldPosition, Vector2.zero, 10f, LayerMask.GetMask(Constante.Layer.CrossPoint));
				if (hit.transform)
				{
					e = hit.transform.GetComponent<CrossPoint>();
				}

				if (s && e)
				{
					FindPath(s, e);
				}
			}
		}

		private Node GetNode(CrossPoint point)
		{
			foreach (Node node in mapNodes)
			{
				if (node.point == point)
					return node;
			}
			return null;
		}

		#region Pathfinding
		public Node[] FindPath(CrossPoint start, CrossPoint end)
		{
			Node startNode = GetNode(start);
			Node endNode = GetNode(end);
			bool pathSucess = false;

			if (startNode == null || endNode == null)
				return null;
			Heap<Node> openSet = new Heap<Node>(mapNodes.Count);
			HashSet<Node> closedSet = new HashSet<Node>();

			openSet.Push(startNode);

			while (openSet.Count > 0)
			{
				Node currentNode = openSet.Pop();
				closedSet.Add(currentNode);

				if (currentNode == endNode)
				{
					pathSucess = true;
					break;
				}

				foreach (Node nextNode in currentNode.linkToNode)
				{
					if (closedSet.Contains(nextNode))
						continue;

					int movementCost = currentNode.gCost + GetDistance(currentNode, nextNode);
					if (movementCost < nextNode.gCost || !openSet.Contains(nextNode))
					{
						nextNode.gCost = movementCost;
						nextNode.hCost = GetDistance(currentNode, nextNode);
						nextNode.parent = currentNode;

						if (!openSet.Contains(nextNode))
							openSet.Push(nextNode);
						else
							openSet.UpdateItem(nextNode);
					}
				}
			}

			if (pathSucess)
			{
				return RetracePath(startNode, endNode);
			}
			return null;
		}

		private int GetDistance(Node nodeA, Node nodeB)
		{
			return Mathf.RoundToInt(Vector2.Distance(nodeA.point.transform.position, nodeB.point.transform.position));
		}

		private Node[] RetracePath(Node startNode, Node endNode)
		{
			List<Node> path = new List<Node>();
			Node currentNode = endNode;

			while (currentNode != startNode)
			{
				path.Add(currentNode);
				currentNode = currentNode.parent;
			}

			Node[] waypoints = path.ToArray();
			Array.Reverse(waypoints);
			return waypoints;
		}
		#endregion
	}
}
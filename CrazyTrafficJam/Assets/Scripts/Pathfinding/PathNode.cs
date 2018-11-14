using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace IronSideStudio.CrazyTrafficJam.Pathfinding
{
	public class PathNode : IComparable<PathNode>
	{
		private PathNode parent;
		public PathNode Parent => parent;

		private Vector3 position;
		public Vector3 Position => position;

		private Grid.Node node;

		public Grid.Node Node => node;
		public Grid.ENodeType Type => node.NodeType;

		private int movementCost;
		private int speedCost;
		private int carCost;

		public int MovementCost => movementCost;
		public int SpeedCost => speedCost;
		public int CarCost => carCost;

		public int DistanceToTarget;

		public int TotalCost => movementCost + speedCost + carCost;

		public PathNode(Vector3 position, Grid.Node node)
		{
			this.position = position;
			this.node = node;
			speedCost = 0;
			carCost = 0;
		}

		public void SetParent(PathNode node)
		{
			parent = node;
		}

		public void SetMovementCost(int cost)
		{
			movementCost = cost;
		}

		public void SetDistance(int distance)
		{
			DistanceToTarget = distance;
		}

		public int CompareTo(PathNode nodeToCompare)
		{
			int compare = TotalCost.CompareTo(nodeToCompare.TotalCost);
			if (compare == 0)
			{
				compare = DistanceToTarget.CompareTo(nodeToCompare.DistanceToTarget);
			}
			return -compare;
		}
	}
}
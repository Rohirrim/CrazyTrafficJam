using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace IronSideStudio.CrazyTrafficJam.GridNode
{
	public class GridManager : AManager, IInitializable
	{
		[SerializeField]
		private GridNode gridPrefab;
		[SerializeField]
		private int sizeX;
		[SerializeField]
		private int sizeY;
		private List<GridNode> nodes;

		public override void Construct()
		{
			nodes = new List<GridNode>();
			for (int y = 0 ; y < sizeY ; ++y)
			{
				for (int x = 0 ; x < sizeX ; ++x)
				{
					GridNode node = Instantiate(gridPrefab, transform);
					node.Initialize(x, y);
					nodes.Add(node);
				}
			}
		}

		public void Initialize()
		{
			for (int i = 0 ; i < nodes.Count ; ++i)
			{
				nodes[i].AddOnChangeType(GridChangeType);
			}
		}

		private void GridChangeType(GridNode node)
		{
			switch (node.NodeType)
			{
				case ENodeType.None:
					break;
				case ENodeType.District:
					break;
				case ENodeType.Road:
					break;
				case ENodeType.Intersection:
					break;
				default:
					break;
			}
		}

		public GridNode[] GetGridNodes()
		{
			return nodes.ToArray();
		}

	}
}
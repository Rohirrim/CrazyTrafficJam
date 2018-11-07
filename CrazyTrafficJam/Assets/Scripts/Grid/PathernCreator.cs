using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace IronSideStudio.CrazyTrafficJam.GridNode
{
	public class PathernCreator : AManager, IInitializable, IUpdatable
	{
		[SerializeField]
		private GridPathern[] allPathern;
		private List<GridNode> usedNodes;
		private Vector3Int size;

		private Vector3Int pos;
		private int randPathern;
		private int randNode;

		GridNode useNode;

		public bool Enable { get { return randPathern == -1; } }

		public override void Construct()
		{
			GridManager grid = CoreManager.Instance.GetManager<GridManager>();
			CoreManager.Instance.GetManager<TimeManager>().AddOnWeekPass(TimePass);

			size = new Vector3Int(grid.SizeX, 0, grid.SizeZ);
			usedNodes = new List<GridNode>();
		}

		public void Initialize()
		{
			pos.x = Mathf.CeilToInt(size.x * .5f);
			pos.z = Mathf.CeilToInt(size.z * .5f);

			GridManager grid = CoreManager.Instance.GetManager<GridManager>();
			Vector3 nodePosition = new Vector3();

			for (int z = 0 ; z < size.z ; ++z)
			{
				nodePosition.z = z;
				for (int x = 0 ; x < size.x ; ++x)
				{
					nodePosition.x = x;
					GridNode node = grid.GetNode(nodePosition);
					if (node)
						node.AddOnChangeType(NodeChangeType);
				}
			}

			randPathern = Random.Range(0, allPathern.Length);
			useNode = grid.GetNode(pos);
		}

		public void MUpdate()
		{
			randNode = Random.Range(0, usedNodes.Count);
			randPathern = Random.Range(0, allPathern.Length);

			useNode = usedNodes[randNode];

			if (!allPathern[randPathern].CanInstantiate(useNode.transform.position))
			{
				randPathern = -1;
			}
		}

		private void NodeChangeType(GridNode gridNode)
		{
			if (gridNode.NodeType == ENodeType.District)
			{
				if (!usedNodes.Contains(gridNode))
					usedNodes.Add(gridNode);
			}
			else
			{
				if (usedNodes.Contains(gridNode))
					usedNodes.Remove(gridNode);
			}
		}

		private void TimePass(SDayInfo dayInfo)
		{
			allPathern[randPathern].Apply(useNode.transform.position);
			randPathern = -1;
		}
	}
}
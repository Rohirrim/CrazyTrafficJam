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

		public bool Enable { get { return randPathern == -1; } }

		public override void Construct()
		{
			GridManager grid = CoreManager.Instance.GetManager<GridManager>();
			size = new Vector3Int(grid.SizeX, 0, grid.SizeZ);
			usedNodes = new List<GridNode>();
		}

		public void Initialize()
		{
			pos.x = Mathf.CeilToInt(size.x * .5f);
			pos.z = Mathf.CeilToInt(size.z * .5f);

			CoreManager.Instance.GetManager<TimeManager>().AddOnWeekPass(TimePass);

			GridManager grid = CoreManager.Instance.GetManager<GridManager>();
			GridNode[] allNodes = grid.GetGridNodes();

			foreach (GridNode n in allNodes)
				n.AddOnChangeType(NodeChangeType);

			int rand = Random.Range(0, allPathern.Length);
			allPathern[rand].Apply(pos);
		}

		public void MUpdate()
		{
			randNode = Random.Range(0, usedNodes.Count);
			randPathern = Random.Range(0, allPathern.Length);

			if (!allPathern[randPathern].CanInstantiate(usedNodes[randNode].transform.position))
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
			allPathern[randPathern].Apply(usedNodes[randNode].transform.position);
			randPathern = -1;
		}
	}
}
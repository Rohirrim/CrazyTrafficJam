using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace IronSideStudio.CrazyTrafficJam.Grid
{
	public class PathernCreator : AManager, IInitializable, IUpdatable
	{
		[SerializeField]
		private Pathern[] allPathern;
		private List<Node> usedNodes;
		private Vector3Int size;

		private int randPathern;
		private int randNode;

		Node useNode;

		public bool Enable { get { return randPathern == -1; } }

		public override void Construct()
		{
			Manager grid = GameplayManager.Instance.GetManager<Manager>();
			GameplayManager.Instance.GetManager<TimeManager>().AddOnWeekPass(TimePass);

			size = new Vector3Int(grid.SizeX, 0, grid.SizeZ);
			usedNodes = new List<Node>();
		}

		public void Initialize()
		{
			Manager grid = GameplayManager.Instance.GetManager<Manager>();
			Vector3 nodePosition = new Vector3();
			Vector3Int pos = new Vector3Int(Mathf.CeilToInt(size.x * .5f), 0, Mathf.CeilToInt(size.z * .5f));

			for (int z = 0 ; z < size.z ; ++z)
			{
				nodePosition.z = z;
				for (int x = 0 ; x < size.x ; ++x)
				{
					nodePosition.x = x;
					Node node = grid.GetNode(nodePosition);
					if (node)
						node.AddOnChangeType(NodeChangeType);
				}
			}

			randPathern = Random.Range(0, allPathern.Length);
			useNode = grid.GetNode(pos);
		}

		void IUpdatable.MUpdate()
		{
			randNode = Random.Range(0, usedNodes.Count);
			randPathern = Random.Range(0, allPathern.Length);

			useNode = usedNodes[randNode];

			if (!allPathern[randPathern].CanInstantiate(useNode.transform.position))
			{
				randPathern = -1;
			}
		}

		private void NodeChangeType(Node gridNode)
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
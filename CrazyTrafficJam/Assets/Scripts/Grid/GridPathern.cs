using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace IronSideStudio.CrazyTrafficJam.GridNode
{
	[CreateAssetMenu(fileName = "GridPathern", menuName = "GridPathern")]
	public class GridPathern : ScriptableObject
	{
		[System.Serializable]
		public struct SInfo
		{
			[SerializeField]
			public Vector3Int position;
			[SerializeField]
			public GridNodeType type;
		}

		[SerializeField]
		private int order;
		[SerializeField]
		private SInfo[] nodes;

		private List<SInfo> extremities;
		Vector3Int offset;

		public bool CanInstantiate(Vector3 positionStart)
		{
			RaycastHit hit;
			GridNode gridRoot;
			if (!Physics.Raycast(positionStart + Vector3.up, Vector3.down, out hit))
				return false;
			gridRoot = hit.transform.GetComponent<GridNode>();
			if (gridRoot.NodeType != ENodeType.District)
				return false;

			extremities = new List<SInfo>();
			for (int i = 0 ; i < nodes.Length ; ++i)
			{
				if (nodes[i].type.NodeType == ENodeType.District)
					extremities.Add(nodes[i]);
			}

			extremities.Shuffle();

			foreach (SInfo ext in extremities)
			{
				offset.x = -ext.position.x;
				offset.z = -ext.position.z;
				if (TryExtremity(positionStart + offset))
				{
					return true;
				}
			}

			return false;
		}

		private bool TryExtremity(Vector3 positionStart)
		{
			RaycastHit hit;
			foreach (SInfo n in nodes)
			{
				if (!Physics.Raycast(positionStart + n.position + Vector3.up, Vector3.down, out hit))
					return false;
				GridNode gridNode = hit.transform.GetComponent<GridNode>();
				if (gridNode == null)
					return false;

				switch (n.type.NodeType)
				{
					case ENodeType.None:
						continue;
					case ENodeType.District:
						if (gridNode.NodeType != ENodeType.District && gridNode.NodeType != ENodeType.None)
							return false;
						break;
					case ENodeType.Road:
						if (gridNode.NodeType != ENodeType.None)
							return false;
						break;
					case ENodeType.Intersection:
						if (gridNode.NodeType != ENodeType.Intersection && gridNode.NodeType != ENodeType.Road &&
							gridNode.NodeType != ENodeType.None)
							return false;
						break;
					default:
						break;
				}
			}
			return true;
		}

		public void Apply(Vector3 positionStart)
		{
			RaycastHit hit;

			foreach (SInfo n in nodes)
			{
				if (Physics.Raycast(positionStart + n.position + offset + Vector3.up, Vector3.down, out hit))
				{
					GridNode gridNode = hit.transform.GetComponent<GridNode>();
					if (gridNode == null)
						continue;
					gridNode.SetType(n.type.NodeType);
				}
			}
		}
	}
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace IronSideStudio.CrazyTrafficJam.Grid
{
	[CreateAssetMenu(fileName = "GridPathern", menuName = "GridPathern")]
	public class Pathern : ScriptableObject
	{
		[System.Serializable]
		public struct SInfo
		{
			[SerializeField]
			public Vector3Int position;
			[SerializeField]
			public NodeType type;
		}

		[SerializeField]
		private int order;
		[SerializeField]
		private SInfo[] nodes;

		private List<SInfo> extremities;
		private Vector3Int offset;

		public bool CanInstantiate(Vector3 positionStart)
		{
			RaycastHit hit;
			Node gridRoot;
			if (!Physics.Raycast(positionStart + Vector3.up, Vector3.down, out hit, LayerMask.GetMask(Constante.Layer.GridNode)))
				return false;
			gridRoot = hit.transform.GetComponent<Node>();
			if (gridRoot.NodeType != ENodeType.District)
				return false;

			extremities = new List<SInfo>();
			for (int i = 0 ; i < nodes.Length ; ++i)
			{
				if (nodes[i].type.Type == ENodeType.District)
					extremities.Add(nodes[i]);
			}

			extremities.Shuffle();
			offset = Vector3Int.zero;

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
			positionStart += Vector3.up;
			foreach (SInfo nodePathern in nodes)
			{
				if (!Physics.Raycast(positionStart + nodePathern.position, Vector3.down, out hit, LayerMask.GetMask(Constante.Layer.GridNode)))
					return false;
				Node gridNode = hit.transform.GetComponent<Node>();
				if (gridNode == null)
					return false;

				switch (nodePathern.type.Type)
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
				if (Physics.Raycast(positionStart + n.position + offset + Vector3.up, Vector3.down, out hit, LayerMask.GetMask(Constante.Layer.GridNode)))
				{
					Node gridNode = hit.transform.GetComponent<Node>();
					if (gridNode == null)
						continue;
					if (gridNode.NodeType == ENodeType.Intersection && n.type.Type == ENodeType.Intersection)
						continue;
					n.type.AddNode(gridNode);
				}
			}
		}
	}
}
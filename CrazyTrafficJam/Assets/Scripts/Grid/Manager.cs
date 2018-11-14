using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace IronSideStudio.CrazyTrafficJam.Grid
{
	public class Manager : AManager, IInitializable, IUpdatable
	{
		[SerializeField]
		private Node gridPrefab;
		[SerializeField]
		private Vector2Int size;

		[SerializeField]
		private NodeType[] allTypes;

		private List<Node> gridNodes;
		private List<Node> useNodes;

		public int SizeX => size.x;
		public int SizeZ => size.y;

		public bool Enable { get { return enabled; } }

		public override void Construct()
		{
			gridNodes = new List<Node>();
			useNodes = new List<Node>();

			foreach (NodeType types in allTypes)
			{
				types.Initialize();
			}

			for (int z = 0 ; z < SizeZ ; ++z)
			{
				for (int x = 0 ; x < SizeX ; ++x)
				{
					Node node = Instantiate(gridPrefab, transform);
					node.Initialize(x, z, allTypes);
					gridNodes.Add(node);
				}
			}
		}

		public void Initialize()
		{
			for (int i = 0 ; i < gridNodes.Count ; ++i)
			{
				gridNodes[i].AddOnChangeType(GridChangeType);
			}
			enabled = true;
		}

		public void MUpdate()
		{
			for (int i = 0 ; i < allTypes.Length ; ++i)
			{
				if (allTypes[i].Enable)
					allTypes[i].MUpdate();
			}
		}

		public SaveMap Save(string saveName)
		{
			SaveMap save = new SaveMap() {
				name = "New :)",
				crossPoints = new SaveGridNode[gridNodes.Count]
			};

			for (int i = 0 ; i < useNodes.Count ; ++i)
			{
				save.crossPoints[i] = useNodes[i].Save();
			}

			return save;
		}

		public void Load(SaveMap save)
		{
			for (int i = 0 ; i < save.crossPoints.Length ; ++i)
			{
				SaveGridNode node = save.crossPoints[i];
				RaycastHit hit;

				if (Physics.Raycast(node.position + Vector3.up, Vector3.down, out hit, LayerMask.GetMask(Constante.Layer.GridNode)))
				{
					Node gridNode = hit.transform.GetComponent<Node>();
					if (gridNode == null)
						continue;
					gridNode.SetType(node.type);
				}
			}
		}

		private void GridChangeType(Node node)
		{
			switch (node.NodeType)
			{
				case ENodeType.None:
					useNodes.Remove(node);
					break;
				case ENodeType.District:
					useNodes.Add(node);
					break;
				case ENodeType.Road:
					useNodes.Add(node);
					break;
				case ENodeType.Intersection:
					useNodes.Add(node);
					break;
				default:
					break;
			}
		}

		public Node[] GetGridNodes(bool useNode = false)
		{
			if (useNode)
				return useNodes.ToArray();
			return gridNodes.ToArray();
		}

		public Node GetNode(Vector3 position)
		{
			RaycastHit hit;
			if (Physics.Raycast(position + Vector3.up, Vector3.down, out hit, LayerMask.GetMask(Constante.Layer.GridNode)))
				return hit.transform.GetComponent<Node>();
			return null;
		}
	}
}
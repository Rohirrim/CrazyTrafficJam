using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace IronSideStudio.CrazyTrafficJam.GridNode
{
	public class GridManager : AManager, IInitializable, IUpdatable
	{
		[SerializeField]
		private GridNode gridPrefab;
		[SerializeField]
		private Vector2Int size;

		[SerializeField]
		private GridNodeType[] allTypes;

		private List<GridNode> gridNodes;
		private List<GridNode> useNodes;

		public int SizeX => size.x;
		public int SizeZ => size.y;

		public bool Enable { get { return enabled; } }

		public override void Construct()
		{
			gridNodes = new List<GridNode>();
			useNodes = new List<GridNode>();

			foreach (GridNodeType types in allTypes)
			{
				types.Init();
			}

			for (int z = 0 ; z < SizeZ ; ++z)
			{
				for (int x = 0 ; x < SizeX ; ++x)
				{
					GridNode node = Instantiate(gridPrefab, transform);
					node.Initialize(x, z);
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
				allTypes[i].Update();
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

				if (Physics.Raycast(node.position + Vector3.up, Vector3.down, out hit))
				{
					GridNode gridNode = hit.transform.GetComponent<GridNode>();
					if (gridNode == null)
						continue;
					gridNode.SetType(node.type);
				}
			}
		}

		private void GridChangeType(GridNode node)
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

		public GridNode[] GetGridNodes(bool useNode = false)
		{
			if (useNode)
				return useNodes.ToArray();
			return gridNodes.ToArray();
		}
	}
}
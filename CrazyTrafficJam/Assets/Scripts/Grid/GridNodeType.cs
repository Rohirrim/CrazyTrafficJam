using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace IronSideStudio.CrazyTrafficJam.GridNode
{
	[CreateAssetMenu(fileName = "GridNodeType", menuName = "GridNodeType")]
	public class GridNodeType : ScriptableObject
	{
		[SerializeField]
		private ENodeType nodeType;
		[SerializeField]
		private IGridBehaviour behaviour;
		private HashSet<GridNode> nodes;
		public Color color;

		public ENodeType NodeType => nodeType;

		public void Init()
		{
			nodes = new HashSet<GridNode>();
		}

		public void AddNode(GridNode n)
		{
			nodes.Add(n);
		}

		public void RemoveNode(GridNode n)
		{
			nodes.Remove(n);
		}

		public void Update()
		{
			if (behaviour)
			{
				foreach (GridNode n in nodes)
					behaviour.Behaviour(n);
			}
		}
	}
}
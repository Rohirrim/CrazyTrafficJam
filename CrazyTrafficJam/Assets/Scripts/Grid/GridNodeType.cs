using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace IronSideStudio.CrazyTrafficJam.GridNode
{
	//[CreateAssetMenu(fileName = "GridNodeType", menuName = "GridNodeType")]
	public class GridNodeType : ScriptableObject
	{
		[SerializeField]
		protected ENodeType nodeType;
		protected HashSet<GridNode> nodes;
		public Color color;

		public ENodeType NodeType => nodeType;

		public virtual void Init()
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
			foreach (GridNode n in nodes)
				Behaviour(n);
		}

		protected virtual void Behaviour(GridNode gridNode)
		{ }
	}
}
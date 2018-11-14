using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace IronSideStudio.CrazyTrafficJam.Grid
{
	public enum ENodeType
	{
		None,
		District,
		Road,
		Intersection
	}

	public class NodeType : ScriptableObject, IInitializable, IUpdatable
	{
		[SerializeField]
		protected ENodeType nodeType;
		protected HashSet<Node> nodes;
		[SerializeField]
		private GameObject assetMode;
		public Color color;

		public bool Enable => nodes.Count > 0;

		public ENodeType Type => nodeType;

		public virtual void Initialize()
		{
			nodes = new HashSet<Node>();
		}

		public void AddNode(Node n)
		{
			nodes.Add(n);
			if (assetMode)
			{
				Instantiate(assetMode, n.transform);
			}
		}

		public void RemoveNode(Node n)
		{
			nodes.Remove(n);
		}

		public void MUpdate()
		{
			foreach (Node n in nodes)
				Behaviour(n);
		}

		protected virtual void Behaviour(Node gridNode)
		{ }
	}
}
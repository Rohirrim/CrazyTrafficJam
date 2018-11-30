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
		protected struct SNode
		{
			public Node node;
			public GameObject road;
			public IUpdatable updatable;
		}

		[SerializeField]
		protected ENodeType nodeType;
		protected HashSet<SNode> nodes;
		[SerializeField]
		private GameObject assetMode;
		public Color color;

		public int Count => nodes.Count;
		public bool Enable => nodes.Count > 0;

		public ENodeType Type => nodeType;

		public virtual void Initialize()
		{
			nodes = new HashSet<SNode>();
		}

		public void AddNode(Node n)
		{
			SNode sn = new SNode();
			sn.node = n;
			if (assetMode)
			{
				sn.road = Instantiate(assetMode, n.transform);
				IUpdatable updatable = sn.road.GetComponent<IUpdatable>();
				if (updatable != null)
				{
					sn.updatable = updatable;
				}
			}
			n.SetType(this, sn.road);
			nodes.Add(sn);
		}

		public void RemoveNode(Node n)
		{
			if (!Contains(n))
				return;
			SNode node = GetSNode(n);
			nodes.Remove(node);
			Destroy(node.road);
		}

		public void MUpdate()
		{
			foreach (SNode n in nodes)
			{
				if (n.updatable != null && n.updatable.Enable)
					n.updatable.MUpdate();
			}
		}

		public bool Contains(Node n)
		{
			foreach (SNode node in nodes)
			{
				if (node.node == n)
					return true;
			}
			return false;
		}

		private SNode GetSNode(Node n)
		{
			foreach (SNode node in nodes)
			{
				if (node.node == n)
					return node;
			}
			return new SNode();
		}
	}
}
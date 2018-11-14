using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace IronSideStudio.CrazyTrafficJam.Grid
{
	public class Node : MonoBehaviour
	{
		private NodeType[] allTypes;
		private NodeType currentType;
		public ENodeType NodeType => currentType.Type;

		public void Initialize(int x, int z, NodeType[] nodeTypes)
		{
			name = "GridNode " + x + ", " + z;
			transform.position = new Vector3(x, 0f, z);
			allTypes = nodeTypes;
			SetType(ENodeType.None);
		}

		public Vector3 GetPosition()
		{
			return transform.position;
		}

		public void SetType(ENodeType newType)
		{
			foreach (NodeType t in allTypes)
			{
				if (t.Type == newType)
				{
					currentType?.RemoveNode(this);
					t.AddNode(this);
					currentType = t;
					gameObject.GetComponent<Renderer>().material.color = t.color;
					break;
				}
			}

			InvokeOnChangeType();
		}

		public SaveGridNode Save()
		{
			SaveGridNode save = new SaveGridNode() {
				position = transform.position,
				type = NodeType
			};

			return save;
		}

		#region Events
		public delegate void GridNodeDelegate(Node gridNode);
		private event GridNodeDelegate OnChangeType;

		#region OnChangeType
		public void AddOnChangeType(GridNodeDelegate func)
		{
			OnChangeType += func;
		}

		public void RemoveOnChangeType(GridNodeDelegate func)
		{
			OnChangeType -= func;
		}

		private void InvokeOnChangeType()
		{
			OnChangeType?.Invoke(this);
		}
		#endregion
		#endregion
	}
}
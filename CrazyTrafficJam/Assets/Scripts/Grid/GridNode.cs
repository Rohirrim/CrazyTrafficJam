using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace IronSideStudio.CrazyTrafficJam.GridNode
{
	public class GridNode : MonoBehaviour
	{
		[SerializeField]
		private GridNodeType[] allTypes;
		private GridNodeType currentType;
		public ENodeType NodeType => currentType.NodeType;

		public void Initialize(int x, int z)
		{
			name = "GridNode " + x + ", " + z;
			transform.position = new Vector3(x, 0f, z);
			SetType(ENodeType.None);
		}

		public Vector3 GetPosition()
		{
			return transform.position;
		}

		public void SetType(ENodeType newType)
		{
			foreach (GridNodeType t in allTypes)
			{
				if (t.NodeType == newType)
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
		public delegate void GridNodeDelegate(GridNode gridNode);
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
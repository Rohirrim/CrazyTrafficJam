using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace IronSideStudio.CrazyTrafficJam.Grid
{
	public class Node : MonoBehaviour
	{
		private NodeType currentType;
		public ENodeType NodeType => currentType.Type;

		private IDriveable roadDriveable;

		public void Initialize(int x, int z)
		{
			name = "GridNode " + x + ", " + z;
			transform.position = new Vector3(x, 0f, z);
		}

		public Vector3 GetPosition()
		{
			return transform.position;
		}

		public void SetType(ENodeType newType)
		{
		}

		public void SetType(NodeType nodeType, GameObject obj = null)
		{
			currentType?.RemoveNode(this);
			currentType = nodeType;
			roadDriveable = obj?.GetComponent<IDriveable>();
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

		public Vector3[] GetWaypoint(Vector3 from, Vector3 to)
		{
			if (roadDriveable == null)
				return null;
			return roadDriveable.GetWaypoint(from, to);
		}

		public bool CanDrive(Car.Driver driver)
		{
			if (roadDriveable == null)
				return false;
			return roadDriveable.CanDrive(driver);
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
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

		public void Initialize(int x, int y)
		{
			name = "GridNode " + x + ", " + y;
			transform.position = new Vector3(x, 0f, y);
			SetType(ENodeType.None);

			CoreManager.Instance.GetManager<InputManager>().AddOnTouchDown(MouseDown);
		}

		private void OnDestroy()
		{
			CoreManager.Instance.GetManager<InputManager>().RemoveOnTouchDown(MouseDown);
		}

		private void Update()
		{
			if (currentType.NodeType != ENodeType.None)
				currentType.Update();
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
					currentType = t;
					gameObject.GetComponent<Renderer>().material.color = t.color;
					break;
				}
			}

			InvokeOnChangeType();
		}

		private void MouseDown(SInputTouch touch)
		{
			if (touch.overGUI || touch.gameObject != gameObject)
				return;

			InvokeOnSelect();
		}

		#region Events
		public delegate void GridNodeDelegate(GridNode gridNode);
		private event GridNodeDelegate OnSelect;
		private event GridNodeDelegate OnChangeType;

		#region OnSelect
		public void AddOnSelect(GridNodeDelegate func)
		{
			OnSelect += func;
		}

		public void RemoveOnSelect(GridNodeDelegate func)
		{
			OnSelect -= func;
		}

		private void InvokeOnSelect()
		{
			OnSelect?.Invoke(this);
		}
		#endregion

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
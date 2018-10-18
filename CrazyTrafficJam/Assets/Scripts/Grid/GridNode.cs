﻿using System.Collections;
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

			CoreManager.Instance.GetManager<InputManager>().AddOnTouchClick(TouchDown);
		}

		private void OnDestroy()
		{
			CoreManager.Instance.GetManager<InputManager>().RemoveOnTouchClick(TouchDown);
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

		private void TouchDown(SInputTouch touch)
		{
			if (touch.overGUI || touch.gameObject != gameObject)
				return;

			InvokeOnSelect();
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
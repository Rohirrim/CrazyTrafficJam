using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace IronSideStudio.CrazyTrafficJam
{
	public class InputManager : Manager
	{
		#region Event
		public delegate void OnTouchDelegate(int idFinger, Vector3 position);
		private event OnTouchDelegate OnTouchDown;
		private event OnTouchDelegate OnTouchMove;
		private event OnTouchDelegate OnTouchUp;
		#endregion

		private Vector3 oldPosition;
		private bool isDown;

		public override bool Init()
		{
			base.Init();
			isDown = false;
			return IsInit = true;
		}

		#region Update
		public override void MUpdate()
		{
			UpdatePC();
		}

		private void UpdatePC()
		{
			if (Input.GetMouseButtonDown(0))
			{
				if (EventSystem.current.IsPointerOverGameObject())
					return;
				oldPosition = Input.mousePosition;
				InvokeOnTouchDown(0, oldPosition);
				isDown = true;
			}
			else if (Input.GetMouseButtonUp(0) && isDown)
			{
				if (EventSystem.current.IsPointerOverGameObject())
					return;
				InvokeOnTouchUp(0, Input.mousePosition);
				isDown = false;
			}
			else if (isDown && oldPosition != Input.mousePosition)
			{
				InvokeOnTouchMove(0, Input.mousePosition);
				oldPosition = Input.mousePosition;
			}
		}
		#endregion

		#region Event
		#region OnTouchDown
		public void AddOnTouchDown(OnTouchDelegate func)
		{
			OnTouchDown += func;
		}

		public void RemoveOnTouchDown(OnTouchDelegate func)
		{
			OnTouchDown -= func;
		}

		public void InvokeOnTouchDown(int idFinger, Vector3 position)
		{
			OnTouchDown?.Invoke(idFinger, position);
		}
		#endregion

		#region OnTouchMove
		public void AddOnTouchMove(OnTouchDelegate func)
		{
			OnTouchMove += func;
		}

		public void RemoveOnTouchMove(OnTouchDelegate func)
		{
			OnTouchMove -= func;
		}

		public void InvokeOnTouchMove(int idFinger, Vector3 position)
		{
			OnTouchMove?.Invoke(idFinger, position);
		}
		#endregion

		#region OnTouchUp
		public void AddOnTouchUp(OnTouchDelegate func)
		{
			OnTouchUp += func;
		}

		public void RemoveOnTouchUp(OnTouchDelegate func)
		{
			OnTouchUp -= func;
		}

		public void InvokeOnTouchUp(int idFinger, Vector3 position)
		{
			OnTouchUp?.Invoke(idFinger, position);
		}
		#endregion
		#endregion
	}
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace IronSideStudio.CrazyTrafficJam
{
	public class InputManager : AManager, IUpdatable
	{
		public bool Enable { get { return enabled; } }
		[SerializeField]
		private Camera mainCamera;

		public override void Construct()
		{
			enabled = true;
		}

		public void MUpdate()
		{
#if UNITY_STANDALONE_WIN
			if (Input.GetMouseButtonDown(0))
				TouchDown(Input.mousePosition);
			else if (Input.GetMouseButtonUp(0))
				TouchUp(Input.mousePosition);
			else if (Input.GetMouseButton(0))
				TouchMove(Input.mousePosition);
#endif
		}

		private void TouchDown(Vector3 position)
		{
			InvokeOnTouchDown(CreateTouchData(position));
		}

		private void TouchMove(Vector3 position)
		{
			InvokeOnTouchMove(CreateTouchData(position));
		}

		private void TouchUp(Vector3 position)
		{
			InvokeOnTouchUp(CreateTouchData(position));
		}

		private SInputTouch CreateTouchData(Vector3 position)
		{
			Ray ray = mainCamera.ScreenPointToRay(position);
			RaycastHit hit;
			Physics.Raycast(ray, out hit, 50f);

			SInputTouch touch = new SInputTouch {
				worldPosition = mainCamera.ScreenToWorldPoint(position),
				screenPosition = position,
				overGUI = UnityEngine.EventSystems.EventSystem.current.IsPointerOverGameObject(),
				gameObject = hit.transform ? hit.transform.gameObject : null
			};

			return touch;
		}

		#region Events
		public delegate void InputDelegate(SInputTouch touch);
		private event InputDelegate OnTouchDown;
		private event InputDelegate OnTouchMove;
		private event InputDelegate OnTouchUp;

		#region OnTouchDown
		public void AddOnTouchDown(InputDelegate func)
		{
			OnTouchDown += func;
		}

		public void RemoveOnTouchDown(InputDelegate func)
		{
			OnTouchDown -= func;
		}

		private void InvokeOnTouchDown(SInputTouch touch)
		{
			OnTouchDown?.Invoke(touch);
		}
		#endregion

		#region OnTouchMove
		public void AddOnTouchMove(InputDelegate func)
		{
			OnTouchMove += func;
		}

		public void RemoveOnTouchMove(InputDelegate func)
		{
			OnTouchMove -= func;
		}

		private void InvokeOnTouchMove(SInputTouch touch)
		{
			OnTouchMove?.Invoke(touch);
		}
		#endregion

		#region OnTouchUp
		public void AddOnTouchUp(InputDelegate func)
		{
			OnTouchUp += func;
		}

		public void RemoveOnTouchUp(InputDelegate func)
		{
			OnTouchUp -= func;
		}

		private void InvokeOnTouchUp(SInputTouch touch)
		{
			OnTouchUp?.Invoke(touch);
		}
		#endregion
		#endregion
	}

	public struct SInputTouch
	{
		public Vector3 worldPosition;
		public Vector3 screenPosition;
		public bool overGUI;
		public GameObject gameObject;
	}
}
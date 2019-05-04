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

		private SInputTouch dataDown;
		private SInputTouch dataUp;

		private Vector3 mousePosition;
		private Vector3 oldPosition;

        public float zoomSpeed = 0.5f;
        public float minZoom, maxZoom;


		public override void Construct()
		{
			enabled = true;
		}

		public void MUpdate()
		{
			mousePosition = Input.mousePosition;
#if UNITY_STANDALONE_WIN
			if (Input.GetMouseButtonDown(0))
			{
				oldPosition = mousePosition;
				TouchDown(mousePosition);
			}
			else if (Input.GetMouseButtonUp(0))
			{
				TouchUp(mousePosition);
				if (dataDown.gameObject == dataUp.gameObject && dataDown.overGUI == dataUp.overGUI && dataDown.screenPosition == dataUp.screenPosition)
					TouchClick();
			}
			else if (Input.GetMouseButton(0))
			{
				TouchMove(mousePosition);
			}

			if (Vector3.Distance(mousePosition, oldPosition) <= 10.0f)
			{
				MouseMove(mousePosition);
				oldPosition = mousePosition;
			}

            if(Input.GetAxis("Mouse ScrollWheel") != 0)
            {
                mainCamera.fieldOfView += Input.GetAxis("Mouse ScrollWheel") * zoomSpeed;
                mainCamera.fieldOfView = Mathf.Clamp(mainCamera.fieldOfView, minZoom, maxZoom);
            }
#endif
            if(Input.touchCount == 2)
            {
                Touch touch0 = Input.GetTouch(0);
                Touch touch1 = Input.GetTouch(1);

                Vector2 touch0PreviousPos = touch0.position - touch0.deltaPosition;
                Vector2 touch1PreviousPos = touch1.position - touch1.deltaPosition;

                float prevTouchMagDelta = (touch0PreviousPos - touch1PreviousPos).magnitude;
                float touchMagDelta = (touch0.position - touch1.position).magnitude;

                float deltaMagnitudeDiff = prevTouchMagDelta - touchMagDelta;

                mainCamera.fieldOfView += deltaMagnitudeDiff * zoomSpeed;
                mainCamera.fieldOfView = Mathf.Clamp(mainCamera.fieldOfView, minZoom, maxZoom);
            }
		}

		private void TouchDown(Vector3 position)
		{
			dataDown = CreateTouchData(position);
			InvokeOnTouchDown(dataDown);
		}

		private void TouchMove(Vector3 position)
		{
			InvokeOnTouchMove(CreateTouchData(position));
		}

		private void TouchUp(Vector3 position)
		{
			dataUp = CreateTouchData(position);
			InvokeOnTouchUp(dataUp);
		}

		private void TouchClick()
		{
			InvokeOnTouchClick(dataUp);
		}

		private void MouseMove(Vector3 position)
		{
			InvokeOnTouchUp(CreateTouchData(position));
		}

		private SInputTouch CreateTouchData(Vector3 position)
		{
			Ray ray = mainCamera.ScreenPointToRay(position);
			RaycastHit hit;
			Physics.Raycast(ray, out hit);

			SInputTouch touch = new SInputTouch {
				worldPosition = hit.point,
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
		private event InputDelegate OnTouchClick;

		private event InputDelegate OnMouseMove;

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

		#region OnTouchClick
		public void AddOnTouchClick(InputDelegate func)
		{
			OnTouchClick += func;
		}

		public void RemoveOnTouchClick(InputDelegate func)
		{
			OnTouchClick -= func;
		}

		private void InvokeOnTouchClick(SInputTouch touch)
		{
			OnTouchClick?.Invoke(touch);
		}
		#endregion

		#region OnMouseMove
		public void AddOnMouseMove(InputDelegate func)
		{
			OnMouseMove += func;
		}

		public void RemoveOnMouseMove(InputDelegate func)
		{
			OnMouseMove -= func;
		}

		private void InvokeOnMouseMove(SInputTouch touch)
		{
			OnMouseMove?.Invoke(touch);
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
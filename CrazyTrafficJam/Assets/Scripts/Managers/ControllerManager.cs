using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace IronSideStudio.CrazyTrafficJam
{
	public class ControllerManager : AManager, IInitializable
	{
		[SerializeField]
		private Transform camTrans;
		[SerializeField]
		private float speed;
		private Vector3 oldPosition;

		public bool Enable { get { return enabled; } }

		public override void Construct()
		{
		}

		public void Initialize()
		{
			InputManager input = CoreManager.Instance.GetManager<InputManager>();

			input.AddOnTouchDown(TouchDown);
			input.AddOnTouchMove(TouchMove);
		}

		private void TouchDown(SInputTouch touch)
		{
			oldPosition = touch.screenPosition;
		}

		private void TouchMove(SInputTouch touch)
		{
			Vector3 direction = oldPosition - touch.screenPosition;
			direction.z = direction.y;
			direction.y = 0f;
			camTrans.position += direction * speed;
			oldPosition = touch.screenPosition;
		}
	}
}
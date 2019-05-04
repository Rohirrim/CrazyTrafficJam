using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

namespace IronSideStudio.CrazyTrafficJam
{
	public class ControllerManager : AManager, IInitializable, ICleanable
	{
		[SerializeField]
		private Transform camTrans;
		[SerializeField]
		private float speed;
		private Vector3 oldPosition;

        private Camera mainCam;

        public Slider zoomSlider;

		public bool Enable { get { return enabled; } }

		public override void Construct()
		{
			Grid.Manager gridManager = GameplayManager.Instance.GetManager<Grid.Manager>();

			camTrans.position = new Vector3(gridManager.SizeX * .5f, camTrans.position.y, gridManager.SizeZ * .5f);
		}

		public void Initialize()
		{
			InputManager input = GameplayManager.Instance.GetManager<InputManager>();

			input.AddOnTouchDown(TouchDown);
			input.AddOnTouchMove(TouchMove);

            mainCam = Camera.main;
            InitialiseZoomValue();
		}

		public void Clean()
		{
			InputManager input = GameplayManager.Instance.GetManager<InputManager>();

			input.RemoveOnTouchDown(TouchDown);
			input.RemoveOnTouchMove(TouchMove);
		}

		private void TouchDown(SInputTouch touch)
		{
            if (!EventSystem.current.IsPointerOverGameObject())
            {
                oldPosition = touch.screenPosition;
            }
		}

		private void TouchMove(SInputTouch touch)
		{
            if (!EventSystem.current.IsPointerOverGameObject())
            {
                Vector3 direction = oldPosition - touch.screenPosition;
                direction.z = direction.y;
                direction.y = 0f;
                camTrans.position += direction * speed;
                oldPosition = touch.screenPosition;
            }
        }

        void InitialiseZoomValue()
        {
            zoomSlider.maxValue = GameplayManager.Instance.GetManager<InputManager>().maxZoom;
            zoomSlider.minValue = GameplayManager.Instance.GetManager<InputManager>().minZoom;

            zoomSlider.value = GameplayManager.Instance.GetManager<InputManager>().maxZoom;
            ChangeZoomValue();
        }

        public void ChangeZoomValue()
        {
            mainCam.fieldOfView = zoomSlider.value;
        }
	}
}
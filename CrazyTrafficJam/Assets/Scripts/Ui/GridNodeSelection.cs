using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace IronSideStudio.CrazyTrafficJam.UI
{
	public class GridNodeSelection : MonoBehaviour, IInitializable, ICleanable
	{
        public static GridNodeSelection Instance;

		[SerializeField]
		public Transform typeSelectionTransform;
		private SelectType[] buttonSelection;

		private Grid.Node gridNodeSelected;
		public Grid.Node GridSelected => gridNodeSelected;

		public void Initialize()
		{
            Instance = this;

			GameplayManager.Instance.GetManager<InputManager>().AddOnTouchClick(TouchDown);
			GameplayManager.Instance.GetManager<TimeManager>().AddOnWeekPass(OnTimePass);
			buttonSelection = typeSelectionTransform.GetComponentsInChildren<SelectType>();

			InitButtonType();
			GetComponentInChildren<PanelIntersection>(true).Initialize();
		}

		public void Clean()
		{
			GameplayManager.Instance.GetManager<InputManager>().RemoveOnTouchClick(TouchDown);
			GameplayManager.Instance.GetManager<TimeManager>().RemoveOnWeekPass(OnTimePass);
			OnOpenSelection = null;
		}

		private void InitButtonType()
		{
			foreach (SelectType sNodeType in buttonSelection)
			{
				sNodeType.Initialize();
			}
			typeSelectionTransform.gameObject.SetActive(false);
		}

		private void OnTimePass(SDayInfo dayInfo)
		{
			if (dayInfo.day != EDay.Monday)
				return;
			typeSelectionTransform.gameObject.SetActive(false);
		}

		private void TouchDown(SInputTouch touch)
		{
			if (touch.overGUI)
				return;
			if (touch.gameObject == null)
			{
				typeSelectionTransform.gameObject.SetActive(false);
			}
			else
			{
				gridNodeSelected = touch.gameObject.GetComponent<Grid.Node>();
				if (gridNodeSelected)
				{
					typeSelectionTransform.gameObject.SetActive(gridNodeSelected.NodeType == Grid.ENodeType.Intersection);
					//typeSelectionTransform.position = Camera.main.WorldToScreenPoint(gridNodeSelected.GetPosition() + Vector3.up * .5f);
				}
			}
			InvokeOnOpenSelection(gameObject.activeInHierarchy);
		}

		#region Event
		public delegate void OpenGridNodeSelection(bool isOpen);
		private event OpenGridNodeSelection OnOpenSelection;

		#region OnOpenSelection
		public void AddOnOpenSelection(OpenGridNodeSelection func)
		{
			OnOpenSelection += func;
		}

		public void RemoveOnOpenSelection(OpenGridNodeSelection func)
		{
			OnOpenSelection -= func;
		}

		private void InvokeOnOpenSelection(bool isOpen)
		{
			OnOpenSelection?.Invoke(isOpen);
		}
		#endregion
		#endregion
	}
}
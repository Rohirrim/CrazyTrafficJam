using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace IronSideStudio.CrazyTrafficJam.UI
{
	public class UiGridNodeSelection : MonoBehaviour, IInitializable, ICleanable
	{
		[SerializeField]
		private Button[] typeSelection;
		private GridNode.GridNode gridNodeSelected;

		public void Initialize()
		{
			CoreManager.Instance.GetManager<InputManager>().AddOnTouchClick(TouchDown);

			for (int i = 0 ; i < typeSelection.Length ; ++i)
			{
				int intDelegate = i;
				typeSelection[i].onClick.AddListener(delegate {
					ChangeType((ENodeType)intDelegate);
					gameObject.SetActive(false);
				});
			}
			gameObject.SetActive(false);
		}

		public void Clean()
		{
			CoreManager.Instance.GetManager<InputManager>().RemoveOnTouchClick(TouchDown);
		}

		private void ChangeType(ENodeType nodeType)
		{
			if (gridNodeSelected == null)
				return;
			gridNodeSelected.SetType(nodeType);
		}

		private void TouchDown(SInputTouch touch)
		{
			if (touch.overGUI)
				return;
			if (touch.gameObject == null)
			{
				gameObject.SetActive(false);
				return;
			}
			gridNodeSelected = touch.gameObject.GetComponent<GridNode.GridNode>();
			if (gridNodeSelected)
			{
				gameObject.SetActive(gridNodeSelected.NodeType == ENodeType.Intersection);
				transform.position = Camera.main.WorldToScreenPoint(gridNodeSelected.GetPosition() + Vector3.up * .5f);
			}
		}
	}
}
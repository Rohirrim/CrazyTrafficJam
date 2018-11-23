using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace IronSideStudio.CrazyTrafficJam.UI
{
	public class UiGridNodeSelection : MonoBehaviour, IInitializable, ICleanable
	{
		[System.Serializable]
		private struct SNodeType
		{
			public Button button;
			public Grid.NodeType nodeType;
		}

		[SerializeField]
		private SNodeType[] buttonSelection;
		private Grid.Node gridNodeSelected;

		public void Initialize()
		{
			GameplayManager.Instance.GetManager<InputManager>().AddOnTouchClick(TouchDown);

			foreach (SNodeType sNodeType in buttonSelection)
			{
				if (sNodeType.nodeType == null)
				{
					sNodeType.button.gameObject.SetActive(false);
					continue;
				}
				sNodeType.button.GetComponent<Image>().color = sNodeType.nodeType.color;
				sNodeType.button.onClick.AddListener(delegate {
					ChangeType(sNodeType.nodeType);
					gameObject.SetActive(false);
				});
			}
			gameObject.SetActive(false);
		}

		public void Clean()
		{
			GameplayManager.Instance.GetManager<InputManager>().RemoveOnTouchClick(TouchDown);
		}

		private void ChangeType(Grid.NodeType nodeType)
		{
			if (gridNodeSelected == null)
				return;
			nodeType.AddNode(gridNodeSelected);
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
			gridNodeSelected = touch.gameObject.GetComponent<Grid.Node>();
			if (gridNodeSelected)
			{
				gameObject.SetActive(gridNodeSelected.NodeType == Grid.ENodeType.Intersection);
				transform.position = Camera.main.WorldToScreenPoint(gridNodeSelected.GetPosition() + Vector3.up * .5f);
			}
		}
	}
}
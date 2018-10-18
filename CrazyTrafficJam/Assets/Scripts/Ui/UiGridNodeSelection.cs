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
			CoreManager.Instance.GetManager<InputManager>().AddOnTouchDown(MouseDown);
			GridNode.GridNode[] nodes = CoreManager.Instance.GetManager<GridNode.GridManager>().GetGridNodes();

			for (int i = 0 ; i < nodes.Length ; ++i)
			{
				nodes[i].AddOnSelect(SelectGridNode);
			}

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
			CoreManager.Instance.GetManager<InputManager>().RemoveOnTouchDown(MouseDown);
			GridNode.GridNode[] nodes = CoreManager.Instance.GetManager<GridNode.GridManager>().GetGridNodes();

			for (int i = 0 ; i < nodes.Length ; ++i)
			{
				nodes[i].RemoveOnSelect(SelectGridNode);
			}
		}

		private void SelectGridNode(GridNode.GridNode selectedNode)
		{
			gameObject.SetActive(true);
			gridNodeSelected = selectedNode;
			transform.position = Camera.main.WorldToScreenPoint(selectedNode.GetPosition() + Vector3.up * .5f);
		}

		private void ChangeType(ENodeType nodeType)
		{
			if (gridNodeSelected == null)
				return;
			gridNodeSelected.SetType(nodeType);
		}

		private void MouseDown(SInputTouch touch)
		{
			if (touch.overGUI)
				return;
			if (touch.gameObject == null)
				gameObject.SetActive(false);
		}
	}
}
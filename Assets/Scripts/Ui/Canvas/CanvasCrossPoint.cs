using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace IronSideStudio.CrazyTrafficJam
{
	public class CanvasCrossPoint : MonoBehaviour
	{
		[SerializeField]
		private UiCrossPoint uiPoint;
		[SerializeField]
		private UiCreateCrossPoint uiCreate;

		public void Init()
		{
			Hide();
		}

		public void Hide()
		{
			uiPoint.gameObject.SetActive(false);
			uiCreate.View(false, Vector3.zero);
		}

		public void ViewCreateUi(bool view, Vector3 position)
		{
			uiCreate.View(view, position);
		}

		public void SelectCrossPoint(CrossPoint selection)
		{
			uiPoint.gameObject.SetActive(selection != null);
			if (selection == null)
				return;
			uiPoint.Select(selection);
		}

		public void ViewSelectUi(bool view, Vector3 position)
		{
			uiPoint.gameObject.SetActive(view);
		}
	}
}
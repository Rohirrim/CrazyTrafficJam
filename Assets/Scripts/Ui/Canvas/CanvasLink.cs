using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace IronSideStudio.CrazyTrafficJam
{
	public class CanvasLink : MonoBehaviour
	{
		[SerializeField]
		private GameObject content;
		[SerializeField]
		private Button btnDelete;

		public void Init()
		{
			Hide();
			btnDelete.onClick.AddListener(DeleteLink);
		}

		public void Hide()
		{
			content.SetActive(false);
		}

		public void ViewContent()
		{
			content.SetActive(true);
		}

		private void DeleteLink()
		{
			LinkManager linkManager = CoreManager.Instance.GetManager<LinkManager>();

			linkManager.DeleteLink();
		}
	}
}
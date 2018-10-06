using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace IronSideStudio.CrazyTrafficJam
{
	public class UiCreateCrossPoint : UiCanvas
	{
		[SerializeField]
		private Button btnCreate;

		private CrossPointManager crossManager;

		private void Start()
		{
			crossManager = CoreManager.Instance.GetManager<CrossPointManager>();
			btnCreate.onClick.AddListener(CreateCross);
		}

		private void CreateCross()
		{
			crossManager.CreatePoint();
			gameObject.SetActive(false);
		}
	}
}
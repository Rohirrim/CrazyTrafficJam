using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace IronSideStudio.CrazyTrafficJam
{
	public class UiSaveData : MonoBehaviour
	{
		[SerializeField]
		private Text saveName;
		[SerializeField]
		private Button loadSave;
		[SerializeField]
		private Button deleteSave;

		public void Init(string name)
		{
			saveName.text = name;

			loadSave.onClick.AddListener(delegate {
				CoreManager.Instance.GetManager<SaveManager>().LoadGame(name);
			});
			deleteSave.onClick.AddListener(delegate {
				CoreManager.Instance.GetManager<SaveManager>().DeleteSave(name);
				Destroy(gameObject);
			});
		}
	}
}
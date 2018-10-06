using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;

namespace IronSideStudio.CrazyTrafficJam
{
	public class UiSaveManager : MonoBehaviour
	{
		[SerializeField]
		private UiSaveData loadDataPrefab;
		[SerializeField]
		private Transform contentLoadData;
		[SerializeField]
		private InputField newSaveNameField;
		[SerializeField]
		private Button newSaveButton;

		private SaveManager saveManager;

		public void Init()
		{
			saveManager = CoreManager.Instance.GetManager<SaveManager>();
			newSaveButton.onClick.AddListener(NewSave);
			Refresh(true);
		}

		public void Refresh(bool active = true)
		{
			CleanContent();
			if (!Directory.Exists(saveManager.Path))
				return;

			string[] filesSaveName = Directory.GetFiles(saveManager.Path, "*" + saveManager.Extension);

			foreach (string saveName in filesSaveName)
			{
				FileInfo file = new FileInfo(saveManager.Path + saveName);

				UiSaveData tmp = Instantiate(loadDataPrefab, contentLoadData);
				tmp.Init(Path.GetFileNameWithoutExtension(file.Name));
			}
			gameObject.SetActive(active);
		}

		private void CleanContent()
		{
			UiSaveData[] allUiSave = contentLoadData.GetComponentsInChildren<UiSaveData>();

			for (int i = 0 ; i < allUiSave.Length ; ++i)
				Destroy(allUiSave[i].gameObject);
		}

		public void NewSave()
		{
			if (string.IsNullOrEmpty(newSaveNameField.text))
				return;

			saveManager.LoadGame(newSaveNameField.text);
			newSaveNameField.text = "";
		}
	}
}
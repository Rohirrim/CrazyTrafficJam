using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace IronSideStudio.CrazyTrafficJam
{
	public class UiGamePlay : MonoBehaviour
	{
		[SerializeField]
		private Button saveBtn;
		[SerializeField]
		private Button quitBtn;

		private SaveManager saveManager;

		private void Start()
		{
			saveManager = CoreManager.Instance.GetManager<SaveManager>();
			quitBtn.onClick.AddListener(Quit);
			saveBtn.onClick.AddListener(Save);
		}

		private void Save()
		{
			saveManager.SaveGame();
		}

		private void Quit()
		{
			SceneManager.LoadScene(Constante.Scene.MainMenu);
		}
	}
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace IronSideStudio.CrazyTrafficJam.Menu
{
	public class MainMenu : MonoBehaviour
	{
		public void LaunchGame()
		{
            //UnityEngine.SceneManagement.SceneManager.LoadScene(Constante.Scene.Gameplay);
            UnityEngine.SceneManagement.SceneManager.LoadScene(1);
		}

		public void QuitGame()
		{
			Application.Quit();
		}
	}
}
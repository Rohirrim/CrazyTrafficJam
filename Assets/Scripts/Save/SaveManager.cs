using System.Collections;
using System.Xml.Serialization;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace IronSideStudio.CrazyTrafficJam
{
	public class SaveManager : Manager
	{
		[SerializeField]
		private string path;
		[SerializeField]
		private string extension;
		[SerializeField]
		private UiSaveManager uiSavePrefab;

		public string Path => Application.persistentDataPath + "/" + path;
		public string Extension => extension;

		private string currentSaveName;

		private Save CreateSaveObject(string saveName)
		{
			Save save = new Save() {
				map = core.GetManager<CrossPointManager>().Save(saveName)
			};

			return save;
		}

		public override bool Init()
		{
			base.Init();

			SceneManager.sceneLoaded += OnLoadScene;

			return IsInit = true;
		}

		public override void Clear()
		{
			SceneManager.sceneLoaded -= OnLoadScene;
		}

		private void OnLoadScene(Scene sceneLoaded, LoadSceneMode mode)
		{
			if (sceneLoaded.name == Constante.Scene.MainMenu)
			{
				Instantiate(uiSavePrefab).Init();
				currentSaveName = "";
			}
		}

		public void DeleteSave(string saveName)
		{
			if (!File.Exists(Path + saveName + extension))
				return;
			File.Delete(Path + saveName + extension);
		}

		#region save
		[ContextMenu("SaveMap")]
		public void SaveGame()
		{
			if (string.IsNullOrEmpty(currentSaveName))
				SaveGame("default");
			else
				SaveGame(currentSaveName);
		}

		public void SaveGame(string saveName)
		{
			if (!Directory.Exists(Path))
				Directory.CreateDirectory(Path);

			Save save = CreateSaveObject(saveName);
			XmlSerializer serializer = new XmlSerializer(save.GetType());
			StreamWriter writer = new StreamWriter(Path + saveName + extension);
			serializer.Serialize(writer.BaseStream, save);
			writer.Close();
		}
		#endregion

		#region load
		[ContextMenu("LoadMap")]
		private void LoadGame()
		{
			LoadGame("default");
		}

		public void LoadGame(string saveName)
		{
			currentSaveName = saveName;
			if (File.Exists(Path + saveName + extension))
			{
				XmlSerializer serializer = new XmlSerializer(typeof(Save));
				StreamReader reader = new StreamReader(Path + saveName + extension);
				Save save = (Save)serializer.Deserialize(reader.BaseStream);
				reader.Close();
				StartCoroutine(LoadGameplay(save));
			}
			else
				SceneManager.LoadScene(Constante.Scene.Gameplay);

		}

		private void LoadGame(Save save)
		{
			core.GetManager<CrossPointManager>().Load(save.map);
			core.GetManager<PathfindingManager>().Load(save.map);
		}

		private IEnumerator LoadGameplay(Save save)
		{
			AsyncOperation loadScene = SceneManager.LoadSceneAsync(Constante.Scene.Gameplay);
			yield return loadScene;
			LoadGame(save);
		}
		#endregion
	}
}
using System.Collections;
using System.Xml.Serialization;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace IronSideStudio.CrazyTrafficJam
{
	public class SaveManager : AManager
	{
		[SerializeField]
		private string path;
		[SerializeField]
		private string extension;

		public string Path => Application.persistentDataPath + "/" + path;
		public string Extension => extension;

		private string currentSaveName;

		public override void Construct()
		{
		}

		private Save CreateSaveObject(string saveName)
		{
			Save save = new Save() {
				map = CoreManager.Instance.GetManager<GridNode.GridManager>().Save(saveName)
			};

			return save;
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
			LoadGame("default2");
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
				LoadGame(save);
			}
		}

		private void LoadGame(Save save)
		{
			CoreManager.Instance.GetManager<GridNode.GridManager>().Load(save.map);
		}
		#endregion
	}
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace IronSideStudio.CrazyTrafficJam
{
	public class MapEditorManager : Manager
	{
		public enum EMode
		{
			None,
			Cross,
			Link
		}

		#region Event
		public delegate void OnSetModeDelegate(EMode mode);
		private event OnSetModeDelegate OnSetMode;
		#endregion

		[SerializeField]
		private CanvasMapEditor canvasEditorPrefab;

		private EMode mode;
		public EMode Mode { get { return mode; } private set { mode = value; InvokeOnSetMode(); } }

		public override bool Init()
		{
			base.Init();
			mode = EMode.None;

			SceneManager.sceneLoaded += OnLoadScene;
			SceneManager.sceneUnloaded += OnUnloadScene;

			return IsInit = true;
		}

		public override void Clear()
		{
			OnSetMode = null;
			SceneManager.sceneLoaded -= OnLoadScene;
			SceneManager.sceneUnloaded -= OnUnloadScene;
		}

		private void OnLoadScene(Scene sceneLoaded, LoadSceneMode mode)
		{
			if (sceneLoaded.name != Constante.Scene.Gameplay)
				return;
			CanvasMapEditor canvas = Instantiate(canvasEditorPrefab);
			canvas.Init();
			Mode = EMode.Cross;
		}

		private void OnUnloadScene(Scene sceneUnloaded)
		{
			if (sceneUnloaded.name != Constante.Scene.Gameplay)
				return;
			Mode = EMode.None;
		}

		public void SetMode(EMode newMode)
		{
			if (newMode == mode)
				return;
			Mode = newMode;
		}

		#region Event
		public void AddOnSetMode(OnSetModeDelegate func)
		{
			OnSetMode += func;
		}

		public void RemoveOnSetMode(OnSetModeDelegate func)
		{
			OnSetMode -= func;
		}

		private void InvokeOnSetMode()
		{
			OnSetMode?.Invoke(mode);
		}
		#endregion
	}
}
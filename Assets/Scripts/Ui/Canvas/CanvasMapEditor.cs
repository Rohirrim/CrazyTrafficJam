using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace IronSideStudio.CrazyTrafficJam
{
	public class CanvasMapEditor : MonoBehaviour
	{
		[SerializeField]
		private Toggle tglCrossMode;
		[SerializeField]
		private Toggle tglLinkMode;
		private MapEditorManager editorManager;

		public void Init()
		{
			editorManager = CoreManager.Instance.GetManager<MapEditorManager>();

			tglCrossMode.onValueChanged.AddListener((value) => ValueChange(value, MapEditorManager.EMode.Cross));
			tglLinkMode.onValueChanged.AddListener((value) => ValueChange(value, MapEditorManager.EMode.Link));
		}

		private void ValueChange(bool value, MapEditorManager.EMode mode)
		{
			if (value == false)
				return;

			editorManager.SetMode(mode);
		}
	}
}
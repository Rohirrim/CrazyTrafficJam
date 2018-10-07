using System.Reflection;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace IronSideStudio.CrazyTrafficJam.GridNode
{
	[CustomEditor(typeof(GridNodeType))]
	public class GridNodeTypeEditor : Editor
	{
		private GridNodeType obj;

		private void Awake()
		{
		}

		public override void OnInspectorGUI()
		{
			base.OnInspectorGUI();
		}
	}
}
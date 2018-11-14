using System.Reflection;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace IronSideStudio.CrazyTrafficJam.Grid
{
	[CustomEditor(typeof(NodeType))]
	public class NodeTypeEditor : Editor
	{
		private NodeType obj;

		private void Awake()
		{
		}

		public override void OnInspectorGUI()
		{
			base.OnInspectorGUI();
		}
	}
}
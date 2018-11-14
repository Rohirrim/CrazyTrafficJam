using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace IronSideStudio.CrazyTrafficJam.GridEditor
{
	public class WindowGridPathern : EditorWindow
	{
		[MenuItem("Window/Pathern")]
		private static void Init()
		{
			GetWindow(typeof(WindowGridPathern)).Show();
		}

		private List<Grid.Pathern> allPathern;
		private List<Grid.NodeType> allNodeType;

		private ViewGridPathern currentPathern;

		private GUIStyle skinButton;

		private void Awake()
		{
			allPathern = new List<Grid.Pathern>();
			allNodeType = new List<Grid.NodeType>();

			var files = Directory.GetFiles(Application.dataPath + "/Pathern", "*.asset");
			for (int i = 0 ; i < files.Length ; ++i)
			{
				string fileName = files[i].Substring(files[i].LastIndexOf('/') + 1).Replace('\\', '/');
				Grid.Pathern pathern = AssetDatabase.LoadAssetAtPath<Grid.Pathern>("Assets/" + fileName);
				allPathern.Add(pathern);
			}

			files = Directory.GetFiles(Application.dataPath + "/GridNodeType", "*.asset", SearchOption.TopDirectoryOnly);
			for (int i = 0 ; i < files.Length ; ++i)
			{
				string fileName = files[i].Substring(files[i].LastIndexOf('/') + 1).Replace('\\', '/');
				Grid.NodeType gridType = AssetDatabase.LoadAssetAtPath<Grid.NodeType>("Assets/" + fileName);
				allNodeType.Add(gridType);
			}

		}

		private void OnGUI()
		{
			if (allPathern == null)
				Awake();

			skinButton = new GUIStyle(GUI.skin.button) {
				fixedWidth = 100f,
				fixedHeight = 100f,
				alignment = TextAnchor.LowerCenter
			};


			GUILayout.Label("Choose a Pathern", EditorStyles.boldLabel);

			GUILayout.BeginHorizontal();
			for (int i = 0 ; i < allPathern.Count ; ++i)
			{
				if (GUILayout.Button(allPathern[i].name, skinButton))
					currentPathern = new ViewGridPathern(allPathern[i]);
			}
			GUILayout.EndHorizontal();

			DrawLine();

			GUILayout.BeginHorizontal();
			for (int i = 0 ; i < allNodeType.Count ; ++i)
			{
				GUILayout.Button(allNodeType[i].name, skinButton);
			}
			GUILayout.EndHorizontal();

			DrawLine(10);

			if (currentPathern != null)
				currentPathern.Draw();
		}

		private void DrawLine(int height = 1)
		{
			Rect rect = EditorGUILayout.GetControlRect(false, height);
			rect.height = height;
			EditorGUI.DrawRect(rect, new Color(0.5f, 0.5f, 0.5f, 1));
		}
	}

	public class ViewGridPathern
	{
		private SerializedObject currentPathern;
		private SerializedProperty nodesArray;

		private Vector2Int size;
		private Grid.NodeType[,] grid;
		private GUIStyle skinGrid;

		public ViewGridPathern(Grid.Pathern pathern)
		{
			currentPathern = new SerializedObject(pathern);

			nodesArray = currentPathern.FindProperty("nodes");
			Vector3Int propertyPosition;

			for (int i = 0 ; i < nodesArray.arraySize ; ++i)
			{
				propertyPosition = nodesArray.GetArrayElementAtIndex(i).FindPropertyRelative("position").vector3IntValue;
				++propertyPosition.x;
				++propertyPosition.z;

				if (propertyPosition.x > size.x)
					size.x = propertyPosition.x;
				if (propertyPosition.z > size.y)
					size.y = propertyPosition.z;
			}
			if (size.x < 1)
				size.x = 1;
			if (size.y < 1)
				size.y = 1;
			grid = new Grid.NodeType[size.x, size.y];

			skinGrid = new GUIStyle(GUI.skin.button) {
				fixedWidth = 50f,
				fixedHeight = 50f
			};

		}

		public void Draw()
		{
			DrawSize();
			DrawGrid();
		}

		private void DrawSize()
		{
			EditorGUI.BeginChangeCheck();
			Vector2Int oldSize = size;
			size = EditorGUILayout.Vector2IntField("size", size);
			if (EditorGUI.EndChangeCheck())
			{
				if (size.x < 1)
					size.x = 1;
				if (size.y < 1)
					size.y = 1;
				ChangeSize(oldSize);
			}
		}

		private void ChangeSize(Vector2Int oldSize)
		{
			Grid.NodeType[,] n = new Grid.NodeType[size.x, size.y];

			for (int y = 0 ; y < size.y && y < oldSize.y ; ++y)
			{
				for (int x = 0 ; x < size.x && x < oldSize.x ; ++x)
				{
					n[x, y] = grid[x, y];
				}
			}
			grid = n;
		}

		private void DrawGrid()
		{
			for (int y = 0 ; y < size.y ; ++y)
			{
				GUILayout.BeginHorizontal();
				for (int x = 0 ; x < size.x ; ++x)
				{
					GUILayout.Button(x + ", " + y, skinGrid);
				}
				GUILayout.EndHorizontal();
			}
		}
	}
}
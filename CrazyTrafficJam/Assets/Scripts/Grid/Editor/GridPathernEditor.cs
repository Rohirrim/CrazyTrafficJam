using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace IronSideStudio.CrazyTrafficJam.GridNode
{
	[CustomEditor(typeof(GridPathern))]
	public class GridPathernEditor : Editor
	{
		private Vector2Int size;
		private SerializedProperty nodesArray;
		private GridNodeType[,] grid;

		private Vector2 sizeNode;
		private Vector2 offset;
		private Vector2 space;

		private void Awake()
		{
			Init();

		}

		private void Init()
		{
			offset.x = 15f;
			offset.y = 80f;
			space.x = 1f;
			space.y = 1f;

			nodesArray = serializedObject.FindProperty("nodes");
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
			grid = new GridNodeType[size.x, size.y];

			for (int i = 0 ; i < nodesArray.arraySize ; ++i)
			{
				propertyPosition = nodesArray.GetArrayElementAtIndex(i).FindPropertyRelative("position").vector3IntValue;
				GridNodeType nodeType = (GridNodeType)nodesArray.GetArrayElementAtIndex(i).FindPropertyRelative("type").objectReferenceValue;
				grid[propertyPosition.x, propertyPosition.z] = nodeType;
			}
			sizeNode.x = (Screen.width - offset.x * 2f) / size.x;
			sizeNode.x -= space.x * (size.x - 1);
			if (sizeNode.x > 100f)
				sizeNode.x = 100f;
			sizeNode.y = sizeNode.x;
		}

		public override void OnInspectorGUI()
		{
			if (nodesArray == null)
				Init();
			CustomInspector();
		}

		private void CustomInspector()
		{
			SetSize();
			if (DrawNode())
				Save();
		}

		private void SetSize()
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
			sizeNode.x = (Screen.width - offset.x * 2f) / size.x;
			if (sizeNode.x > 100f)
				sizeNode.x = 100f;
			sizeNode.y = sizeNode.x;
		}

		private void ChangeSize(Vector2Int oldSize)
		{
			GridNodeType[,] n = new GridNodeType[size.x, size.y];

			for (int y = 0 ; y < size.y && y < oldSize.y ; ++y)
			{
				for (int x = 0 ; x < size.x && x < oldSize.x ; ++x)
				{
					n[x, y] = grid[x, y];
				}
			}
			grid = n;
		}

		private bool DrawNode()
		{
			Event ev = Event.current;
			Vector2 mousePosition = ev.mousePosition;
			bool change = false;

			for (int y = 0 ; y < size.y ; ++y)
			{
				for (int x = 0 ; x < size.x ; ++x)
				{
					Rect r = new Rect(offset.x + (sizeNode.x + space.x) * x, offset.y + y * (sizeNode.y + space.y), sizeNode.x, sizeNode.y);
					Color c = grid[x, y] != null ? grid[x, y].color : Color.white;

					switch (ev.type)
					{
						case EventType.DragUpdated:
						case EventType.DragPerform:
							if (!r.Contains(mousePosition))
								break;
							if (DragAndDrop.objectReferences.Length != 1)
								break;
							GridNodeType dragType = (GridNodeType)DragAndDrop.objectReferences[0];
							if (dragType == null)
								break;
							DragAndDrop.visualMode = DragAndDropVisualMode.Link;
							c = dragType.color;
							if (ev.type == EventType.DragPerform)
							{
								change = true;
								DragAndDrop.AcceptDrag();
								grid[x, y] = dragType;
							}
							ev.Use();
							break;
					}
					EditorGUI.DrawRect(r, c);
				}
			}
			return change;
		}

		private void Save()
		{
			int i = 0;
			nodesArray.ClearArray();

			for (int z = 0 ; z < size.y ; ++z)
			{
				for (int x = 0 ; x < size.x ; ++x)
				{
					if (grid[x, z] == null || grid[x, z].NodeType == ENodeType.None)
						continue;
					nodesArray.InsertArrayElementAtIndex(i);
					nodesArray.GetArrayElementAtIndex(i).FindPropertyRelative("position").vector3IntValue = new Vector3Int(x, 0, z);
					nodesArray.GetArrayElementAtIndex(i).FindPropertyRelative("type").objectReferenceValue = grid[x, z];
					++i;
				}
			}

			serializedObject.ApplyModifiedProperties();
		}
	}
}
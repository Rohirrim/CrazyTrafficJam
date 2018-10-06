using System.Xml;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace IronSideStudio.CrazyTrafficJam
{
	public class CrossPointManager : Manager
	{
		#region Event
		public delegate void OnSelectCrossDelegate(CrossPoint selection);
		private event OnSelectCrossDelegate OnSelectCrossRoad;
		#endregion

		[SerializeField]
		private CrossPoint crossPrefab;
		[SerializeField]
		private CanvasCrossPoint canvasManagerPrefab;

		private CrossPoint currentPoint;
		private CanvasCrossPoint currentCanvas;

		private Transform contentPoint;
		private HashSet<CrossPoint> allPoint;
		private Vector2 worldPosition;
		private HashSet<int> usedId;
		private int id;

		public override bool Init()
		{
			base.Init();
			allPoint = new HashSet<CrossPoint>();
			usedId = new HashSet<int>();

			MapEditorManager mapEditor = core.GetManager<MapEditorManager>();

			SceneManager.sceneLoaded += OnLoadScene;
			SceneManager.sceneUnloaded += OnUnloadScene;
			mapEditor.AddOnSetMode(OnChangeEditorMode);
			return IsInit = true;
		}

		public override void Clear()
		{
			MapEditorManager mapEditor = core.GetManager<MapEditorManager>();

			SceneManager.sceneLoaded -= OnLoadScene;
			SceneManager.sceneUnloaded -= OnUnloadScene;
			mapEditor.RemoveOnSetMode(OnChangeEditorMode);
		}

		private void OnLoadScene(Scene sceneLoaded, LoadSceneMode mode)
		{
			if (sceneLoaded.name != Constante.Scene.Gameplay)
				return;
			allPoint.Clear();
			usedId.Clear();
			contentPoint = new GameObject("ContentCrossPoint").transform;
			currentCanvas = Instantiate(canvasManagerPrefab);
			currentCanvas.Init();
		}

		private void OnUnloadScene(Scene sceneUnloaded)
		{
			if (sceneUnloaded.name != Constante.Scene.Gameplay)
				return;

			contentPoint = null;
			currentCanvas = null;
		}

		private void OnChangeEditorMode(MapEditorManager.EMode mode)
		{
			currentCanvas?.Hide();
			InputManager manager = core.GetManager<InputManager>();
			switch (mode)
			{
				case MapEditorManager.EMode.None:
					manager.RemoveOnTouchDown(OnTouchDown);
					manager.RemoveOnTouchMove(OnTouchMove);
					manager.RemoveOnTouchUp(OnTouchUp);
					break;
				case MapEditorManager.EMode.Cross:
					manager.AddOnTouchDown(OnTouchDown);
					manager.AddOnTouchMove(OnTouchMove);
					manager.AddOnTouchUp(OnTouchUp);
					break;
				case MapEditorManager.EMode.Link:
					manager.RemoveOnTouchDown(OnTouchDown);
					manager.RemoveOnTouchMove(OnTouchMove);
					manager.RemoveOnTouchUp(OnTouchUp);
					break;
				default:
					manager.RemoveOnTouchDown(OnTouchDown);
					manager.RemoveOnTouchMove(OnTouchMove);
					manager.RemoveOnTouchUp(OnTouchUp);
					break;
			}
		}

		#region Save&Load
		public SaveMap Save(string saveName)
		{
			SaveMap newMap = new SaveMap() {
				name = saveName,
				crossPoints = new SaveCrossPoint[allPoint.Count]
			};

			int i = 0;
			foreach (CrossPoint point in allPoint)
			{
				newMap.crossPoints[i++] = point.Save();
			}

			return newMap;
		}

		public void Load(SaveMap save)
		{
			int i = 0;

			for (; i < save.crossPoints.Length ; ++i)
			{
				CrossPoint loadedCross = Instantiate(crossPrefab, contentPoint);
				allPoint.Add(loadedCross);
				usedId.Add(save.crossPoints[i].id);
				loadedCross.Init(save.crossPoints[i].id);
			}

			i = 0;
			foreach (CrossPoint point in allPoint)
			{
				point.Load(this, save.crossPoints[i++]);
			}
		}
		#endregion

		#region OnTouch
		private void OnTouchDown(int idFinger, Vector3 position)
		{
			currentCanvas.Hide();
			worldPosition = Camera.main.ScreenToWorldPoint(position);

			RaycastHit2D hit = Physics2D.Raycast(worldPosition, Vector2.zero, 10f, LayerMask.GetMask(Constante.Layer.CrossPoint));
			if (hit.transform)
			{
				currentPoint = hit.transform.GetComponent<CrossPoint>();
			}
			else
				currentPoint = null;
		}

		private void OnTouchMove(int idFinger, Vector3 position)
		{
			if (currentPoint == null)
				return;

			worldPosition = Camera.main.ScreenToWorldPoint(position);
			currentPoint.UpdatePosition(worldPosition);
		}

		private void OnTouchUp(int idFinger, Vector3 position)
		{
			if (currentPoint)
			{
				currentCanvas.SelectCrossPoint(currentPoint);
			}
			else
			{
				worldPosition = Camera.main.ScreenToWorldPoint(position);

				RaycastHit2D hit = Physics2D.Raycast(worldPosition, Vector2.zero, 10f, LayerMask.GetMask(Constante.Layer.CrossPoint));
				if (hit.transform == null)
				{
					currentCanvas.ViewCreateUi(true, position);
				}
			}
		}
		#endregion

		#region ManageCrossPoint
		public void CreatePoint()
		{
			CreatePoint(worldPosition);
		}

		public void CreatePoint(Vector2 position)
		{
			currentPoint = Instantiate(crossPrefab, position, Quaternion.identity, contentPoint);
			while (!usedId.Add(id))
				++id;
			currentPoint.Init(id++);
			allPoint.Add(currentPoint);
		}

		public CrossPoint GetCross(int id)
		{
			foreach (CrossPoint cross in allPoint)
			{
				if (cross.Id == id)
					return cross;
			}
			return null;
		}

		public void DeletePoint()
		{
			if (currentPoint == null)
				return;
			DeletePoint(currentPoint);
			currentPoint = null;
		}

		public void DeletePoint(CrossPoint point)
		{
			allPoint.Remove(point);
			point.Delete();
		}
		#endregion

		#region Event
		public void AddOnSelectCrossRoad(OnSelectCrossDelegate func)
		{
			OnSelectCrossRoad += func;
		}

		public void RemoveOnSelectCrossRoad(OnSelectCrossDelegate func)
		{
			OnSelectCrossRoad -= func;
		}

		private void InvokeOnSelectCrossRoad()
		{
			OnSelectCrossRoad?.Invoke(currentPoint);
		}
		#endregion
	}
}
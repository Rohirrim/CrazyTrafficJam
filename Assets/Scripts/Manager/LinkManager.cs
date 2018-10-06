using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace IronSideStudio.CrazyTrafficJam
{
	public class LinkManager : Manager
	{
		[SerializeField]
		private Link linkPrefab;
		private CrossPoint downPoint;
		private CrossPoint upPoint;

		[SerializeField]
		private CanvasLink canvasLinkPrefab;
		private CanvasLink currentCanvas;

		private Transform contentLink;
		private List<Link> allLink;
		private Link currentLink;

		[SerializeField]
		private LineRenderer currentLine;

		public override bool Init()
		{
			base.Init();
			allLink = new List<Link>();
			MapEditorManager mapEditor = core.GetManager<MapEditorManager>();

			SceneManager.sceneLoaded += OnLoadScene;
			SceneManager.sceneUnloaded += OnUnloadScene;
			mapEditor.AddOnSetMode(OnChangeEditorMode);
			currentLine.enabled = false;

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
			contentLink = new GameObject("ContentLink").transform;
			currentLine.enabled = false;
			currentCanvas = Instantiate(canvasLinkPrefab);
			currentCanvas.Init();
			currentLink = null;
		}

		private void OnUnloadScene(Scene sceneUnloaded)
		{
			if (sceneUnloaded.name != Constante.Scene.Gameplay)
				return;

			allLink.Clear();
			contentLink = null;
			currentLine.enabled = false;
			currentCanvas = null;
			currentLink = null;
		}

		private void OnChangeEditorMode(MapEditorManager.EMode mode)
		{
			currentCanvas?.Hide();
			currentLink = null;
			InputManager manager = core.GetManager<InputManager>();
			switch (mode)
			{
				case MapEditorManager.EMode.None:
					manager.RemoveOnTouchDown(OnTouchDown);
					manager.RemoveOnTouchMove(OnTouchMove);
					manager.RemoveOnTouchUp(OnTouchUp);
					break;
				case MapEditorManager.EMode.Cross:
					manager.RemoveOnTouchDown(OnTouchDown);
					manager.RemoveOnTouchMove(OnTouchMove);
					manager.RemoveOnTouchUp(OnTouchUp);
					break;
				case MapEditorManager.EMode.Link:
					manager.AddOnTouchDown(OnTouchDown);
					manager.AddOnTouchMove(OnTouchMove);
					manager.AddOnTouchUp(OnTouchUp);
					break;
				default:
					manager.RemoveOnTouchDown(OnTouchDown);
					manager.RemoveOnTouchMove(OnTouchMove);
					manager.RemoveOnTouchUp(OnTouchUp);
					break;
			}
		}

		public void UpdateRenderer()
		{
			foreach (Link link in allLink)
			{
				link.UpdateRenderer();
			}
		}

		#region OnTouch
		private void OnTouchDown(int idFinger, Vector3 position)
		{
			currentCanvas.Hide();
			currentLink = null;
			Vector2 worldPosition = Camera.main.ScreenToWorldPoint(position);

			RaycastHit2D hit = Physics2D.Raycast(worldPosition, Vector2.zero, 10f, LayerMask.GetMask(Constante.Layer.CrossPoint, Constante.Layer.Link));
			if (hit.transform)
			{
				downPoint = hit.transform.GetComponent<CrossPoint>();
				if (downPoint)
				{
					currentLine.enabled = true;
					currentLine.SetPosition(0, hit.transform.position);
					currentLine.SetPosition(1, worldPosition);
				}
			}
			else
			{
				downPoint = null;
			}
		}

		private void OnTouchMove(int idFinger, Vector3 position)
		{
			if (downPoint == null)
				return;

			Vector2 worldPosition = Camera.main.ScreenToWorldPoint(position);
			currentLine.SetPosition(1, worldPosition);
		}

		private void OnTouchUp(int idFinger, Vector3 position)
		{
			currentLine.enabled = false;
			Vector2 worldPosition = Camera.main.ScreenToWorldPoint(position);
			if (downPoint != null)
			{

				RaycastHit2D hit = Physics2D.Raycast(worldPosition, Vector2.zero, 10f, LayerMask.GetMask(Constante.Layer.CrossPoint));
				if (hit.transform)
				{
					upPoint = hit.transform.GetComponent<CrossPoint>();
					downPoint.AddLink(upPoint);
				}
			}
			else
			{
				RaycastHit2D hit = Physics2D.Raycast(worldPosition, Vector2.zero, 10f, LayerMask.GetMask(Constante.Layer.Link));
				if (hit.transform)
				{
					currentLink = hit.transform.GetComponent<Link>();
					currentCanvas.ViewContent();
				}
			}
		}
		#endregion

		public Link CreateLink(CrossPoint a, CrossPoint b)
		{
			Link newLink = Instantiate(linkPrefab, contentLink);
			newLink.Init(a, b);
			allLink.Add(newLink);
			return newLink;
		}

		public void DeleteLink()
		{
			if (currentLink)
				DeleteLink(currentLink);
			currentLink = null;
			currentCanvas?.Hide();
		}

		public void DeleteLink(Link linkToDelete)
		{
			linkToDelete.Delete();
		}
	}
}
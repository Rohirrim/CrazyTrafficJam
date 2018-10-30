using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace IronSideStudio.CrazyTrafficJam.Car
{
	[CreateAssetMenu(fileName = "Car", menuName = "Car")]
	public class CarSpawner : ScriptableObject
	{
		[System.Serializable]
		private struct SCar
		{
			public GameObject car;
			[Range(1, 100)]
			public int probability;
			public GridNode.GridNode desination;
		}

		[SerializeField]
		private SCar[] carPrefab;
		private List<GridNode.GridNode> districts;

		public void Init()
		{
			GridNode.GridManager grid = CoreManager.Instance.GetManager<GridNode.GridManager>();
			districts = new List<GridNode.GridNode>();
			RaycastHit hit;
			Vector3 raycastStart = new Vector3();

			for (int y = 0 ; y < grid.SizeZ ; ++y)
			{
				raycastStart.y = y;
				for (int x = 0 ; x < grid.SizeX ; ++x)
				{
					raycastStart.x = x;
					if (!Physics.Raycast(raycastStart + Vector3.up, Vector3.down, out hit, LayerMask.GetMask(Constante.Layer.GridNode)))
						continue;
					hit.transform.GetComponent<GridNode.GridNode>().AddOnChangeType(AddDesination);
				}
			}
		}

		private void AddDesination(GridNode.GridNode node)
		{
			if (node.NodeType != ENodeType.District)
				return;

			districts.Add(node);
		}

		public GameObject Spawn()
		{
			int sum = Random.Range(1, 101);

			for (int i = 0 ; i < carPrefab.Length ; ++i)
			{
				if (sum < carPrefab[i].probability)
					return null;
			}

			int index = Random.Range(0, carPrefab.Length);

			return Instantiate(carPrefab[index].car);
		}

		public GameObject Spawn(GridNode.GridNode node)
		{
			int index = Random.Range(0, carPrefab.Length);
			GameObject obj = Instantiate(carPrefab[index].car);

			var p = Pathfinding.PathFinder.GetPath(node.GetPosition(), Vector3.up);

			Debug.Log("Start path");

			foreach (var d in p)
			{
				Debug.Log(d);
			}
			Debug.Log("End path");

			return obj;
		}

		#region events
		public delegate void SpawnCar(GameObject car);
		private event SpawnCar OnSpawn;

		#region OnSpawn
		public void AddOnSpawn(SpawnCar func)
		{
			OnSpawn += func;
		}

		public void RemoveOnSpawn(SpawnCar func)
		{
			OnSpawn -= func;
		}

		public void InvokeOnSpawn()
		{
			OnSpawn?.Invoke(null);
		}
		#endregion
		#endregion
	}
}
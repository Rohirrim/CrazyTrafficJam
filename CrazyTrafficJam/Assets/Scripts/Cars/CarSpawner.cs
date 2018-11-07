using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace IronSideStudio.CrazyTrafficJam.Car
{
	[CreateAssetMenu(fileName = "Car", menuName = "Car")]
	public class CarSpawner : ScriptableObject, IInitializable
	{
		[System.Serializable]
		private struct SCar
		{
			public CarBehaviour car;
			[Range(1, 100)]
			public int probability;
			public GridNode.GridNode desination;
		}

		[SerializeField]
		private SCar[] carPrefab;
		[SerializeField]
		private List<GridNode.GridNode> districts;

		public void Initialize()
		{
			GridNode.GridManager grid = CoreManager.Instance.GetManager<GridNode.GridManager>();
			districts = new List<GridNode.GridNode>();
			Vector3 nodePosition = new Vector3();

			for (int z = 0 ; z < grid.SizeZ ; ++z)
			{
				nodePosition.z = z;
				for (int x = 0 ; x < grid.SizeX ; ++x)
				{
					nodePosition.x = x;
					GridNode.GridNode node = grid.GetNode(nodePosition);
					if (node)
						node.AddOnChangeType(AddDesination);
				}
			}
		}

		private void AddDesination(GridNode.GridNode node)
		{
			if (node.NodeType != GridNode.ENodeType.District)
				districts.Remove(node);
			else
				districts.Add(node);
		}

		private GridNode.GridNode GetDestination(GridNode.GridNode node)
		{
			int index = Random.Range(0, districts.Count);

			while (districts[index] == node)
				index = Random.Range(0, districts.Count);

			return districts[index];
		}

		public CarBehaviour Spawn(GridNode.GridNode node)
		{
			int index = Random.Range(0, carPrefab.Length);
			GridNode.GridNode destination = GetDestination(node);
			Vector3[] p = Pathfinding.PathFinder.GetPath(node.GetPosition(), destination.GetPosition());
			Vector3 direction = (p[0] - node.GetPosition() + Vector3.up).normalized;
			CarBehaviour obj = Instantiate(carPrefab[index].car, node.GetPosition() + Vector3.up, Quaternion.Euler(direction));

			obj.SetPath(p);

			InvokeOnSpawn(obj);

			return obj;
		}

		#region events
		public delegate void SpawnCar(CarBehaviour car);
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

		public void InvokeOnSpawn(CarBehaviour car)
		{
			OnSpawn?.Invoke(car);
		}
		#endregion
		#endregion
	}
}
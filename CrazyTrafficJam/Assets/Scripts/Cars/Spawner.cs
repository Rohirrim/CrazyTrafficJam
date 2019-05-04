using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace IronSideStudio.CrazyTrafficJam.Car
{
	[CreateAssetMenu(fileName = "Car", menuName = "Car")]
	public class Spawner : ScriptableObject, IInitializable, IUpdatable, ICleanable
	{
		[System.Serializable]
		private struct SCar
		{
			public Driver car;
            [Range(1, 100)]
            public int probability;
			public List<Driver> poolCar;
			public Grid.Node destination;

			public Driver CreateCar(Vector3 position)
			{
				foreach (Driver c in poolCar)
				{
					if (!c.gameObject.activeSelf)
					{
						c.gameObject.SetActive(true);
						c.transform.position = position;
						return c;
					}
				}
				Driver newCar = Instantiate(car, position, Quaternion.identity);
				poolCar.Add(newCar);
				return newCar;
			}

			public void Update()
			{
				foreach (Driver c in poolCar)
				{
					if (c.Enable)
						c.MUpdate();
				}
			}
		}

		[SerializeField]
		private SCar[] carPrefab;
		[SerializeField]
		private List<Grid.Node> districts;

		public bool Enable { get { return true; } }

		public void Initialize()
		{
			Grid.Manager grid = GameplayManager.Instance.GetManager<Grid.Manager>();
			districts = new List<Grid.Node>();
			Vector3 nodePosition = new Vector3();

			for (int z = 0 ; z < grid.SizeZ ; ++z)
			{
				nodePosition.z = z;
				for (int x = 0 ; x < grid.SizeX ; ++x)
				{
					nodePosition.x = x;
					Grid.Node node = grid.GetNode(nodePosition);
					if (node)
						node.AddOnChangeType(AddDestination);
				}
			}

			for (int i = 0 ; i < carPrefab.Length ; ++i)
			{
				carPrefab[i].poolCar = new List<Driver>();
			}
		}

		public void MUpdate()
		{
			foreach (SCar c in carPrefab)
				c.Update();
		}

		public void Clean()
		{
			districts.Clear();
			for (int i = 0 ; i < carPrefab.Length ; ++i)
			{
				carPrefab[i].poolCar.Clear();
			}
		}

		private void AddDestination(Grid.Node node)
		{
			if (node.NodeType != Grid.ENodeType.District)
				districts.Remove(node);
			else
				districts.Add(node);
		}

		private Grid.Node GetDestination(Grid.Node node)
		{
			int index = Random.Range(0, districts.Count);

			while (districts[index] == node)
				index = Random.Range(0, districts.Count);

			return districts[index];
		}

		/*public Driver Spawn(Grid.Node node)
		{
			int index = Random.Range(0, carPrefab.Length);
			Grid.Node destination = GetDestination(node);
			Driver obj = carPrefab[index].CreateCar(node.GetPosition() + Vector3.up * .51f);
			Grid.Node[] p = Pathfinding.PathFinder.GetPath(node.GetPosition(), destination.GetPosition());

			obj.SetPath(p);

			InvokeOnSpawn(obj);

			return obj;
		}*/

        public Driver Spawn(Grid.Node node, Grid.Node destination)
        {
            int index = Random.Range(0, carPrefab.Length);
            //Grid.Node newDestination = destination;
            Driver obj = carPrefab[index].CreateCar(node.GetPosition() + Vector3.up * .51f);

            Grid.Node[] p = Pathfinding.PathFinder.GetPath(node.GetPosition(), destination.GetPosition());

            obj.SetPath(p);

            InvokeOnSpawn(obj);

            return obj;
        }

		#region events
		public delegate void SpawnCar(Driver car);
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

		public void InvokeOnSpawn(Driver car)
		{
			OnSpawn?.Invoke(car);
		}
		#endregion
		#endregion
	}
}
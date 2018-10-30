using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace IronSideStudio.CrazyTrafficJam.Car
{
	public class CarManager : AManager, IInitializable, ICleanable
	{
		[SerializeField]
		private CarSpawner[] allSpawner;
		private List<GameObject> cars;

		public override void Construct()
		{
			cars = new List<GameObject>();
		}

		public void Initialize()
		{
			for (int i = 0 ; i < allSpawner.Length ; ++i)
			{
				allSpawner[i].Init();
				allSpawner[i].AddOnSpawn(AddCar);
			}
			Pathfinding.PathFinder.CreateInstance();
		}

		public void Clean()
		{
			for (int i = 0 ; i < allSpawner.Length ; ++i)
			{
				allSpawner[i].RemoveOnSpawn(AddCar);
			}
			Pathfinding.PathFinder.DeleteInstance();
		}

		private void AddCar(GameObject obj)
		{
			cars.Add(obj);
		}
	}
}
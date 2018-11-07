using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace IronSideStudio.CrazyTrafficJam.Car
{
	public class CarManager : AManager, IInitializable, ICleanable, IUpdatable
	{
		[SerializeField]
		private CarSpawner[] allSpawner;
		private List<CarBehaviour> cars;

		public bool Enable { get { return cars.Count > 0; } }

		public override void Construct()
		{
			cars = new List<CarBehaviour>();
		}

		public void Initialize()
		{
			for (int i = 0 ; i < allSpawner.Length ; ++i)
			{
				allSpawner[i].Initialize();
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

		public void MUpdate()
		{
			foreach (CarBehaviour c in cars)
			{
				if (c.Enable)
					c.MUpdate();
			}
		}

		private void AddCar(CarBehaviour obj)
		{
			cars.Add(obj);
		}
	}
}
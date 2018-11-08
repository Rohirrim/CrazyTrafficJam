using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace IronSideStudio.CrazyTrafficJam.Car
{
	public class CarManager : AManager, IInitializable, ICleanable, IUpdatable
	{
		[SerializeField]
		private CarSpawner[] allSpawner;

		public bool Enable { get { return enabled; } }

		public override void Construct()
		{
		}

		public void Initialize()
		{
			for (int i = 0 ; i < allSpawner.Length ; ++i)
			{
				allSpawner[i].Initialize();
			}
			Pathfinding.PathFinder.CreateInstance();
		}

		public void Clean()
		{
			Pathfinding.PathFinder.DeleteInstance();
		}

		public void MUpdate()
		{
			foreach (CarSpawner c in allSpawner)
			{
				c.MUpdate();
			}
		}
	}
}
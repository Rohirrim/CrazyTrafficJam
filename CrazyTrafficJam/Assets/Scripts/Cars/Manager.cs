using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace IronSideStudio.CrazyTrafficJam.Car
{
	public class Manager : AManager, IInitializable, ICleanable, IUpdatable
	{
		[SerializeField]
		private Spawner[] allSpawner;

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
			foreach (Spawner c in allSpawner)
			{
				c.MUpdate();
			}
		}
	}
}
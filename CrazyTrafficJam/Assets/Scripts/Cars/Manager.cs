using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace IronSideStudio.CrazyTrafficJam.Car
{
	public class Manager : AManager, IInitializable, ICleanable, IUpdatable
	{
        public static Manager Instance;

		[SerializeField]
		private Spawner[] allSpawner;

        public List<DistrictScript> allDistricts;

		public bool Enable { get { return enabled; } }

		public override void Construct()
		{
		}

		public void Initialize()
		{
            Instance = this;

			for (int i = 0 ; i < allSpawner.Length ; ++i)
			{
				allSpawner[i].Initialize();
			}
			Pathfinding.PathFinder.CreateInstance();
		}

		public void Clean()
		{
			for (int i = 0 ; i < allSpawner.Length ; ++i)
			{
				allSpawner[i].Clean();
			}
			Pathfinding.PathFinder.DeleteInstance();
		}

		public void MUpdate()
		{
			foreach (Spawner c in allSpawner)
			{
				c.MUpdate();
			}

            CheckAllDistricts();
		}

        public void UpdateDistrictHour(DayTime currentTime)
        {
            foreach(DistrictScript D in allDistricts)
            {
                D.SetDistrictTraffic(currentTime);
            }
        }

        public void CheckAllDistricts()//Dans le cas où il y a un district
        {
            for(int i = 0; i < allDistricts.Count; i++)
            {
                if(allDistricts[i] == null)
                {
                    allDistricts.Remove(allDistricts[i]);
                }
            }
        }
	}
}
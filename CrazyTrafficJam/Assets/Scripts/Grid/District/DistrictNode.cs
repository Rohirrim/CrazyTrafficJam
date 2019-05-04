using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace IronSideStudio.CrazyTrafficJam.Grid
{
	[CreateAssetMenu(fileName = "DistrictNode", menuName = "GridNodeType/District")]
	public class DistrictNode : NodeType
	{
		[System.Serializable]
		private struct SSpawn
		{
			[EnumFlag]
			[SerializeField]
			public EDay day;
			public Car.Spawner carSpawner;
		}

		[SerializeField]
		private SSpawn[] spawnInfo;

		public override void Initialize()
		{
			base.Initialize();
			//GameplayManager.Instance.GetManager<TimeManager>().AddOnDayPass(Spawn);
		}

		private void Spawn(SDayInfo dayInfo)//OLD
		{
			for (int i = 0 ; i < spawnInfo.Length ; ++i)
			{
				SSpawn spawn = spawnInfo[i];

				if ((dayInfo.day & spawn.day) != 0)
				{
					foreach (SNode n in nodes)
					{
						//spawn.carSpawner.Spawn(n.node);
					}
				}
			}
		}
	}
}
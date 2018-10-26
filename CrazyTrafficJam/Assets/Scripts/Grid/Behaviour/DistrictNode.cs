using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace IronSideStudio.CrazyTrafficJam.GridNode
{
	[CreateAssetMenu(fileName = "DistrictNode", menuName = "GridNodeType/District")]
	public class DistrictNode : GridNodeType
	{
		[System.Serializable]
		private struct SSpawn
		{
			[EnumFlag]
			[SerializeField]
			public EDay day;
			public Car.Car type;
		}

		[SerializeField]
		private SSpawn[] spawnInfo;

		public override void Init()
		{
			base.Init();
			CoreManager.Instance.GetManager<TimeManager>().AddOnDayPass(Spawn);
		}

		private void Spawn(SDayInfo dayInfo)
		{
			for (int i = 0 ; i < spawnInfo.Length ; ++i)
			{
				SSpawn spawn = spawnInfo[i];

				if ((dayInfo.day & spawn.day) != 0)
				{
					foreach (GridNode n in nodes)
					{
						GameObject obj = spawn.type.Spawn();
						obj.transform.position = n.transform.position + Vector3.up;
					}
				}
			}
		}

		protected override void Behaviour(GridNode gridNode)
		{
		}
	}
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace IronSideStudio.CrazyTrafficJam.UI
{
	public class PanelIntersection : MonoBehaviour, IInitializable, ICleanable
	{
		GridNodeSelection nodeSelection;

		public void Initialize()
		{
			GameplayManager.Instance.GetManager<TimeManager>().AddOnWeekPass(OnTimePass);
		}

		public void Clean()
		{
			GameplayManager.Instance.GetManager<TimeManager>().RemoveOnWeekPass(OnTimePass);
		}

		private void OnTimePass(SDayInfo dayInfo)
		{
			if (dayInfo.day != EDay.Monday)
				return;
			gameObject.SetActive(true);
			GameplayManager.Instance.GetManager<TimeManager>().SetTimeScale(0f);
		}
	}
}
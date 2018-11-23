using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace IronSideStudio.CrazyTrafficJam.UI
{
	public class UiTimeElapse : MonoBehaviour, IInitializable, ICleanable
	{
		[SerializeField]
		private Text dayElapse;
		[SerializeField]
		private Text weekElapse;
		[SerializeField]
		private RectTransform pointer;

		public void Initialize()
		{
			dayElapse.text = EDay.Monday.ToString();
			weekElapse.text = "0";

			TimeManager time = GameplayManager.Instance.GetManager<TimeManager>();
			time.AddOnHourPass(TimePass);
		}

		public void Clean()
		{
			TimeManager time = GameplayManager.Instance.GetManager<TimeManager>();
			time.RemoveOnHourPass(TimePass);
		}

		private void TimePass(SDayInfo dayInfo)
		{
			float percent = dayInfo.hour / 24f;

			Vector3 angle = Vector3.zero;
			angle.z = Mathf.Lerp(0, -720, percent) + 90f;
			pointer.eulerAngles = angle;

			dayElapse.text = dayInfo.day.ToString();
			weekElapse.text = dayInfo.week.ToString();
		}
	}
}
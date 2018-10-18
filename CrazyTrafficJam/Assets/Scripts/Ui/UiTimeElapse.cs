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

			TimeManager time = CoreManager.Instance.GetManager<TimeManager>();
			time.AddOnHourPass(HourPass);
			time.AddOnDayPass(DayPass);
			time.AddOnWeekPass(WeekPass);
		}

		public void Clean()
		{
			TimeManager time = CoreManager.Instance.GetManager<TimeManager>();
			time.RemoveOnHourPass(HourPass);
			time.RemoveOnDayPass(DayPass);
			time.RemoveOnWeekPass(WeekPass);
		}

		private void HourPass(float value, EDay currentDay)
		{
			float percent = value / 23.99f;

			Vector3 angle = Vector3.zero;
			angle.z = Mathf.Lerp(0, -360, percent) + 90f;
			pointer.eulerAngles = angle;
		}

		private void DayPass(float value, EDay currentDay)
		{
			dayElapse.text = currentDay.ToString();
		}

		private void WeekPass(float value, EDay currentDay)
		{
			weekElapse.text = value.ToString();
		}
	}
}
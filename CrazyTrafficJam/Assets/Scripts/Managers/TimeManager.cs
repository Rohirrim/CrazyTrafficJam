using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace IronSideStudio.CrazyTrafficJam
{
	public enum EDay
	{
		Monday,
		Tuesday,
		Wednesday,
		Thursday,
		Friday,
		Saturday,
		Sunday
	}

	public class TimeManager : AManager, IInitializable, IUpdatable, ICleanable
	{
		[SerializeField]
		private float dayTime;
		private float currentHour;
		private EDay currentDay;

		private float f;
		private int week;
		private int day;

		public bool Enable { get { return enabled; } }

		public override void Construct()
		{

		}

		public void Initialize()
		{
			day = 0;
			week = 0;
			currentDay = EDay.Monday;
		}

		public void Clean()
		{
			OnHourPass = null;
			OnDayPass = null;
			OnWeekPass = null;
		}

		public void MUpdate()
		{
			currentHour = Mathf.Lerp(0f, 23.99f, f);
			InvokeOnHourPass();

			f += Time.deltaTime / dayTime;
			if (f >= 1f)
			{
				f = 0f;
				++day;
				currentDay = (EDay)(day % 7);
				InvokeOnDayPass();
				if (currentDay == EDay.Monday)
				{
					++week;
					InvokeOnWeekPass();
				}
			}
		}

		#region Events
		public delegate void TimePass(float timeValue, EDay currentDay);
		private event TimePass OnHourPass;
		private event TimePass OnDayPass;
		private event TimePass OnWeekPass;

		#region OnHourPass
		public void AddOnHourPass(TimePass func)
		{
			OnHourPass += func;
		}

		public void RemoveOnHourPass(TimePass func)
		{
			OnHourPass -= func;
		}

		private void InvokeOnHourPass()
		{
			OnHourPass?.Invoke(currentHour, currentDay);
		}
		#endregion

		#region OnDayPass
		public void AddOnDayPass(TimePass func)
		{
			OnDayPass += func;
		}

		public void RemoveOnDayPass(TimePass func)
		{
			OnDayPass -= func;
		}

		private void InvokeOnDayPass()
		{
			OnDayPass?.Invoke(day, currentDay);
		}
		#endregion

		#region OnWeekPass
		public void AddOnWeekPass(TimePass func)
		{
			OnWeekPass += func;
		}

		public void RemoveOnWeekPass(TimePass func)
		{
			OnWeekPass -= func;
		}

		private void InvokeOnWeekPass()
		{
			OnWeekPass?.Invoke(week, currentDay);
		}
		#endregion
		#endregion
	}
}
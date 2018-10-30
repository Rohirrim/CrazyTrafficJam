using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace IronSideStudio.CrazyTrafficJam
{
	public enum EDay
	{
		Monday = 1 << 0,
		Tuesday = 1 << 1,
		Wednesday = 1 << 2,
		Thursday = 1 << 3,
		Friday = 1 << 4,
		Saturday = 1 << 5,
		Sunday = 1 << 6
	}

	public struct SDayInfo
	{
		public int week;
		public EDay day;
		public int hour;
	}

	public class TimeManager : AManager, IInitializable, IUpdatable, ICleanable
	{
		[SerializeField]
		private float dayTime;

		private float t;
		private int day;
		private SDayInfo dayInfo;

		public bool Enable { get { return enabled; } }

		public override void Construct()
		{
			dayInfo = new SDayInfo();
		}

		public void Initialize()
		{
			dayInfo.hour = 0;
			dayInfo.day = EDay.Monday;
			dayInfo.week = 0;

			day = 0;
		}

		public void Clean()
		{
			OnHourPass = null;
			OnDayPass = null;
			OnWeekPass = null;

			dayInfo.hour = 0;
			dayInfo.day = EDay.Monday;
			dayInfo.week = 0;
		}

		public void MUpdate()
		{
			dayInfo.hour = (int)Mathf.Lerp(0, 24, t);
			InvokeOnHourPass();

			t += Time.deltaTime / dayTime;
			if (t >= 1f)
			{
				t = 0f;
				dayInfo.day = (EDay)(1 << ++day);
				if (day == 6)
					day = -1;
				InvokeOnDayPass();
				if (dayInfo.day == EDay.Monday)
				{
					++dayInfo.week;
					InvokeOnWeekPass();
				}
			}
		}

		#region Events
		public delegate void TimePass(SDayInfo dayInfo);
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
			OnHourPass?.Invoke(dayInfo);
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
			OnDayPass?.Invoke(dayInfo);
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
			OnWeekPass?.Invoke(dayInfo);
		}
		#endregion
		#endregion
	}
}
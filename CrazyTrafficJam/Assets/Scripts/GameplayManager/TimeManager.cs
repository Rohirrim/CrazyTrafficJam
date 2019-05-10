using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace IronSideStudio.CrazyTrafficJam
{
    public enum DayTime
    {
        MATIN,
        APRESMIDI,
        SOIREE,
        NUIT,

    }

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

    [Serializable]
	public struct SDayInfo
	{
		public int week;
		public EDay day;
		public float hour;
	}

	public class TimeManager : AManager, IInitializable, IUpdatable, ICleanable
	{
        [Header("Pas touche")]
		[SerializeField]
		private float dayTime;

		public float t;
		public int day;
		public SDayInfo dayInfo;

		public bool Enable => enabled;

        public static TimeManager Instance;

		public override void Construct()
		{
			dayInfo = new SDayInfo();
		}

		public void Initialize()
		{
            Instance = this;
			StopTimer();
			dayInfo.hour = 0f;
			dayInfo.day = EDay.Monday;
			dayInfo.week = 0;

			day = 0;
		}

		public void Clean()
		{
			OnHourPass = null;
			OnDayPass = null;
			OnWeekPass = null;

			dayInfo.hour = 0f;
			dayInfo.day = EDay.Monday;
			dayInfo.week = 0;
		}

		public void MUpdate()
		{
			dayInfo.hour = Mathf.Lerp(0f, 24f, t);
			InvokeOnHourPass();
            IronSideStudio.CrazyTrafficJam.Car.Manager.Instance.UpdateDistrictHour(CurrentDayTime());


            t += Time.deltaTime / dayTime;
			if (t >= 1f)
			{
				t = 0f;
				dayInfo.day = (EDay)(1 << ++day);
				if (day == 6)
					day = -1;
				if (dayInfo.day == EDay.Monday)
				{
					++dayInfo.week;
					InvokeOnWeekPass();
				}
				InvokeOnDayPass();
			}
		}

		public void SetTimeScale(float scale)
		{
			Time.timeScale = scale;
		}

		public void StopTimer()
		{
			enabled = false;
		}

		public void StartTimer()
		{
			enabled = true;
            InvokeOnWeekPass();
		}

        public DayTime CurrentDayTime()
        {
            if(dayInfo.hour >= 6 && dayInfo.hour < 14)
            {
                return DayTime.MATIN;
            }
            else if(dayInfo.hour >= 14 && dayInfo.hour < 19)
            {
                return DayTime.APRESMIDI;
            }
            else if(dayInfo.hour >= 19 && dayInfo.hour < 22)
            {
                return DayTime.SOIREE;
            }
            else
            {
                return DayTime.NUIT;
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
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace IronSideStudio.CrazyTrafficJam
{
	public class GameplayManager : AManager, IInitializable, IUpdatable, ICleanable
	{
		[SerializeField]
		private AManager[] managers;
		private IUpdatable[] updatableManager;
		private IInitializable[] initializableManager;
		private ICleanable[] cleanableManager;

		public bool Enable => enabled;

		public override void Construct()
		{
			AManager[] comp = GetComponents<AManager>();
			managers = new AManager[comp.Length - 1];

			for (int i = 1 ; i < comp.Length ; ++i)
			{
				managers[i - 1] = comp[i];
				managers[i - 1].Construct();
			}

			updatableManager = GetInterface<IUpdatable>();
			initializableManager = GetInterface<IInitializable>();
			cleanableManager = GetInterface<ICleanable>();
		}

		private T[] GetInterface<T>() where T : class
		{
			List<T> templateList = new List<T>();

			for (int i = 0 ; i < managers.Length ; ++i)
			{
				T t = managers[i] as T;
				if (t != null)
					templateList.Add(t);

			}
			return templateList.ToArray();
		}

		public void Initialize()
		{
			for (int i = 0 ; i < initializableManager.Length ; ++i)
			{
				initializableManager[i].Initialize();
			}
			enabled = updatableManager.Length > 0;
			CoreManager.Instance.GetManager<TimeManager>().StartTimer();
		}

		public void MUpdate()
		{
			for (int i = 0 ; i < updatableManager.Length ; ++i)
			{
				if (updatableManager[i].Enable)
					updatableManager[i].MUpdate();
			}
		}

		public void Clean()
		{
			for (int i = 0 ; i < cleanableManager.Length ; ++i)
			{
				cleanableManager[i].Clean();
			}
		}
	}
}
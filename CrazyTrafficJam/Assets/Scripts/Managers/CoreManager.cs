using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace IronSideStudio.CrazyTrafficJam
{
	public class CoreManager : MonoBehaviour
	{
		[SerializeField]
		private AManager[] managers;
		private IUpdatable[] updatableManager;
		private IInitializable[] initializableManager;

		private static CoreManager instance;
		public static CoreManager Instance => instance;

		private void Awake()
		{
			instance = this;

			for (int i = 0 ; i < managers.Length ; ++i)
			{
				managers[i].Construct();
			}

			updatableManager = GetUpdatable();
			initializableManager = GetInitializable();
		}

		private IUpdatable[] GetUpdatable()
		{
			List<IUpdatable> templateList = new List<IUpdatable>();

			for (int i = 0 ; i < managers.Length ; ++i)
			{
				IUpdatable t = managers[i] as IUpdatable;
				if (t != null)
					templateList.Add(t);

			}
			return templateList.ToArray();
		}

		private IInitializable[] GetInitializable()
		{
			List<IInitializable> templateList = new List<IInitializable>();

			for (int i = 0 ; i < managers.Length ; ++i)
			{
				IInitializable t = managers[i] as IInitializable;
				if (t != null)
					templateList.Add(t);

			}
			return templateList.ToArray();
		}

		private void Start()
		{
			for (int i = 0 ; i < initializableManager.Length ; ++i)
			{
				initializableManager[i].Initialize();
			}
			enabled = updatableManager.Length > 0;
		}

		private void Update()
		{
			for (int i = 0 ; i < updatableManager.Length ; ++i)
			{
				if (updatableManager[i].Enable)
					updatableManager[i].MUpdate();
			}
		}

		public T GetManager<T>() where T : AManager
		{
			T searchManager = null;

			for (int i = 0 ; i < managers.Length ; ++i)
			{
				searchManager = managers[i] as T;
				if (searchManager != null)
					return searchManager;
			}
			return null;
		}
	}
}
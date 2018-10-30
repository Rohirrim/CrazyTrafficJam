﻿using System.Collections;
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
		private ICleanable[] cleanableManager;

		private static CoreManager instance;
		public static CoreManager Instance => instance;

		private void Awake()
		{
			instance = this;

			for (int i = 0 ; i < managers.Length ; ++i)
			{
				managers[i].Construct();
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

		private void OnDestroy()
		{
			for (int i = 0 ; i < cleanableManager.Length ; ++i)
			{
				cleanableManager[i].Clean();
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
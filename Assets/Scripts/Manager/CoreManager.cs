using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace IronSideStudio.CrazyTrafficJam
{
	public sealed class CoreManager : Manager
	{
		#region Managers 
		[SerializeField]
		private List<Manager> managers = new List<Manager>();
		[SerializeField]
		private List<Manager> prefabManagers;
		#endregion

		private void Start()
		{
			Init();
		}

		#region Singleton
		//public CoreManager Instance => instance;
		public static CoreManager Instance => instance;
		private static CoreManager instance = null;
		void Awake()
		{
			if (instance == null)
				instance = this;

			else if (instance != this)
				Destroy(gameObject);

			DontDestroyOnLoad(gameObject);
		}
		#endregion
		#region Manager
		public override bool Init()
		{
			InstantiateManagers();
			InitManagers();

			return IsInit = true;
		}

		public override void Clear()
		{
			foreach (Manager m in managers)
				m.Clear();
		}
		#endregion
		#region Manager Management
		public T GetManager<T>() where T : Manager
		{
			T tmp = null;
			foreach (var manager in managers)
			{
				tmp = manager as T;
				if (tmp != null)
				{
					break;
				}
			}
			return tmp;
		}
		#endregion
		#region InitManagers 
		private void InstantiateManagers()
		{
			Manager tmp;
			foreach (Manager prefabManager in prefabManagers)
			{
				tmp = Instantiate(prefabManager, transform);
				managers.Add(tmp);
			}

			foreach (Manager manager in managers)
			{
				manager.name = manager.GetType().Name;
				manager.AddOnInit(ManagerInit);
			}
		}

		private void InitManagers()
		{
			foreach (Manager manager in managers)
			{
				manager.Init();
			}
		}

		private void ManagerInit(string name, bool isReady)
		{
			if (!isReady)
				throw new ManagerException("Initialization fail on " + name);

			foreach (Manager manager in managers)
			{
				if (!manager.IsInit)
					return;
			}

			CallbackManagersInit();
		}

		private void CallbackManagersInit()
		{
			Debug.Log("All managers Init");
			SceneManager.LoadScene(Constante.Scene.MainMenu);
		}
		#endregion

		private void Update()
		{
			foreach (var manager in managers)
			{
				manager.MUpdate();
			}
		}

		private void FixedUpdate()
		{
			foreach (var manager in managers)
			{
				manager.MFixedUpdate();
			}
		}

		private void LateUpdate()
		{
			foreach (var manager in managers)
			{
				manager.MLateUpdate();
			}
		}
	}
}
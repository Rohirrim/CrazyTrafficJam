using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace IronSideStudio.CrazyTrafficJam
{
	public class Manager : MonoBehaviour
	{
		protected bool isInit;
		public bool IsInit
		{
			get
			{
				return isInit;
			}
			protected set
			{
				isInit = value;
				InvokeOnInit(GetType().Name, isInit);
			}
		}

		[SerializeField]
		protected CoreManager core;

		public virtual void MUpdate()
		{

		}

		public virtual void MFixedUpdate()
		{

		}

		public virtual void MLateUpdate()
		{

		}

		public virtual void Clear()
		{

		}

		public virtual bool Init()
		{
			core = CoreManager.Instance;
			enabled = false;
			return true;
		}

		#region Events
		#region OnInit
		public delegate void InitEvent(string name, bool value);
		private event InitEvent OnInit;
		public void AddOnInit(InitEvent func)
		{
			OnInit += func;
		}

		public void RemoveOnInit(InitEvent func)
		{
			OnInit -= func;
		}

		private void ResetOnInit()
		{
			OnInit = null;
		}

		private void InvokeOnInit(string name, bool value)
		{
			OnInit?.Invoke(name, value);
		}
		#endregion
		#endregion
	}
}
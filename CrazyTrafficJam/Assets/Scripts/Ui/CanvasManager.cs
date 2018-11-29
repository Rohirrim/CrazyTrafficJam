using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace IronSideStudio.CrazyTrafficJam.UI
{
	public class CanvasManager : AManager, IInitializable, ICleanable
	{
		private IInitializable[] compInitializable;
		private ICleanable[] compCleanable;

		public override void Construct()
		{
			compInitializable = this.GetComponentsOnlyInChildren<IInitializable>(false, true);
			compCleanable = this.GetComponentsOnlyInChildren<ICleanable>(false, true);
		}

		public void Initialize()
		{
			for (int i = 0 ; i < compInitializable.Length ; ++i)
			{
				compInitializable[i].Initialize();
			}
		}

		public void Clean()
		{
			for (int i = 0 ; i < compCleanable.Length ; ++i)
			{
				compCleanable[i].Clean();
			}
		}
	}
}
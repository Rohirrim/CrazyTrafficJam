using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace IronSideStudio.CrazyTrafficJam.GridNode
{
	public abstract class IGridBehaviour : ScriptableObject
	{
		public abstract void Behaviour(GridNode n);
	}
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace IronSideStudio.CrazyTrafficJam
{
	public interface IUpdatable
	{
		bool Enable { get; }

		void MUpdate();
	}
}
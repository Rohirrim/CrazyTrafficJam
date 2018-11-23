using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace IronSideStudio.CrazyTrafficJam
{
	public interface IDriveable
	{
		Vector3[] GetWaypoint(Vector3 from, Vector3 to);

		bool CanDrive(Car.Driver driver);
	}
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace IronSideStudio.CrazyTrafficJam.Grid
{
	public class SimpleRoad : MonoBehaviour, IDriveable
	{
		[SerializeField]
		protected Transform straight;
		[SerializeField]
		protected Transform right;
		[SerializeField]
		protected Transform left;

		public Vector3[] GetWaypoint(Vector3 from, Vector3 to)
		{
			List<Vector3> waypoints = new List<Vector3>();
			Vector3 previousDirection = (transform.position - from).normalized;
			Vector3 nextDirection = (to - transform.position).normalized;
			previousDirection.y = 0f;
			nextDirection.y = 0f;

			if (previousDirection == nextDirection)
			{
				straight.forward = previousDirection;
				foreach (Transform way in straight)
				{
					waypoints.Add(way.position + Quaternion.Euler(0f, 90f, 0f) * nextDirection * Constante.Gameplay.roadSpace);
				}
			}
			else if (Quaternion.Euler(0f, 90f, 0f) * previousDirection == nextDirection)
			{
				right.forward = previousDirection;
				foreach (Transform way in right)
				{
					waypoints.Add(way.position + Quaternion.Euler(0f, 90f, 0f) * previousDirection * Constante.Gameplay.roadSpace);
				}
			}
			else
			{
				left.forward = previousDirection;
				foreach (Transform way in left)
				{
					waypoints.Add(way.position + Quaternion.Euler(0f, 90f, 0f) * previousDirection * Constante.Gameplay.roadSpace);
				}
			}

			return waypoints.ToArray();
		}

		public virtual bool CanDrive(Car.Driver driver)
		{
			return true;
		}
	}
}
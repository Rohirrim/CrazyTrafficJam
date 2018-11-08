using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace IronSideStudio.CrazyTrafficJam.Car
{
	public class CarBehaviour : MonoBehaviour, IUpdatable
	{
		[SerializeField]
		private float speed;
		private Vector3 direction;

		private Vector3[] pathToDestination;
		private Vector3 currentDestination;
		private int index;

		public bool Enable { get { return index < pathToDestination.Length; } }

		public void MUpdate()
		{
			Move();
			CheckDestination();
		}

		private void Move()
		{
			transform.position += direction * speed * Time.deltaTime;
		}

		private void CheckDestination()
		{
			if (Vector3.Distance(transform.position, currentDestination) < 0.01f)
			{
				transform.position = pathToDestination[index++];
				if (Enable)
					NextStep();
				else
					gameObject.SetActive(false);
			}
		}

		private void NextStep()
		{
			currentDestination = pathToDestination[index];
			direction = (currentDestination - transform.position).normalized;
			direction.y = 0f;
			transform.forward = direction;

			Vector3 rightDirection = Quaternion.Euler(0f, 90f, 0f) * direction;
			transform.position += rightDirection * .35f;
			currentDestination += rightDirection * .35f;
		}

		public void SetPath(Vector3[] path)
		{
			pathToDestination = path;
			index = 0;
			NextStep();
		}
	}
}
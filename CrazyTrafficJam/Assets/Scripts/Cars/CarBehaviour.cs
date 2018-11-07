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
			transform.position += direction * speed * Time.deltaTime;

			if (Vector3.Distance(transform.position, currentDestination) < 0.01f)
			{
				transform.position = pathToDestination[index++];
				if (Enable)
					NextStep();
				gameObject.SetActive(Enable);
			}
		}

		private void NextStep()
		{
			currentDestination = pathToDestination[index];
			direction = (currentDestination - transform.position).normalized;
			transform.forward = direction;
			/*
			transform.position += transform.right * .35f;
			currentDestination += transform.right * .35f;
			/**/
		}

		public void SetPath(Vector3[] path)
		{
			pathToDestination = path;
			index = 0;
			NextStep();
		}
	}
}
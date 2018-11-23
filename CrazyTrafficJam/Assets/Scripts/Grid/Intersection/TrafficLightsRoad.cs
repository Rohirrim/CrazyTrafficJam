using System.Collections;
using System.Collections.Generic;
using IronSideStudio.CrazyTrafficJam.Car;
using UnityEngine;

namespace IronSideStudio.CrazyTrafficJam.Grid
{
	public class TrafficLightsRoad : SimpleRoad, IUpdatable
	{
		[SerializeField]
		private Renderer topLight;
		[SerializeField]
		private Renderer bottomLight;
		[SerializeField]
		private Renderer leftLight;
		[SerializeField]
		private Renderer rightLight;

		private float currentTime;
		[SerializeField]
		private float switchTime;
		private bool driveTop;

		public bool Enable => true;

		public void MUpdate()
		{
			currentTime -= Time.deltaTime;
			if (currentTime < 0f)
			{
				currentTime = switchTime;
				SwitchLight();
			}
		}

		public override bool CanDrive(Driver driver)
		{
			Renderer rendLight = topLight;
			float distance = Vector3.Distance(transform.position, rendLight.transform.position);

			if (Vector3.Distance(transform.position, bottomLight.transform.position) < distance)
			{
				rendLight = bottomLight;
				distance = Vector3.Distance(transform.position, rendLight.transform.position);
			}
			if (Vector3.Distance(transform.position, leftLight.transform.position) < distance)
			{
				rendLight = leftLight;
				distance = Vector3.Distance(transform.position, rendLight.transform.position);
			}
			if (Vector3.Distance(transform.position, rightLight.transform.position) < distance)
			{
				rendLight = rightLight;
				distance = Vector3.Distance(transform.position, rendLight.transform.position);
			}

			return rendLight.material.color == Color.green;
		}

		private void SwitchLight()
		{
			driveTop = !driveTop;
			topLight.material.color = driveTop ? Color.red : Color.green;
			bottomLight.material.color = driveTop ? Color.red : Color.green;
			leftLight.material.color = !driveTop ? Color.red : Color.green;
			rightLight.material.color = !driveTop ? Color.red : Color.green;
		}
	}
}